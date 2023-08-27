using Dotnet.MiniJira.Domain.Entities;
using System.Text;

namespace Dotnet.MiniJira.Notification.Client
{
    public class TableCreatorHelper
    {
        public string CreateTable(List<Board> boardList)
        {
            var toReturn = new StringBuilder();

            foreach (var board in boardList)
            {
                toReturn.AppendLine("");
                var table1 = new Table();
                table1.SetHeaders("Board", "Columns", "Tasks", "Date create");
                var totalTasks = board.Columns.Select(p => new { total = p.Tasks?.Count() }).Sum(p => p.total).ToString();
                table1.AddRow(board.Name, board.Columns.Count.ToString(), totalTasks, board.DateCreate.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                toReturn.Append(table1.ToString());

                string[] columnItems = new string[] { };
                foreach (var item in board.Columns)
                {
                    var table = new Table();
                    table.SetHeaders("Column", "Date create", "Id");
                    table.AddRow(item.Name, item.DateCreate.ToString("MM/dd/yyyy h:mm tt"), item.Id);
                    toReturn.Append(table.ToString());

                    foreach (var tsk in item.Tasks)
                    {
                        toReturn.AppendLine($@"
    Id:          {tsk.Id}
    Name:        {tsk.Name}
    Description: {tsk.Description}
    Assignee:    {tsk.Assignee.Username}({tsk.Assignee.Name}) 
    Dead line:   {tsk.DeadLine?.ToString($"dddd, dd MMMM yyyy HH:mm:ss")}
    Task status: " + (DateTime.Now >= tsk.DeadLine ? "Expired" : "Not expired") + $@"
    Date create: {tsk.DateCreate.ToString($"dddd, dd MMMM yyyy HH:mm:ss")}
    Date update: {tsk.DateCreate.ToString($"dddd, dd MMMM yyyy HH:mm:ss")}
    Favorite:    " + (tsk.IsFavorite ? "Yes" : "No"));
                    }
                    toReturn.AppendLine("");
                }
            }

            return toReturn.ToString();
        }
    }
}
