namespace ProjectHr.Users;

public interface IMailAppService
{
    void SendMail(SendMailModel input);

    string GetEmailTemplate(EmailType emailType);
}