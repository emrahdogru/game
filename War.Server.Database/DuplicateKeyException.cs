using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Database
{
    public  class DuplicateKeyException : Exception
    {
        public DuplicateKeyException(string message):base(message) { }

        public DuplicateKeyException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
