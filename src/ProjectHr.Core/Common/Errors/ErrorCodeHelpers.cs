using System;
using ProjectHr.Common.Exceptions;

namespace ProjectHr.Common.Errors;

public static class ErrorCodeHelpers
{
    public static void DuplicateMessageHelper(Exception e)
    {
        var message = e.InnerException.Message;
        
        if (message.Contains("duplicate"))
        {
            ErrorCode errorCode = ErrorCode.DuplicateError;
            
            if (message.Contains("WorkEmailAddress"))
                errorCode = ErrorCode.WorkEmailAdressUnique;
            
            else if (message.Contains("WorkPhone"))
                errorCode = ErrorCode.WorkPhoneUnique;
            
            else if (message.Contains("PersonalPhone"))
                errorCode = ErrorCode.PersonalPhoneUnique;
            
            else if (message.Contains("IdentityNumber"))
                errorCode = ErrorCode.IdentityNumberUnique;
            
            else if (message.Contains("EmergencyContactPhone"))
                errorCode = ErrorCode.EmergencyPhoneUnique;
            
                throw ExceptionHelper.Create(errorCode);
        }
    }
}