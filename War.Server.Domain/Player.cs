using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Database;
using War.Server.Domain.Repositories;

namespace War.Server.Domain
{
    /// <summary>
    /// Oyun tahtasında yer alan kullanıcı  (Tahtadaki oyuncu)
    /// </summary>
    [Db("Player")]
    public class Player : GameBoardEntity
    {
        private User? _user = null;

        public Player(GameBoard game) : base(game)
        {

        }

        [BsonElement]
        internal ObjectId UserId { get; set; }

        [BsonIgnore]
        public User User
        {
            get
            {
                _user ??= UserRepository.FindOrThrow(this.UserId);
                return _user;
            }
            set
            {
                _user = value ?? throw new NullReferenceException("User can not be null.");
                UserId = value.Id;
            }
        }

        public string Name { get; set; } = string.Empty;
    }
}
