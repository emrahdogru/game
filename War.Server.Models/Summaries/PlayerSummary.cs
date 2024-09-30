using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain;

namespace War.Server.Models.Summaries
{
    public class PlayerSummary
    {
        public PlayerSummary(Player player)
        {
            this.Id = player.Id;
            this.Name = player.Name;
            this.GameBoard = new GameBoardSummary(player.GameBoard);
        }

        public ObjectId Id { get; }
        public string Name { get; }
        public GameBoardSummary GameBoard { get; }
    }
}
