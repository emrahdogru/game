using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.Attributes;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.Items.UnitObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.PersonObjects
{
    public abstract class PersonObject : UnitObject, IPersonAttribute
    {
        public override string Type { get; } = nameof(PersonObject);
        public override double Speed { get; } = 5;

        public override int Range { get; } = 0;

        public override double FuelConsumption { get; } = 0;

        public override double FuelCapacity { get; } = 0;

        public override int Strength { get; } = 100;

        /// <summary>
        /// Efficiency factor. Higher values provide more efficiency in the production process.
        /// It is affects to production duration. Value could be between 0.1 and 2.0
        /// <list type="">2.0 is two time more effective</list>
        /// <list type="">0.1 is 10% effective</list>
        /// </summary>
        public virtual double EfficiencyFactor { get; } = 1;

        /// <summary>
        /// A shortcut for person types for <seealso cref="BuildingObjects.BuildingObject.WorkablePeople"/>
        /// </summary>
        internal readonly static ImmutableHashSet<PersonObject> AllPersonTypes =
        [
            ItemObjectSet.Person,
            ItemObjectSet.Engineer,
            ItemObjectSet.Medic
        ];

        /// <summary>
        /// A shortcut for person types for <seealso cref="BuildingObjects.BuildingObject.WorkablePeople"/>
        /// </summary>
        internal readonly static ImmutableHashSet<PersonObject> PersonAndEngineer = [
            ItemObjectSet.Person,
            ItemObjectSet.Engineer,
            ];

        /// <summary>
        /// A shortcut for person types for <seealso cref="BuildingObjects.BuildingObject.WorkablePeople"/>
        /// </summary>
        internal readonly static ImmutableHashSet<PersonObject> Person =
        [
            ItemObjectSet.Person
        ];

        /// <summary>
        /// A shortcut for person types for <seealso cref="BuildingObjects.BuildingObject.WorkablePeople"/>
        /// </summary>
        internal readonly static ImmutableHashSet<PersonObject> Engineer =
        [
            ItemObjectSet.Engineer
        ];

        /// <summary>
        /// A shortcut for person types for <seealso cref="BuildingObjects.BuildingObject.WorkablePeople"/>
        /// </summary>
        internal readonly static ImmutableHashSet<PersonObject> Medic =
        [
            ItemObjectSet.Medic
        ];
    }
}
