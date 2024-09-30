using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BuildingObjects
{
    public class Dairy : BuildingObject
    {
        public override BuildingProductionType ProductionType { get; } = BuildingProductionType.Sequental;

        public override string Name { get; } = "Dairy";

        public override int Strength { get; } = 1000;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(1500, new()
                    {
                        { ItemObjectSet.Stone, 1000 },
                        { ItemObjectSet.Iron, 500 }
                    }
                );

                return _receipe;
            }
        }

        public override ImmutableHashSet<ItemObject> ProducibleItems
        {
            get
            {
                _producibleItems ??= [ItemObjectSet.Cheese, ItemObjectSet.Butter];
                return _producibleItems;
            }
        }
    }
}
