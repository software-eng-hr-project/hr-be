using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using ProjectHr.Authorization;
using ProjectHr.Authorization.Accounts;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;
using ProjectHr.Roles.Dto;
using ProjectHr.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHr.Common.Errors;
using ProjectHr.Common.Exceptions;
using ProjectHr.JobTitles.Dto;

namespace ProjectHr.Users
{   
    [AbpAuthorize(PermissionNames.Pages_Users)]
    [Route("/api/users")]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IRepository<User, long> _userRepository;

        public UserAppService(
            IRepository<User, long> userRepository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager)
            : base(userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _userRepository = userRepository;
        }
        
        [HttpPost]
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }
        [RemoteService(false)]
        [HttpPut]
        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }
        [HttpDelete]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }
        
        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        [HttpPost("activate")]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }
        [HttpPost("de-activate")]
        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }
        [HttpGet("get-roles")]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }
        [RemoteService(false)]
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }
        
        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }
        
        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }
        
        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();
        
            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);
        
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
        
            return userDto;
        }
        
        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }
        
        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);
        
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }
        
            return user;
        }
        
        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }
        
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        [HttpPut("change-password")]
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
        
            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }
            
            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }
        
            return true;
        }
        [HttpPut("reset-password")]
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }
            
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }
        
            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        
            return true;
        }
        
        //ToDo Authentice olmuş userin id si gelecek id kalkacal yani
        [HttpPut("additional-info")]
        public async Task<UserDto> UpdateAdditionalInfo( UserOwnUpdateDto input) 
        {
            var user = _userRepository.FirstOrDefault(u => u.Id == input.Id);

            ObjectMapper.Map(input, user);
            
            await _userRepository.UpdateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();

            var userDto = ObjectMapper.Map<UserDto>(user);
            return userDto;
        }
        
        [HttpGet]
        public async Task<List<UserDto>> GetAll(PagedUserResultRequestDto input)
        {
            var users = _userRepository.GetAll()
                .Include(x => x.Roles)
                .Include(u => u.JobTitle);

            
            var roles = await _roleRepository.GetAllListAsync();
            
            var userDtos = ObjectMapper.Map<List<UserDto>>(users);
            

            foreach (var userDto in userDtos)
            {
                var roleIds = users.First(x => x.Id == userDto.Id).Roles.Select(x => x.RoleId);
                userDto.RoleNames = roles.Where(x => roleIds.Any(y => y == x.Id)).Select(x=>x.Name).ToArray();
            }
            
            
        
            return userDtos;
        }
        [HttpGet("{userId}")]
        public async Task<UserDto> Get(long userId)
        {
            var user = _userRepository.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefault(x => x.Id == userId);
        
            if (user == null)
                throw ExceptionHelper.Create(ErrorCode.UserCannotFound);
            
            var roles = await _roleRepository.GetAllListAsync();
            
            var userDtos = ObjectMapper.Map<UserDto>(user);
            
            var roleIds = user.Roles.Select(x => x.RoleId);

            userDtos.RoleNames = roles.Where(x => roleIds.Any(y => y == x.Id)).Select(x=>x.Name).ToArray();
            return userDtos;
        }
        
        [RemoteService(false)]
        [HttpGet("/aaaaaaaaa/aaaaaaaaaaa{duplicategetallhatasiveriyordu}")]
        public override Task<UserDto> GetAsync(EntityDto<long> id)
        {
            return base.GetAsync(id);
        }

        [RemoteService(false)]
        [HttpGet("duplicategetallhatasiveriyordu")] //hata verdiği için templateye bir şey vermemiz gerekti
        public override Task<PagedResultDto<UserDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            return base.GetAllAsync(input);
        }
    }
}

