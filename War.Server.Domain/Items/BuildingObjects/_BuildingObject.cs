using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.Attributes;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.MapObjects;

namespace War.Server.Domain.Items.BuildingObjects
{
    public abstract class BuildingObject : ItemObject, IBuildingAttribute
    {
        protected ImmutableHashSet<ItemObject>? _producibleItems = null;
        protected ImmutableHashSet<PersonObject>? _workablePeople = null;

        public override string Type { get; } = nameof(BuildingObject);
        public override int Mass { get; } = 0;
        public override bool CanCarry { get; } = false;
        public override bool IsCarryable { get; } = false;

        /// <summary>
        /// Maximum amount of building than can be constructed in a city
        /// </summary>
        public virtual int MaxAmount { get; } = 10;

        /// <summary>
        /// Maximum <see cref="PersonObjects.PersonObject"/> count than can be work in this building
        /// </summary>
        public virtual int MaxWorker { get; } = 0;

        /// <summary>
        /// Production type. It is related to <see cref="City.BuildingContainer"/> sub types.
        /// </summary>
        public abstract BuildingProductionType ProductionType { get; }

        /// <summary>
        /// The items than can be produce in this building.
        /// </summary>
        public virtual ImmutableHashSet<ItemObject> ProducibleItems { get; } = [];

        /// <summary>
        /// Which <see cref="Person"/> types can work in this building.
        /// </summary>
        public virtual ImmutableHashSet<PersonObject> WorkablePeople { get; } = [];

        /// <summary>
        /// Buildings required in the city to construct this building.
        /// </summary>
        public virtual ImmutableHashSet<BuildingObject> RequiredBuildings { get; } = [];

        /// <summary>
        /// Building production type.
        /// </summary>
        public enum BuildingProductionType
        {
            /// <summary>
            /// No production.
            /// </summary>
            None,

            /// <summary>
            /// Continuous production like coal, water, wood etc.
            /// </summary>
            Continious,

            /// <summary>
            /// Sequental production like unit, tool etc.
            /// </summary>
            Sequental,

            /// <summary>
            /// Livestock breeding of cattle, sheep and by-products such as milk, wool etc.
            /// </summary>
            Breeding
        }
    }
}
