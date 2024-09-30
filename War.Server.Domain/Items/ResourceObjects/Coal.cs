using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Items.ResourceObjects
{
    public class Coal : ResourceObject
    {
        public override string Name { get; } = "Coal";

        public override int Mass { get; } = 1;

        public override ReceipeDetail Receipe { get; } = new(10, []);
    }
}
