using System.Numerics;
using System.Runtime.CompilerServices;
using War.Server.Database;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Repositories;

namespace War.Server.Domain
{
    /// <summary>
    /// Oyun tahtası
    /// </summary>
    [Db("GameBoard")]
    public class GameBoard : Entity, IGameBoard
    {
        public string Name { get; set; } = "Game " + Guid.NewGuid().ToString().Split('-')[0];

        /// <summary>
        /// Oyun fazı.
        /// </summary>
        public double Phase { get; set; } = 1;


        public IQueryable<Player> GetPlayers()
        {
            return Repository<Player>.GetAll().Where(x => x.GameBoardId == this.Id);
        }

        public Point Size { get; set; }

        /// <summary>
        /// Nokta, harita içinde geçerli bir nokta mı?
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsValidPoint(Point point)
        {
            if (point.X < 0 || point.Y < 0)
                return false;

            if (point.X > Size.X || point.Y > Size.Y)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is GameBoard target)
                return this == target;

            return false;
        }

        public static bool operator ==(GameBoard target, GameBoard other)
        {
            return target.Id == other.Id;
        }

        public static bool operator !=(GameBoard target, GameBoard other)
        {
            return !(target == other);
        }
    }
}
