using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.UnitObjects
{
    public class Artillery : UnitObject
    {
        public override string Name { get; } = "Artillery";

        public override double Speed { get; } = 1;

        public override int Range { get; } =  10;

        public override double FuelConsumption { get; } = 0;

        public override double FuelCapacity { get; } = 0;

        public override int Mass { get; } = 2000;
        public override int CargoCapacity { get; } = 20;

        public override int Strength { get; } = 100;

        public override bool CanCarry { get; } = false;

        public override bool IsCarryable { get; } = true;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(1200, new() {
                    { ItemObjectSet.Iron, 200 },
                    { ItemObjectSet.Person, 2 },
                });
                return _receipe;
            }
        }
    }
}
