﻿using System;
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
using ProjectHr.JobTitles.Dto;

namespace ProjectHr.Users
{   
    [AbpAuthorize]
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
        private readonly SESOptions _sesOptions;
        
        public UserAppService(
            IRepository<User, long> userRepository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IOptions<SESOptions> sesOptions,
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
            _sesOptions = sesOptions.Value;
        }
        
        [HttpPost]
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
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

            return MapToEntityDto(user);
        }
        
        [AbpAuthorize(PermissionNames.Pages_Users_Delete)]
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
        [HttpPut("reset-password")]
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            var user = _userRepository.FirstOrDefault(u => u.PasswordResetToken == input.Token);
            
            // var result = await _userManager.ResetPasswordAsync(user, input.Token, input.NewPassword);
            
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                user.PasswordResetToken = null;
                if(user.IsInvited)
                    user.IsInvited = false;
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        
            return true;
        }
        [AbpAllowAnonymous]
        [HttpPost("reset-password-email/send")]
        public async Task<string> ResetPasswordMail(ResetPasswordMailInput input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                var users = _userRepository.GetAll().Where(x => x.EmailAddress == input.EmailAddress).ToList();

                if (users.Count == 0)
                {
                    throw new UserFriendlyException("There is no user registered with this email!");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(users.FirstOrDefault());

                foreach (var user in users)
                {
                    user.PasswordResetCode = token;
                    await _userRepository.UpdateAsync(user);
                }

                var link = _sesOptions.ClientURL
                var linkWithToken = string.Format($"{link}{_sesOptions.ResetPasswordPath}", token);
                var logo = string.Format($"{_sesOptions.GcsLogoUrl}");

                var template = _sesService.GetEmailTemplate(EmailType.PasswordReset, new Dictionary<string, string>()
                {
                    { "#link_with_token", linkWithToken },
                    { "#gcs_logo", logo },
                });


                var mail = new SendMailModel
                {
                    To = users.FirstOrDefault().EmailAddress,
                    Body = template,
                    Subject = "Reset Password",
                    LinkWithToken = linkWithToken
                };

                await _sesService.SendMail(mail);
            }
        }
        [AbpAuthorize(PermissionNames.Pages_Users_Update_All_Infos)]
        [HttpPut("{userId}")]
        public async Task<UserDto> UpdateAllInfo( UserAllUpdateDto input, long userId)
        {
            var user = _userRepository.GetAll()
                .Include(u => u.Roles)
                .Include(u=> u.JobTitle)
                .FirstOrDefault( u => u.Id == userId);

            await _userManager.SetRolesAsync(user, input.RoleNames);

            ObjectMapper.Map(input, user);

            await _userRepository.UpdateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();

            var userDto = MapToEntityDto(user);
            return userDto;
        }
        
        [HttpPut("profile")]
        public async Task<UserDto> UpdateOwnInfo( UserOwnUpdateDto input) 
        {
            var abpSessionUserId = AbpSession.GetUserId();
            var user = _userRepository.GetAll()
                .Include(u => u.Roles)
                .Include(u=> u.JobTitle)
                .FirstOrDefault(u => u.Id == abpSessionUserId);

            ObjectMapper.Map(input, user);
            
            await _userRepository.UpdateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();

            var userDto = MapToEntityDto(user);
            return userDto;
        }
        
        
        // public async Task
        
        [HttpGet]
        public async Task<List<GetUserGeneralInfo>> GetAll(PagedUserResultRequestDto input)
        {
            var users = _userRepository.GetAll()
                .Include(u => u.JobTitle);
            //
            // var abpSessionUserId = AbpSession.GetUserId();
            // var user = users.FirstOrDefault(u => u.Id == abpSessionUserId);
            

            // var roles = await _roleRepository.GetAllListAsync();
            // var roles =  _roleRepository.GetAllIncluding(r=> r.Permissions); // ROL CHECK YAPARKEN GETALLLİST YERİNE BUNU YAZACAZ


            // var hasPermissions = roles.FirstOrDefault(r => r.Id == user.Roles.ToList()[0].RoleId).Permissions
            //     .Where(p => p.Name == PermissionNames.Pages_Users_Read_All_Infos).ToList().Count != 0;
            //
            // if (!hasPermissions)
            // {
            //     var limitedInfos = ObjectMapper.Map<List<CreateUserDto>>(users);
            //     var limitedInfosDto = ObjectMapper.Map<List<UserDto>>(limitedInfos);
            //     // foreach (var limitedInfo in limitedInfosDto)
            //     // {
            //     //     var roleIds = users.First(x => x.Id == limitedInfo.Id).Roles.Select(x => x.RoleId);
            //     //     limitedInfo.RoleNames = roles.Where(x => roleIds.Any(y => y == x.Id)).Select(x=>x.Name).ToArray();
            //     // }
            //
            //     return limitedInfosDto;
            // }
            var userDtos = ObjectMapper.Map<List<GetUserGeneralInfo>>(users);
            
            // foreach (var userDto in userDtos)
            // {
            //     var roleIds = users.First(x => x.Id == userDto.Id).Roles.Select(x => x.RoleId);
            //     userDto.RoleNames = roles.Where(x => roleIds.Any(y => y == x.Id)).Select(x=>x.Name).ToArray();
            // }
            
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

            userDtos.RoleNames = roles.Where(x => roleIds.Any(y => y == x.Id)).Select(x=>x.Name).ToArray();
            return userDtos;
        }        
 
        [AbpAuthorize(PermissionNames.Pages_Users_Read_All_Infos)]
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

            userDtos.RoleNames = roles.Where(x => roleIds.Any(y => y == x.Id)).Select(x=>x.Name).ToArray();
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

