using System.Threading.Tasks;
using Abp.Application.Services;

namespace ProjectHr.Users;

public interface IMailService: IApplicationService
{
    Task ResetPassword(ResetPasswordMailInput input);
    
    Task InviteUserMail(string input);
    
        
}