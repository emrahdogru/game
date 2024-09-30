using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.MidObjects
{
    public class Rifle : MidObject
    {
        public override string Name { get; } = "Rifle";

        public override int Mass { get; } = 4;

        public override int Strength { get; } = 100;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(30, new()
                {
                    { ItemObjectSet.Iron, 3 }
                });

                return _receipe;
            }
        }
    }
}
