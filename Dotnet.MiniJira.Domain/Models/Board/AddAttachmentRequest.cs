namespace Dotnet.MiniJira.Domain.Models.Board;

public class AddAttachmentRequest
{
    public string BoardId { get; set; } = string.Empty;
    public string TaskId { get; set; } = string.Empty;
    public List<string> Attachments { get; set; } = new List<string>();
}
