namespace Dotnet.MiniJira.API.Middleware;

using Dotnet.MiniJira.Domain.Helpers;
using System.Net;
using System.Text.Json;

public class ErrorHandlerMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            _logger.LogCritical($"{error.GetType().Name} -> {error.Message}");

            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case UnauthorizedException:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case AppException:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { code = response.StatusCode, message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}