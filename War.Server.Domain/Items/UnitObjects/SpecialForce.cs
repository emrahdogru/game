using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.UnitObjects
{
    public class SpecialForce : UnitObject
    {
        public override string Name { get; } = "Special Force";

        public override double Speed { get; } = 2;

        public override int Range { get; } = 2;

        public override double FuelConsumption { get; } = 0;

        public override double FuelCapacity { get; } = 0;

        public override int Mass { get; } = 100;
        public override int CargoCapacity { get; } = 20;

        public override int Strength { get; } = 400;

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
