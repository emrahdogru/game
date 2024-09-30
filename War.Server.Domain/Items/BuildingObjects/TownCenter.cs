using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BuildingObjects
{
    public class TownCenter : BuildingObject
    {
        public override string Name { get; } = "Town Center";

        public override int Mass { get; } = 0;
        public override int MaxWorker { get; } = 20;
        public override int Strength { get; } = 10000;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(999999, []);
                return _receipe;
            }
        }

        public override int MaxAmount { get; } = 1;

        public override ImmutableHashSet<ItemObject> ProducibleItems
        {
            get
            {
                _producibleItems ??= [ItemObjectSet.Person];
                return _producibleItems;
            }
        }

        public override ImmutableHashSet<PersonObject> WorkablePeople
        {
            get
            {
                _workablePeople ??= PersonObject.AllPersonTypes;
                return _workablePeople;
            }
        }

        public override BuildingProductionType ProductionType { get; } = BuildingProductionType.Sequental;
    }
}
