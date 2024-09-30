using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Reflection;
using War.Server.Database;
namespace War.Server.Domain
{
    public abstract class Entity : IEntity
    {
        private ObjectId _id = ObjectId.Empty;

        [Newtonsoft.Json.JsonMergeKey]
        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.ObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId Id
        {
            get
            {
                if (_id == ObjectId.Empty)
                    _id = ObjectId.GenerateNewId();
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        [BsonExtraElements]
        protected BsonDocument? ExtraElements { get; set; }

        [BsonElement]
        [JsonIgnore]
        public DateTime CreateDate { get; internal set; } = DateTime.UtcNow;

        [BsonElement]
        protected int Version { get; set; }

        int IEntity.Version { get => Version; set => Version = value; }
    }
}
