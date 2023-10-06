using WebTech.Auth.ErrorHandler.CustomExceptions.Common;

namespace WebTech.Auth.ErrorHandler.CustomExceptions;

public class CriticalServerException : CustomException
{
    public override int StatusCode { get; } = 500;

    public CriticalServerException() : base() { }

    public CriticalServerException(string message) : base(message) { }

    public CriticalServerException(string[] messages) : base(messages) { }

    public CriticalServerException(string message, params object[] args) : base(message, args) { }
}