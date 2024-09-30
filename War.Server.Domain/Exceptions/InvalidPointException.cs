using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Exceptions
{
    public class InvalidPointException : Exception
    {
        public InvalidPointException(Point point)
            :base("Invalid coordinate.")
        {
        
        }

        public InvalidPointException(Point point, string message)
        :base(message)
        {
            this.Point = point;
        }

        public Point Point { get;  }
    }
}
