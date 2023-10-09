using System.Net;
using System.Text.Json;
using WebTech.Auth.ErrorHandler.CustomExceptions;

namespace WebTech.Auth.ErrorHandler;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        string result;
        switch (exception)
        {
            case CriticalServerException e:
                response.StatusCode = e.StatusCode;
                result = JsonSerializer.Serialize(e?.Messages);
                break;
            case ClientException e:
                response.StatusCode = e.StatusCode;
                result = JsonSerializer.Serialize(e?.Messages);
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(exception?.Message);
                break;
        }

        await response.WriteAsync(result);
    }
}

public static class ErrorHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandler(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}