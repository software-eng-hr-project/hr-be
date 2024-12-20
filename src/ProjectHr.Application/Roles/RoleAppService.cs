﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using ProjectHr.Authorization;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;
using ProjectHr.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjectHr.Roles
{
    [AbpAuthorize]
    [Route("/api/roles")]
    public class RoleAppService :
        AsyncCrudAppService<Role, RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;

        public RoleAppService(IRepository<Role> roleRepository, RoleManager roleManager, UserManager userManager)
            : base(roleRepository)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [AbpAuthorize(PermissionNames.Create_Role)]
        [HttpPost]
        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }

        [AbpAuthorize(PermissionNames.List_Role)]
        [HttpGet]
        public async Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        [AbpAuthorize(PermissionNames.Update_Role)]
        [HttpPut]
        public override async Task<RoleDto> UpdateAsync(RoleDto input)
        {
            CheckUpdatePermission();

            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            ObjectMapper.Map(input, role);

            CheckErrors(await _roleManager.UpdateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }

        [AbpAuthorize(PermissionNames.Delete_Role)]
        [HttpDelete]
        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        [AbpAuthorize(PermissionNames.List_Role)]
        [HttpGet("permissions")]
        public async Task<ListResultDto<PermissionWithRoleDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();
            var roles = _roleRepository.GetAll().ToList();
            
            var permissionDto = new ListResultDto<PermissionWithRoleDto>(
                ObjectMapper.Map<List<PermissionWithRoleDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            );
            foreach (var permission in permissionDto.Items)
            {
                var roleNames = new List<string>();
                foreach (var role in roles)
                {
                    var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                    if (grantedPermissions.Any(g =>g.Name == permission.Name))
                    {
                        roleNames.Add(role.Name);
                    }
                }

                permission.RoleNames = roleNames.ToArray();
            }
            
            return permissionDto;
        }

        protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Permissions)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
                                                                   || x.DisplayName.Contains(input.Keyword)
                                                                   || x.Description.Contains(input.Keyword));
        }

        protected override async Task<Role> GetEntityByIdAsync(int id)
        {
            return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultRequestDto input)
        {
            return query.OrderBy(r => r.DisplayName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        [AbpAuthorize(PermissionNames.List_Role)]
        [HttpGet("{roleId}/permissions")]
        public async Task<GetRoleForEditOutput> GetRoleForEdit(int roleId)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var role = await _roleManager.GetRoleByIdAsync(roleId);
            var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
            var roleEditDto = ObjectMapper.Map<RoleEditDto>(role);

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName)
                    .ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }
        
        [AbpAuthorize(PermissionNames.List_Role)]
        [HttpGet("granted-permissions")]
        public override Task<PagedResultDto<RoleDto>> GetAllAsync(PagedRoleResultRequestDto input)
        {
            return base.GetAllAsync(input);
        }
        
        [RemoteService(false)]
        [HttpGet("asdasdasdasd{id}")]
        public override Task<RoleDto> GetAsync(EntityDto<int> input)
        {
            return base.GetAsync(input);
        }
    }
}
