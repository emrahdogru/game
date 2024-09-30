using Amazon.Auth.AccessControlPolicy;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using War.Server.Database;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Items;
using War.Server.Domain.Repositories;
using static MongoDB.Driver.WriteConcern;

namespace War.Server.Domain.MapObjects
{
    [BsonKnownTypes(
        typeof(City),
        typeof(Troop)
    )]
    [Db("MapObject")]
    public abstract class MapObject(GameBoard game) : GameBoardEntity(game)
    {
        private User? _user;

        [BsonElement]
        internal ObjectId? UserId { get; private set; }

        /// <summary>
        /// Sahibi olan oyuncu
        /// </summary>
        [BsonIgnore]
        public User? User
        {
            get
            {
                if (UserId.HasValue)
                {
                    _user ??= UserRepository.FindOrThrow(UserId.Value);
                    return _user;
                }
                else
                {
                    return null;
                }
            }
            internal set
            {
                UserId = value?.Id;
                _user = value;
            }
        }

        /// <summary>
        /// Harita üzerindeki konumu
        /// </summary>
        [BsonElement]
        public virtual Point Location { get; protected set; }

        /// <summary>
        /// Bu property sadece veritabanına kaydedilmek için kullanılıyor.
        /// </summary>
        [BsonElement("_resources")]
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE1006 // Naming Styles
        private Dictionary<string, int> _resources
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore IDE0051 // Remove unused private members
        {
            get
            {
                return Resources.Where(x => x.Value > 0).ToDictionary(x => x.Key.Key, x => x.Value);
            }
            set
            {
                Resources = new ItemCollection<ItemObject>(value);
            }
        }

        /// <summary>
        /// All resources in the map object.
        /// </summary>
        [BsonIgnore]
        public virtual ItemCollection<ItemObject> Resources { get; protected set; } = [];

        /// <summary>
        /// Adds resources to map object
        /// </summary>
        /// <param name="collection">Resources</param>
        /// <exception cref="NotEnoughCapacityException"></exception>
        public void AddResources(ItemCollection<ItemObject> collection)
        {
            var totalResources = Resources + collection;
            var totalMass = totalResources.Sum(x => x.Key.Mass * x.Value);

            NotEnoughCapacityException.ThrowIfNotEnough(FreeCapacity, collection.Sum(x => x.Key.Mass * x.Value));

            Resources += collection;
        }

        /// <summary>
        /// Adds resource to map object
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <exception cref="NotEnoughCapacityException"></exception>
        public void AddResource(ItemObject item, int amount)
        {
            AddResources(new ItemCollection<ItemObject>(item, amount));
        }

        /// <summary>
        /// Removes resources from map object
        /// </summary>
        /// <param name="collection">Resources</param>
        /// <exception cref="NotEnoughResourceException"></exception>
        public void RemoveResources(ItemCollection<ItemObject> collection)
        {
            Resources -= collection;
        }

        /// <summary>
        /// Removes the resource from map object
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <exception cref="NotEnoughResourceException"></exception>
        public void RemoveResource(ItemObject item, int amount)
        {
            RemoveResources(new ItemCollection<ItemObject>(item, amount));
        }

        /// <summary>
        /// Toplam yük kapasitesi
        /// </summary>
        public abstract int CargoCapacity { get; }

        /// <summary>
        /// Kullanılan yük kapasitesi
        /// </summary>
        public int UsedCapacity => Resources?.Sum(x => x.Key.Mass * x.Value) ?? 0;

        /// <summary>
        /// Kullanılabilir yük kapasitesi
        /// </summary>
        public int FreeCapacity => CargoCapacity - UsedCapacity;



        public abstract bool IsMoving();
    }
}
