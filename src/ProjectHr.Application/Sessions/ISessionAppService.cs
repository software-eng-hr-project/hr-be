using System.Threading.Tasks;
using Abp.Application.Services;
using ProjectHr.Sessions.Dto;

namespace ProjectHr.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
