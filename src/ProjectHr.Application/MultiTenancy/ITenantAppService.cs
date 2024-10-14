using Abp.Application.Services;
using ProjectHr.MultiTenancy.Dto;

namespace ProjectHr.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

