namespace Dotnet.MiniJira.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RefreshToken
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; }
}