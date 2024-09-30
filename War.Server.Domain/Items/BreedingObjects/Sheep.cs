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
    public class Sheep : BreedingObject, IUnitAttribute
    {
        public override string Name { get; } = "Sheep";

        public override int Mass { get; } = 100;

        public override bool CanCarry { get; } = false;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(new TimeSpan(6, 0, 0), []);
                return _receipe;
            }
        }

        public override Dictionary<ItemObject, TimeSpan> Outputs
        {
            get
            {
                _outputs ??= _outputs = new()
                {
                    { ItemObjectSet.Wool, new TimeSpan(6, 0, 0) },
                    { ItemObjectSet.Milk, new TimeSpan(1, 0, 0) },
                    { ItemObjectSet.Fertilizer, new TimeSpan(12, 0, 0) }
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
                    { ItemObjectSet.Meat, 50 },
                    { ItemObjectSet.Leather, 1 }
                };

                return _yields;
            }
        }

        public int CargoCapacity { get; } = 0;

        public double FuelCapacity { get; } = 0;

        public double FuelConsumption { get; } = 0;

        public int Range { get; } = 0;

        public double Speed { get; } = 3;
    }
}
