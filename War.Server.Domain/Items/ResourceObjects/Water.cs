using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Items.ResourceObjects
{
    public class Water : ResourceObject
    {
        public override string Name { get; } = "Water";

        public override int Mass { get; } = 1;

        public override ReceipeDetail Receipe { get; } = new ReceipeDetail(60, [], []);
    }
}
