using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Items.ResourceObjects
{
    public class Wood : ResourceObject
    {
        public override string Name { get; } = "Wood";

        public override int Mass { get; } = 3;
    }
}
