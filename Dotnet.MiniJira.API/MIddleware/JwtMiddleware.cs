namespace MiniJira.API.MIddleware;

using Dotnet.MiniJira.Application.Interface;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtRepository jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token ?? string.Empty);
        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = await userService.OneAsync(userId);
        }

        await _next(context);
    }
}