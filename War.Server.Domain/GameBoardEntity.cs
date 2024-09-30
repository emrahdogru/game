using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
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
    /// Oyun tahtasına bağlı nesne
    /// </summary>
    public abstract class GameBoardEntity : Entity, IGameBoardEntity
    {
        private GameBoard? game = null;

        public GameBoardEntity(GameBoard game)
        {
            this.game = game;
            this.GameBoardId = game.Id;
        }

        [BsonElement]
        internal ObjectId GameBoardId { get; private set; }

        [BsonIgnore]
        public GameBoard GameBoard
        {
            get
            {
                game ??= GameBoardRepository.FindOrThrow(this.GameBoardId);
                return game ?? throw new EntityNotFountException(nameof(GameBoard), this.GameBoardId);
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(GameBoard));
                game = value;
                GameBoardId = value.Id;
            }
        }

        ObjectId IGameBoardEntity.GameBoardId => this.GameBoardId;

        IGameBoard? IGameBoardEntity.GameBoard => this.GameBoard;
    }
}
