namespace Dotnet.MiniJira.Domain.Entities;

using Dotnet.MiniJira.Domain.Core;

public class Task : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string UserCreated { get; set; }
    public string IsFavorite { get; set; }
    public string Assignee { get; set; }
    /// <summary>
    /// List of base64 images
    /// </summary>
    public List<string> Attachments { get; set; }
    public DateTime DeadLine { get; set; }
}