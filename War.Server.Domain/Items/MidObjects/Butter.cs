using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.MidObjects
{
    public class Butter : MidObject
    {
        public override string Name { get; } = "Butter";

        public override int Mass { get; } = 1;

        public override int Strength {  get; } = 1; 

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(160, new()
                {
                    { ItemObjectSet.Milk, 10 }
                });

                return _receipe;
            }
        }
    }
}
