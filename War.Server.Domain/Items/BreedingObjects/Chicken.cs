using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BreedingObjects
{
    public class Chicken : BreedingObject
    {
        public override Dictionary<ItemObject, TimeSpan> Outputs
        {
            get
            {
                _outputs ??= new()
                {
                    { ItemObjectSet.Egg, new TimeSpan(0, 30, 0) },
                    { ItemObjectSet.Fertilizer, new TimeSpan(18, 0, 0) }
                };
                return _outputs;
            }
        }

        public override ItemCollection<ItemObject> Yields
        {
            get
            {
                _yields ??= new()
                {
                    { ItemObjectSet.Meat, 1 }
                };
                return _yields;
            }
        }

        public override string Name { get; } = "Chicken";

        public override int Mass { get; } = 2;

        public override bool CanCarry {  get; } = false;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new ReceipeDetail(new TimeSpan(6, 0, 0), []);
                return _receipe;
            }
        }
    }
}
