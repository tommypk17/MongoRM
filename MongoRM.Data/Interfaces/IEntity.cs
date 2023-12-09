using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoRM.Data.Interfaces;

/**
 * <summary>
 * IEntity represents a MongoDB collection Schema.
 * Any object represented in MongoDB that will be queried should inherit from this object.
 * </summary>
 */
public interface IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}