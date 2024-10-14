using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Runtime.Session;
using ProjectHr.Configuration.Dto;

namespace ProjectHr.Configuration
{
    [RemoteService(false)]
    [AbpAuthorize]
    public class ConfigurationAppService : ProjectHrAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
