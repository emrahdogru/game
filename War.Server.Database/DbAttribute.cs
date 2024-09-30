using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Database
{
    public class DbAttribute(string collectionName) : Attribute
    {
        public string CollectionName { get => collectionName; }
    }
}
