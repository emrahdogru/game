using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.Items.UnitObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BuildingObjects
{
    public class Barracks : BuildingObject
    {
        public override BuildingProductionType ProductionType { get; } = BuildingProductionType.Sequental;

        public override string Name { get; } = "Barracks";

        public override int Strength { get; } = 1000;

        public override int MaxWorker { get; } = 20;

        public override ImmutableHashSet<ItemObject> ProducibleItems
        {
            get
            {
                _producibleItems ??= [
                    ItemObjectSet.Soldier,
                    ItemObjectSet.SpecialForce,
                    ItemObjectSet.Sniper,
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

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(1000, new()
                {
                    { ItemObjectSet.Stone, 500 },
                    { ItemObjectSet.Iron, 200 }
                }, []);

                return _receipe;
            }
        }
    }
}
