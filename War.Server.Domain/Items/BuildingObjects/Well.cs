using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BuildingObjects
{
    public class Well : BuildingObject
    {
        public override string Name { get; } = "Well";

        public override int Mass { get; } = 0;

        public override int Strength { get; } = 1000;

        public override int MaxAmount { get; } = 20;

        public override int MaxWorker { get; } = 6;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe = new(30,
                new() {
                    { ItemObjectSet.Stone, 10 }
                });

                return _receipe;
            }
        }

        public override ImmutableHashSet<ItemObject> ProducibleItems
        {
            get
            {
                _producibleItems ??= [
                    ItemObjectSet.Water
                ];

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

        public override BuildingProductionType ProductionType { get; } = BuildingProductionType.Continious;
    }
}
