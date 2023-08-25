namespace Dotnet.MiniJira.Domain.Entities;

using Dotnet.MiniJira.Domain.Core;

public class Task : Entity
{
    public Task()
    {
    }

    public Task(string name, string description, string idUserCreated, DateTime deadLine, bool isFavorite, string assignee = "")
    {
        Name = name;
        Description = description;
        UserCreated = idUserCreated;
        DeadLine = deadLine;
        IsFavorite = isFavorite;
        Assignee = assignee;

    }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UserCreated { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public string Assignee { get; set; } = string.Empty;
    /// <summary>
    /// List of base64 images
    /// </summary>
    public List<string> Attachments { get; set; } = new List<string>();
    public DateTime DeadLine { get; set; }
}