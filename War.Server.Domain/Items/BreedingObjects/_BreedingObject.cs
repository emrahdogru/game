using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;

namespace War.Server.Domain.Items.BreedingObjects
{
    public abstract class BreedingObject : ItemObject
    {
        protected Dictionary<ItemObject, TimeSpan>? _outputs;
        protected ItemCollection<ItemObject>? _yields;

        public override bool IsCarryable { get; } = true;

        public override int Strength { get; } = 10;

        public override string Type { get; } = nameof(BreedingObject);

        /// <summary>
        /// Outputs provided as long as the BreedingObject is processed.
        /// </summary>
        public abstract Dictionary<ItemObject, TimeSpan> Outputs { get; }

        /// <summary>
        /// All products obtained from slaughter (e.g., meat, leather, etc.)
        /// </summary>
        public abstract ItemCollection<ItemObject> Yields { get; }
        
    }
}
