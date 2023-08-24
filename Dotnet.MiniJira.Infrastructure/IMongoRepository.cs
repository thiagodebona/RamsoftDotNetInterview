namespace Dotnet.MiniJira.Infrastructure;

using Dotnet.MiniJira.Domain.Core;
using MongoDB.Driver;
using System.Linq.Expressions;

public interface IMongoBaseRepository<T> where T : IEntity
{
    Task AddAsync(T obj, CancellationToken cancellationToken);
    Task<List<T>> FindAsync(CancellationToken cancellationToken);
    Task<List<T>> FindBy(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    Task UpdateAsync(T obj, CancellationToken cancellationToken);
    Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
    Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken);
}