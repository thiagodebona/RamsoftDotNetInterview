using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Infrastructure;

namespace Dotnet.MiniJira.Application.Interface
{
    public interface IUserService : IUserRepository<User>
    {
    }
}
