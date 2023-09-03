namespace Dotnet.MiniJira.Application.Interface;

using Dotnet.MiniJira.Domain.Entities;


public interface IJwtRepository
{
    public string GenerateJwtToken(User user);
    public string? ValidateJwtToken(string token);
    public RefreshToken GenerateRefreshToken(string ipAddress);
}
