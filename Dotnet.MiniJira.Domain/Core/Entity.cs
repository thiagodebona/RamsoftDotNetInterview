using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotnet.MiniJira.Domain.Core
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }
    }
}