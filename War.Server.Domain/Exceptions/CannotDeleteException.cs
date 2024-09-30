using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Exceptions
{
    public class CannotDeleteException : Exception
    {
        public CannotDeleteException() { }

        public CannotDeleteException(string message) : base(message) { }
    }
}
