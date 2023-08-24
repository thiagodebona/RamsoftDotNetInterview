namespace Dotnet.MiniJira.Domain.Models.Users;

using Dotnet.MiniJira.Domain.Enums.User;
using System.ComponentModel.DataAnnotations;

public class CreateUserRequest
{
    [Required]
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserProfile? Profile { get; set; }

    [Required]
    public string Password { get; set; }
}