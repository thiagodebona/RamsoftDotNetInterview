using MongoDB.Bson;

namespace Dotnet.MiniJira.Domain.Core
{
    public interface IEntity
    {
        string Id { get; }
    }
}