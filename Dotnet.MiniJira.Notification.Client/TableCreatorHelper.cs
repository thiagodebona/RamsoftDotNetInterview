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
                toReturn.AppendLine($@"
Board name:  {board.Name}
Description: {board.Description}
                ");

                string[] columnItems = new string[] { };
                foreach (var item in board.Columns)
                {
                    var table = new Table();
                    table.SetHeaders($"{item.Name} -> Tasks: {item.Tasks?.Count()}");
                    toReturn.Append(table.ToString());

                    foreach (var tsk in item.Tasks)
                    {
                        toReturn.AppendLine($@"
    Id:          {tsk.Id}
    Name:        {tsk.Name}
    Description: {tsk.Description}
    Assignee:    {tsk.Assignee}
    DeadLine:    {tsk.DeadLine}
    Favorite:    {tsk.IsFavorite}
                        ");
                    }
                    toReturn.AppendLine("------------------------------------------------------------------------------------");
                }
            }

            return toReturn.ToString();
        }
    }
}
