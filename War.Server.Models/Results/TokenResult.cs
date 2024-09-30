using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain;
using War.Server.Models.Summaries;

namespace War.Server.Models.Results
{
    public class TokenResult
    {
        public TokenResult(Token token)
        {
            this.Key = token.Key;
            this.User = new UserProfileSummary(token.User);
            var players = token.User.GetPlayers().ToArray();
            this.Players = players.Select(x => new PlayerSummary(x)).ToArray();
            this.DefaultGameBoardId = Players?.FirstOrDefault()?.GameBoard?.Id;
        }

        public string Key { get; }
        public UserProfileSummary User { get; }
        public IEnumerable<PlayerSummary>? Players { get; }
        public ObjectId? DefaultGameBoardId { get; }
    }
}
