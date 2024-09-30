using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.UnitObjects
{
    public class Sniper : UnitObject
    {
        public override string Name { get; } = "Sniper";

        public override double Speed { get; } = 1;

        public override int Range { get; } = 4;

        public override double FuelConsumption { get; } = 0;

        public override double FuelCapacity { get; } = 0;
        public override int Mass { get; } = 80;

        public override int CargoCapacity { get; } = 0;

        public override int Strength { get; } = 150;

        public override bool CanCarry { get; } = false;

        public override bool IsCarryable { get; } = true;

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
