using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Items.MidObjects
{
    public class GunPowder : MidObject
    {
        public override string Name { get; } = "Gun powder";

        public override int Mass { get; } = 1;

        public override int Strength { get; } = 0;

        public override ReceipeDetail Receipe { get; } = new (5, []);
    }
}
