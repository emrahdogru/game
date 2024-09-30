using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain;

namespace War.Server.Models.Summaries
{
    public class GameBoardSummary
    {
        public GameBoardSummary(GameBoard gameBoard)
        {
            ArgumentNullException.ThrowIfNull(gameBoard, nameof(gameBoard));

            this.Id = gameBoard.Id;
            this.Size = gameBoard.Size;
        }

        public ObjectId Id { get; }
        public Point Size { get; }
    }
}
