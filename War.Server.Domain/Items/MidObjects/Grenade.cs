using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.MidObjects
{
    public class Grenade : MidObject
    {
        public override string Name { get; } = "Grenade";

        public override int Mass { get; } = 1;

        public override int Strength { get; } = 5;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(150, new() {
                    { ItemObjectSet.Iron, 3 }
                });
                return _receipe;
            }
        }
    }
}
