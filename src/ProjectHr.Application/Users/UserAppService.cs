using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Http;
using ProjectHr.Authorization;
using ProjectHr.Authorization.Accounts;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;
using ProjectHr.Roles.Dto;
using ProjectHr.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectHr.Common.Errors;
using ProjectHr.Common.Exceptions;
using ProjectHr.Entities;
using ProjectHr.DataAccess.Dto;
using ProjectHr.ExportFiles;
using ProjectHr.ProjectMembers.Dto;
using ProjectHr.Projects.Dto;

namespace ProjectHr.Users
{
    [AbpAuthorize]
    [Route("/api/users")]
    public class UserAppService :
        AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<EmployeeLayoffInfo> _employeeLayoffInfoRepository;
        private readonly IMailService _mailService;

        public UserAppService(
            IRepository<User, long> userRepository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Project> projectRepository,
            IRepository<EmployeeLayoffInfo> employeeLayoffInfoRepository,
            IMailService mailService
        )
            : base(userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
            _employeeLayoffInfoRepository = employeeLayoffInfoRepository;
            _mailService = mailService;
            _userRepository = userRepository;
        }

        [AbpAuthorize(PermissionNames.Create_User)]
        [HttpPost]
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            try
            {
                CheckCreatePermission();

                var user = ObjectMapper.Map<User>(input);

                user.TenantId = AbpSession.TenantId;
                user.UserName = Guid.NewGuid().ToString();
                user.IsEmailConfirmed = true;
                user.IsActive = true;
                user.IsInvited = true;

                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

                var password = Guid.NewGuid().ToString();

                CheckErrors(await _userManager.CreateAsync(user, password));

                if (input.RoleNames != null)
                {
                    CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
                }

                CurrentUnitOfWork.SaveChanges();

                await _mailService.InviteUserMail(user.EmailAddress);

                return MapToEntityDto(user);
            }
            catch (Exception e)
            {
                ErrorCodeHelpers.DuplicateMessageHelper(e);
                throw e;
            }
        }

        [AbpAuthorize(PermissionNames.Delete_User)]
        [HttpDelete]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize(PermissionNames.ActiveOrDisabled_User)]
        [HttpPost("activate/{userId}")]
        public async Task Activate(long userId)
        {
            await Repository.UpdateAsync(userId, async (entity) =>
            {
                entity.IsActive = true;
                var deletedEmployeeLayoffInfo =
                    _employeeLayoffInfoRepository.FirstOrDefault(x => x.Id == entity.EmployeeLayoffInfoId);
                await _employeeLayoffInfoRepository.DeleteAsync(deletedEmployeeLayoffInfo);
                entity.EmployeeLayoffInfoId = null;
            });
        }

        [AbpAuthorize(PermissionNames.ActiveOrDisabled_User)]
        [HttpPost("de-activate/{userId}")]
        public async Task DeActivate(EmployeeLayoffInfoDto input, long userId)
        {
            var layoffInfo = new EmployeeLayoffInfo()
            {
                EmployeeLayoffId = input.EmployeeLayoffId,
                LayoffReason = input.LayoffReason,
                DismissalDate = input.DismissalDate,
            };
            await _employeeLayoffInfoRepository.InsertAsync(layoffInfo);
            await CurrentUnitOfWork.SaveChangesAsync();
            await Repository.UpdateAsync(userId, async (entity) =>
            {
                entity.IsActive = false;
                entity.EmployeeLayoffInfoId = layoffInfo.Id;
            });
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
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                    x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) ||
                         x.EmailAddress.Contains(input.Keyword))
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
                throw ExceptionHelper.Create(ErrorCode.UserCannotFound);
            }

            if (!await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                throw new UserFriendlyException("Mevcut şifreniz yanlış");
            }

            if (input.CurrentPassword == input.NewPassword)
            {
                throw new UserFriendlyException("Yeni şifreniz mevcut ile aynı olamaz");
            }

            if (!IsPasswordValid(input.NewPassword))
            {
                throw new UserFriendlyException(
                    "Şifreniz en az 6 karakter, en az bir büyük harf, bir küçük harf ve bir sayı içermelidir.");
            }

            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Yanlis sifre."
                }));
            }

            return true;
        }

        [AbpAllowAnonymous]
        [HttpPut("reset-password")]
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.PasswordResetCode == input.Token);

            if (user is null)
            {
                throw ExceptionHelper.Create(ErrorCode.ResetTokenAlreadyUsed);
            }

            if (!IsPasswordValid(input.NewPassword))
            {
                throw new UserFriendlyException(
                    "Şifreniz en az 6 karakter, en az bir büyük harf, bir küçük harf ve bir sayı içermelidir.");
            }

            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            user.PasswordResetCode = null;
            await CurrentUnitOfWork.SaveChangesAsync();

            return true;
        }

        private bool IsPasswordValid(string password)
        {
            // En az 6 karakter, en az bir büyük harf, bir küçük harf ve bir sayı içeren regex kontrolü
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$");
            return regex.IsMatch(password);
        }

        [AbpAuthorize(PermissionNames.Update_Info_User)]
        [HttpPut("{userId}")]
        public async Task<UserDto> UpdateAllInfo(UserAllUpdateDto input, long userId)
        {
            try
            {
                var user = _userRepository.GetAll()
                    .Include(u => u.Roles)
                    .Include(u => u.JobTitle)
                    .FirstOrDefault(u => u.Id == userId);

                await _userManager.SetRolesAsync(user, input.RoleNames);

                ObjectMapper.Map(input, user);

                await _userRepository.UpdateAsync(user);
                await CurrentUnitOfWork.SaveChangesAsync();

                var userDto = MapToEntityDto(user);
                return userDto;
            }
            catch (Exception e)
            {
                ErrorCodeHelpers.DuplicateMessageHelper(e);

                throw e;
            }
        }

        [AbpAuthorize(PermissionNames.Update_Info_User)]
        [HttpPut("{userId}/role")]
        public async Task<UserDto> UpdateRole(UserRoleUpdateDto input, long userId)
        {
            var user = _userRepository.GetAll()
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == userId);

            await _userManager.SetRolesAsync(user, input.RoleNames);

            await _userRepository.UpdateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();

            var userDto = MapToEntityDto(user);
            return userDto;
        }

        [HttpPut("profile")]
        public async Task<UserDto> UpdateOwnInfo(UserOwnUpdateDto input)
        {
            try
            {
                var abpSessionUserId = AbpSession.GetUserId();
                var user = _userRepository.GetAll()
                    .Include(u => u.Roles)
                    .Include(u => u.JobTitle)
                    .FirstOrDefault(u => u.Id == abpSessionUserId);

                ObjectMapper.Map(input, user);

                await _userRepository.UpdateAsync(user);
                await CurrentUnitOfWork.SaveChangesAsync();

                var userDto = MapToEntityDto(user);
                return userDto;
            }
            catch (Exception e)
            {
                ErrorCodeHelpers.DuplicateMessageHelper(e);
                throw e;
            }
        }


        // public async Task
        [HttpGet]
        public async Task<List<GetUserGeneralInfo>> GetAll(PagedUserResultRequestDto input)
        {
            var users = _userRepository.GetAll()
                .OrderBy(u => u.Name)
                .Include(u => u.JobTitle)
                .Include(u => u.ProjectMembers);

            users.Select(u => u.ProjectMembers.Select(pm => pm.ProjectId));


            var userDtos = ObjectMapper.Map<List<GetUserGeneralInfo>>(users);
            foreach (var getUserGeneralInfo in userDtos)
            {
                getUserGeneralInfo.Projects = _projectRepository.GetAll()
                    .Where(p => p.ProjectMembers.Any(pm => pm.UserId == getUserGeneralInfo.Id))
                    .Select(p => p.Name)
                    .ToArray();
            }

            return userDtos;
        }

        [HttpGet("profile")]
        public async Task<UserDto> GetProfile()
        {
            var abpSessionUserId = AbpSession.GetUserId();

            var user = _userRepository.GetAll()
                .Include(x => x.Roles)
                .Include(x => x.JobTitle)
                .FirstOrDefault(x => x.Id == abpSessionUserId);

            if (user == null)
                throw ExceptionHelper.Create(ErrorCode.UserCannotFound);

            var roles = await _roleRepository.GetAllListAsync();

            var userDtos = ObjectMapper.Map<UserDto>(user);

            var roleIds = user.Roles.Select(x => x.RoleId);

            userDtos.RoleNames = roles.Where(x => roleIds.Any(y => y == x.Id)).Select(x => x.Name).ToArray();
            

            return userDtos;
        }

        [HttpGet("profile/career")]
        public async Task<List<ProjectWithUserDto>> GetProfileCareerInfo()
        {
            var abpSessionUserId = AbpSession.GetUserId();
            
            var userProjects = await _projectRepository.GetAll()
                .Include(p => p.ProjectMembers)
                .ThenInclude(p => p.JobTitle)
                .Include(p => p.ProjectMembers)
                .ThenInclude(p => p.User)
                .Where(p => p.ProjectMembers.Any(pm => pm.UserId == abpSessionUserId)).ToListAsync();
            var userProjectsDto = ObjectMapper.Map<List<ProjectWithUserDto>>(userProjects);
            return userProjectsDto;
        }

        [AbpAuthorize(PermissionNames.View_Info_User)]
        [HttpGet("{userId}")]
        public async Task<UserDto> GetUserByIdAdmin(long userId)
        {
            var user = _userRepository.GetAll()
                .Include(x => x.Roles)
                .Include(x => x.JobTitle)
                .FirstOrDefault(x => x.Id == userId);

            if (user == null)
                throw ExceptionHelper.Create(ErrorCode.UserCannotFound);

            var roles = await _roleRepository.GetAllListAsync();

            var userDtos = ObjectMapper.Map<UserDto>(user);

            var roleIds = user.Roles.Select(x => x.RoleId);

            userDtos.RoleNames = roles.Where(x => roleIds.Any(y => y == x.Id)).Select(x => x.Name).ToArray();
            return userDtos;
        }

        [HttpGet("profile/{userId}")]
        public async Task<GetUserGeneralInfo> GetUserById(long userId)
        {
            var user = _userRepository.GetAll()
                .Include(x => x.JobTitle)
                .FirstOrDefault(x => x.Id == userId);

            if (user == null)
                throw ExceptionHelper.Create(ErrorCode.UserCannotFound);

            var userDtos = ObjectMapper.Map<GetUserGeneralInfo>(user);

            return userDtos;
        }

        [AbpAuthorize(PermissionNames.List_Role)]
        [HttpGet("users-with-role")]
        public async Task<List<UserDto>> GetAllUsersWithRole(PagedUserResultRequestDto input)
        {
            var users = await _userRepository.GetAll()
                .OrderBy(u => u.Name)
                .Include(x => x.Roles)
                .Include(x => x.JobTitle)
                .ToListAsync();

            var roles = await _roleRepository.GetAllListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var userDto = ObjectMapper.Map<UserDto>(user);

                var roleIds = user.Roles.Select(x => x.RoleId);

                userDto.RoleNames = roles.Where(x => roleIds.Any(y => y == x.Id)).Select(x => x.Name).ToArray();

                userDtos.Add(userDto);
            }

            return userDtos;
        }

        [AbpAllowAnonymous]
        [HttpPost("email-check")]
        public async Task<ResetPasswordMailInput> UserEmailCheck(ResetPasswordMailInput input)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.EmailAddress == input.EmailAddress);
            if (user is null)
            {
                throw ExceptionHelper.Create(ErrorCode.EmailCannotFound);
            }

            return ObjectMapper.Map<ResetPasswordMailInput>(user);
        }


        #region OverridePart

        [RemoteService(false)]
        [HttpGet("get-roles")]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        [RemoteService(false)]
        [HttpGet("todo-remove-2{id}")] // template name vermeyince duplicate hatası alıyoruz o yüzden default bir değer atadık 
        public override Task<UserDto> GetAsync(EntityDto<long> id)
        {
            return base.GetAsync(id);
        }

        [RemoteService(false)]
        [HttpGet("todo-remove-1")] //hata verdiği için templateye bir şey vermemiz gerekti
        public override Task<PagedResultDto<UserDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            return base.GetAllAsync(input);
        }

        [RemoteService(false)]
        [HttpPut("todo-remove-3")]
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

        [RemoteService(false)]
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        #endregion
    }
}