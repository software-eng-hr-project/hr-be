﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;

namespace ProjectHr.Users;

public interface ISESService : ITransientDependency
{
    Task SendMail(SendMailModel input);

    string GetEmailTemplate(EmailType emailType, Dictionary<string, string> keys = null);
}