namespace Dotnet.MiniJira.Domain.Models.Users;

using Dotnet.MiniJira.Domain.Entities;
using System.Text.Json.Serialization;


public class AuthenticateResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }
    public string Email { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }

    public AuthenticateResponse(User user, string jwtToken, string refreshToken)
    {
        Id = user.Id;
        Name = user.Name;
        Username = user.Username;
        JwtToken = jwtToken;
        Email = user.Email;
        RefreshToken = refreshToken;
    }
}