using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Utility
{
    public class ObjectIdTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public ObjectIdTypeProvider()
        :base(ParsingConfig.Default)
        {
        
        }

        public override HashSet<Type> GetCustomTypes() =>
            new[] { typeof(ObjectId) }.ToHashSet();
    }
}
