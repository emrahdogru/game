using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.UnitObjects
{
    public class Truck : UnitObject
    {
        public override string Name { get; } = "Truck";

        public override double Speed { get; } = 100;

        public override int Range { get; } = 0;

        public override double FuelConsumption { get; } = 0.5;

        public override double FuelCapacity { get; } = 150;

        public override int Mass { get; } = 2000;

        public override int CargoCapacity { get; } = 10000;

        public override int Strength { get; } = 100;

        public override bool CanCarry { get; } = true;

        public override bool IsCarryable { get; } = false;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(1200, new()
                {
                    { ItemObjectSet.Person, 1 },
                    { ItemObjectSet.Rifle, 1 }
                });

                return _receipe;
            }
        }
    }
}
