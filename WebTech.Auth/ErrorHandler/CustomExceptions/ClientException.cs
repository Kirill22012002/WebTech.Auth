using WebTech.Auth.ErrorHandler.CustomExceptions.Common;

namespace WebTech.Auth.ErrorHandler.CustomExceptions;

public class ClientException : CustomException
{
    public override int StatusCode { get; } = 400;

    public ClientException() : base() { }

    public ClientException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public ClientException(string[] messages, int statusCode) : base(messages)
    {
        StatusCode = statusCode;
    }

    public ClientException(string message, params object[] args) : base(message, args) { }

    public ClientException(string message, int statusCode, params object[] args) : base(message, args)
    {
        StatusCode = statusCode;
    }
}