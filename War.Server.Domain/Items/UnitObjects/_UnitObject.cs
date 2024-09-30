using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.Attributes;

namespace War.Server.Domain.Items.UnitObjects
{
    public abstract class UnitObject : ItemObject, IUnitAttribute
    {

        public override string Type { get; } = nameof(UnitObject);
        /// <summary>
        /// Speed of unit.
        /// </summary>
        public abstract double Speed { get; }

        /// <summary>
        /// Range of unit
        /// </summary>
        public abstract int Range { get; }

        /// <summary>
        /// Fuel consumption
        /// </summary>
        public abstract double FuelConsumption { get; }

        /// <summary>
        /// Fuel capacity.
        /// </summary>
        public abstract double FuelCapacity { get; }

        /// <summary>
        /// Cargo capacity. The total <see cref="ItemObject.Mass"/> of the items it carries cannot exceed the cargo capacity.
        /// Cargo stored in <see cref="MapObjects.MapObject.Resources"/> property of <see cref="MapObjects.Troop"/> type.
        /// </summary>
        public abstract int CargoCapacity { get; }
    }
}
