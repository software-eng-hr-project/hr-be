using System.Threading.Tasks;
using ProjectHr.Configuration.Dto;

namespace ProjectHr.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
