using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Repositories
{
    public class GameBoardRepository : Repository<GameBoard>
    {
        public static IQueryable<GameBoard> GetUserGameBoards(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            var gameIds = PlayerRepository.GetAll().Where(x => x.UserId == user.Id).Select(x => x.GameBoardId).ToArray();
            return GetAll().Where(x => gameIds.Contains(x.Id));
        }
    }
}
