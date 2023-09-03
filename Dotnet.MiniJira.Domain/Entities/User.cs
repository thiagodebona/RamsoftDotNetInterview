namespace Dotnet.MiniJira.Domain.Entities;

using System.Text.Json.Serialization;
using Dotnet.MiniJira.Domain.Enums.User;
using MongoDB.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public UserProfile Profile { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }
}