namespace Dotnet.MiniJira.Domain.Entities;

using Dotnet.MiniJira.Domain.Core;
using Dotnet.MiniJira.Domain.Enums.User;
using System.Text.Json.Serialization;

public class User : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public UserProfile Profile { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }
}