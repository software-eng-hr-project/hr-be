using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ProjectHr.Roles.Dto;
using ProjectHr.Users.Dto;

namespace ProjectHr.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task DeActivate(EntityDto<long> user, int layoffId);
        Task Activate(EntityDto<long> user);
        Task<ListResultDto<RoleDto>> GetRoles();
        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);
    }
}
