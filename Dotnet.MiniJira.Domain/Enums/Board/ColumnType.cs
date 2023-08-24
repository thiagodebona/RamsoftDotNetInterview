
namespace Dotnet.MiniJira.Domain.Enums.Board;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum ColumnType
{
    todo = 0,
    InProgress = 1,
    InTest = 2,
    done = 3,
    Done = 4,
    Custom = 5
}
