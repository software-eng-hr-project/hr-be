using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProjectHr.Authorization.Users;
using ProjectHr.Common.Errors;
using ExceptionHelper = ProjectHr.Common.Exceptions.ExceptionHelper;

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
    [HttpPost("reset-password")]
    public async Task ResetPassword(ResetPasswordMailInput input)
    {
        var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == input.EmailAddress);

        if (user is null) throw ExceptionHelper.Create(ErrorCode.EmailCannotFound);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        user.PasswordResetToken = token;
        await _userRepository.UpdateAsync(user);

        var link = _emailSettings.ClientURL;
        var linkWithToken = string.Format($"{link}/reset-password?token={System.Web.HttpUtility.UrlEncode(token)}");

        var template = _sesService.GetEmailTemplate(EmailType.PasswordReset, new Dictionary<string, string>()
        {
            { "#link_with_token", linkWithToken },
        });


        var mail = new SendMailModel
        {
            To = input.EmailAddress,
            Body = template,
            Subject = "Password Reset Request",
            LinkWithToken = linkWithToken,
        };

        await _sesService.SendMail(mail);
    }

    [AbpAllowAnonymous]
    [HttpPost("invite-user")]
    public async Task ResetPasswordMail(ResetPasswordMailInput input)
    {
        var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == input.EmailAddress);

        if (user is null) throw ExceptionHelper.Create(ErrorCode.EmailCannotFound);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        user.PasswordResetToken = token;
        await _userRepository.UpdateAsync(user);
        
        var link = _emailSettings.ClientURL;
        var linkWithToken = string.Format($"{link}/join?token={token}");

        var template = _sesService.GetEmailTemplate(EmailType.UserInvite, new Dictionary<string, string>()
        {
            { "#link_with_token", linkWithToken },
        });


        var mail = new SendMailModel
        {
            To = input.EmailAddress,
            Body = template,
            Subject = "Welcome to QandQHR",
            LinkWithToken = linkWithToken,
        };

        await _sesService.SendMail(mail);
    }
}