using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.UI;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Microsoft.Extensions.Options;

namespace ProjectHr.Users;

public class SendMailModel
{
    public string To { get; set; }
    public string Body { get; set; }
    public string Subject { get; set; }
    public string LinkWithToken { get; set; }
}

public class SESService : ISESService
    {
        private readonly SESOptions _settings;
        private readonly IAmazonSimpleEmailServiceV2 _amazonSimpleEmailServiceV2;

        public SESService(IOptions<SESOptions> settings, IAmazonSimpleEmailServiceV2 amazonSimpleEmailServiceV2)
        {
            _amazonSimpleEmailServiceV2 = amazonSimpleEmailServiceV2;
            _settings = settings.Value;
        }

        public string GetEmailTemplate(EmailType emailType,
            Dictionary<string, string>? keys = null)
        {
            string emailTemplate;


            if (emailType == EmailType.UserInvite)
            {
                emailTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources",
                    "Email",
                    "email_invite_template.html"));
            }
            else if (emailType == EmailType.PasswordReset)
            {
                emailTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources",
                    "Email",
                    "email_reset_password_template.html"));
            }
            else
            {
                throw new Exception("Invalid type of email.");
            }
            
            if (keys != null)
            {
                var replaced = ReplacePlaceholders(keys, emailTemplate);
                return replaced;
            }

            return emailTemplate;
        }

        private string ReplacePlaceholders(Dictionary<string, string> keys, string body)
        {
            var val = body;
            foreach (var keyValuePair in keys)
            {
                val = val.Replace(keyValuePair.Key, keyValuePair.Value);
            }

            return val;
        }

        public async Task SendMail(SendMailModel input)
        {
            Content subject = new Content
            {
                Data = input.Subject
            };

            Content text = new Content
            {
                Data = input.LinkWithToken
            };

            Body body = new Body
            {
                Html = new Content
                {
                    Data = input.Body
                },
                Text = text // just in case, if any problem occurs with template there will also be link as text.
            };

            var emailContent = new EmailContent
            {
                Simple = new Message
                {
                    Subject = subject,
                    Body = body,
                    
                }
            };

            var destination = new Destination
            {
                 ToAddresses = new List<string> { input.To }
            };


            var sendEmailRequest = new SendEmailRequest
            {
                Content = emailContent,
                Destination = destination,
                FromEmailAddress = _settings.MailFrom
            };

            try
            {
                await _amazonSimpleEmailServiceV2.SendEmailAsync(sendEmailRequest);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }