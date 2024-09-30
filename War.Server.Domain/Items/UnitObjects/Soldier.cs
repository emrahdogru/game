using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.UnitObjects
{
    /// <summary>
    /// Asker
    /// </summary>
    public class Soldier : UnitObject
    {
        public override string Name { get; } = "Soldier";

        public override double Speed { get; } = 1;

        public override int Range { get; } = 1;

        public override double FuelConsumption { get; } = 0;

        public override double FuelCapacity { get; } = 0;

        public override int Mass { get; } = 80;
        public override int CargoCapacity { get; } = 20;

        public override int Strength { get; } = 300;

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
