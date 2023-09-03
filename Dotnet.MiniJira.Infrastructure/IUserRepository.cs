namespace Dotnet.MiniJira.Infrastructure;

using System.Linq.Expressions;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Users;
using MongoDB.Entities;

public interface IUserRepository<T> where T : Entity
{
    Task<AuthenticateResponse> CreateUser(CreateUserRequest model, string ipAddress);
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
    Task<User?> OneAsync(string userId);
    //Task AddAsync(T obj, CancellationToken cancellationToken);
    //Task<List<T>> FindAsync(CancellationToken cancellationToken);
    //Task<List<T>> FindBy(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    //Task UpdateAsync(T obj, CancellationToken cancellationToken);
    //Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);
    //Task DeleteAsync(string id, CancellationToken cancellationToken);
}