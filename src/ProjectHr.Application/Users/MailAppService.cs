using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Abp.Application.Services;

namespace ProjectHr.Users;

public class SendMailModel
{
    public string To { get; set; }

    public string Body { get; set; }
    public string Subject { get; set; }
}

public class MailAppService: ProjectHrAppServiceBase, IMailAppService
{
    [RemoteService(false)]
    public string GetEmailTemplate(EmailType emailType)
    {
        string emailTemplate;

        if (emailType == EmailType.EmailVerification)
        {
            emailTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "email_verification_template.html"));
        }

        else if (emailType == EmailType.EmailChanging)
        {
            emailTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "email_changing_template.html"));
        }

        else if (emailType == EmailType.PasswordReset)
        {
            emailTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "password_reset_template.html"));
        }
        else
        {
            throw new Exception("Invalid type of email.");
        }


        return emailTemplate;
    }

    [RemoteService(false)]
    public void SendMail(SendMailModel input)
    {
        // SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
        // client.EnableSsl = true;
        // client.UseDefaultCredentials = false;
        // client.DeliveryMethod = SmtpDeliveryMethod.Network;
        // client.Credentials = new NetworkCredential(_settings.MailFrom, _settings.MailPassword);
        // MailAddress from = new MailAddress(_settings.MailFrom, _settings.MailDisplayName);
        // MailAddress to = new MailAddress(input.To);
        // MailMessage message = new MailMessage(from, to);
        //
        //
        // var body = ReplaceBodyData(input.Body);
        //
        // message.Body = body;
        // message.IsBodyHtml = true;
        // message.Subject = input.Subject;
        // message.SubjectEncoding = System.Text.Encoding.UTF8;
        // ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
        //
        // client.Send(message);
    }
}