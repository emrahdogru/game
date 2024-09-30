using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Exceptions
{


    [Serializable]
    public class GameBoardMismatchException : Exception
    {
        public GameBoardMismatchException(string message)
            : base(message)
        { }

        public GameBoardMismatchException()
        {
        }

        public GameBoardMismatchException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

        public static void ThrowIfNotEqual(GameBoard source, GameBoard target)
        {
            if (source != target)
                throw new GameBoardMismatchException();
        }
    }
}
