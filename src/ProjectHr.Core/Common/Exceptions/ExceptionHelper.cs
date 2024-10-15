using Abp.UI;
using ProjectHr.Common.Errors;

namespace ProjectHr.Common.Exceptions;

public class ExceptionHelper
{
    public static UserFriendlyException Create(ErrorCode errorCode, string message)
    {
        return new UserFriendlyException((int)errorCode, message);
    }

    public static UserFriendlyException Create(ErrorCode errorCode, params object?[] values)
    {
        var errText = CustomError.GetError(errorCode);

        return new UserFriendlyException((int)errorCode, string.Format(errText, values));
    }
}