using System.Globalization;

namespace WebTech.Auth.ErrorHandler.CustomExceptions.Common;

public abstract class CustomException : Exception
{
    public string[] Messages { get; }
    public abstract int StatusCode { get; }

    protected CustomException() : base() { }

    protected CustomException(string message) : base(message)
    {
        Messages = new[] { message };
    }

    protected CustomException(string[] messages)
    {
        Messages = messages;
    }

    protected CustomException(string message, params object[] args) : base(string.Format(message, args))
    {
        Messages = new[] { string.Format(CultureInfo.CurrentCulture, message, args) };
    }
}