using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.MidObjects
{
    public class Cheese : MidObject
    {
        public override string Name { get; } = "Cheese";

        public override int Mass { get; } = 1;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(160, new()
                {
                    { ItemObjectSet.Milk, 20 }
                });

                return _receipe;
            }
        }

        public override int Strength { get; } = 1;
    }
}
