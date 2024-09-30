using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        string message;

        public NotFoundException(string message)
        {
            this.message = message;
        }

        public override string Message => this.message;
    }
}
