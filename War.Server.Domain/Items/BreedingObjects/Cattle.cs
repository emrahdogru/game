using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.Attributes;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BreedingObjects
{
    public class Cattle : BreedingObject, IUnitAttribute
    {
        public override string Name { get; } = "Cattle";

        public override int Mass { get; } = 1000;

        public override bool CanCarry { get; } = true;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new ReceipeDetail(new TimeSpan(12, 0, 0), []);

                return _receipe;
            }
        }

        public override Dictionary<ItemObject, TimeSpan> Outputs
        {
            get
            {
                _outputs ??= new()
                {
                    { ItemObjectSet.Milk, new TimeSpan(0, 10, 0) },
                    { ItemObjectSet.Fertilizer, new TimeSpan(0, 30, 0) }
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
                    { ItemObjectSet.Meat, 500 },
                    { ItemObjectSet.Leather, 2 }
                };

                return _yields;
            }
        }

        public int CargoCapacity { get; } = 100;

        public double FuelCapacity { get; } = 0;

        public double FuelConsumption { get; } = 0;

        public int Range { get; } = 0;

        public double Speed { get; } = 5;
    }
}
