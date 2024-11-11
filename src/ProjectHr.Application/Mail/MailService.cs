using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Users;

[Route("/api/mail")]
public class MailService : ProjectHrAppServiceBase
{
    private readonly IRepository<User, long> _userRepository;
    private readonly UserManager _userManager;
    private readonly EmailSettings _emailSettings;
    private readonly ISESService _sesService;
    
    public MailService(
        IRepository<User, long> userRepository,
        UserManager userManager,
        IOptions<EmailSettings> emailSettings,
        ISESService sesService
        )
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _emailSettings = emailSettings.Value;
        _sesService = sesService;
    }
    
    [AbpAllowAnonymous]
    [HttpPost("invite-user")]
    public async Task ResetPasswordMail(ResetPasswordMailInput input)
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
    
            // var link = _httpContextAccessor.HttpContext.Request.Host.Value;
            var link = _emailSettings.ClientURL;
            var linkWithToken = string.Format($"{link}/users/invite/?token={token}");
    
            var template = _sesService.GetEmailTemplate(EmailType.UserInvite, new Dictionary<string, string>()
            {
                { "#link_with_token", linkWithToken },
            });
    
    
            var mail = new SendMailModel
            {
                To = input.EmailAddress,
                Body = template,
                Subject = "Invite User",
                LinkWithToken = linkWithToken,
                    
            };
    
            await _sesService.SendMail(mail);
        
    }
}