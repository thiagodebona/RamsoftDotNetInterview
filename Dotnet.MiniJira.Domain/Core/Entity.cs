using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Dotnet.MiniJira.Domain.Core
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public string Id { get; protected set; }
    }
}