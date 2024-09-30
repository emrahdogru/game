using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.Items.MidObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.UnitObjects
{
    public class Tank : UnitObject
    {
        public override string Name { get; } = "Tank";

        public override double Speed { get; } = 3;

        public override int Range { get; } = 3;

        public override double FuelConsumption { get; } = 20;

        public override double FuelCapacity { get; } = 250;

        public override int Mass { get; } = 5000;

        public override int CargoCapacity { get; } = 100;

        public override int Strength { get; } = 1000;

        public override bool CanCarry { get; } = true;

        public override bool IsCarryable { get; } = false;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(1200, new()
                {
                    { ItemObjectSet.Iron, 1 },
                    { ItemObjectSet.Rifle, 1 }
                });

                return _receipe;
            }
        }
    }
}
