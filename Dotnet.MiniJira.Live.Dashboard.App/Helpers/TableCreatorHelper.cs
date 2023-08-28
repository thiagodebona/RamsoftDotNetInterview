using Dotnet.MiniJira.Domain.Entities;
using System.Text;

namespace Dotnet.MiniJira.Live.Dashboard.App.Helpers
{

    public class TableCreatorHelper
    {
        public string dateFormat = "yyyy/MM/dd HH:mm";
        public string CreateTable(List<Board> boardList)
        {
            var consoleWr = new ConsoleWriter();
            var toReturn = new StringBuilder();

            foreach (var board in boardList)
            {
                toReturn.AppendLine("");
                var table1 = new Table();
                table1.SetHeaders("Board", "Columns", "Tasks", "Expir. tasks", "Date create", "Archived tasks", "Id");
                var totalTasks = board.Columns.Select(p => new { total = p.Tasks?.Count() }).Sum(p => p.total).ToString();
                var totalExpiredTasks = board.Columns.Select(p => new { total = p.Tasks?.Count(p => DateTime.Now >= p.DeadLine) }).Sum(p => p.total).ToString();
                table1.AddRow(board.Name, board.Columns.Count.ToString(),
                    totalTasks, totalExpiredTasks, board.DateCreate.ToString(dateFormat), board.ArchivedTasks?.Count.ToString() ?? "0", board.Id.ToString());
                toReturn.Append(table1.ToString());

                string[] columnItems = new string[] { };
                foreach (var item in board.Columns)
                {
                    var table = new Table();
                    table.SetHeaders("Column", "Date create", "Tasks", "Expired tasks", "Id");
                    table.AddRow(item.Name, item.DateCreate.ToString(dateFormat), item.Tasks.Count.ToString(), item.Tasks.Count(p => DateTime.Now >= p.DeadLine).ToString(), item.Id.ToString());
                    toReturn.Append($"{table.ToString()}");

                    foreach (var tsk in item.Tasks)
                    {
                        toReturn.AppendLine($@"
      Id:           {{FC=Yellow}}{tsk.Id}{{/FC}}
      Name:         {{FC=Yellow}}{tsk.Name}{{/FC}}
      Description:  {tsk.Description}
      Assignee:     {{FC=Yellow}}{tsk.Assignee.Username}({tsk.Assignee.Name}){{/FC}}
      User created: {{FC=Yellow}}{tsk.UserCreated.Username}({tsk.UserCreated.Name}){{/FC}} 
      Dead line:    {tsk.DeadLine?.ToString(dateFormat)}
      Task status:  " + (DateTime.Now >= tsk.DeadLine ? "{FC=Red}Expired{/FC}" : "{FC=Blue}Not expired{/FC}") + $@"
      Date create:  {tsk.DateCreate.ToString(dateFormat)}
      Date update:  {{FC=Orange}}{tsk.DateUpdate?.ToString(dateFormat) ?? "Not updated yet"}" + @"{/FC}
      Favorite:     " + (tsk.IsFavorite ? "{FC=Green}Yes{/FC}" : "{FC=Cyan}No{/FC}") + $@"
      " + (item.Tasks.Count > 1 && item.Tasks[item.Tasks.IndexOf(tsk)] != item.Tasks[item.Tasks.Count - 1] ? "_________________________________________" : ""));
                    }
                    toReturn.AppendLine("");
                }
            }

            return toReturn.ToString();
        }
    }
}
