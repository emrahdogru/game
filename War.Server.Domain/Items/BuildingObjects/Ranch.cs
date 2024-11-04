using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.Attributes;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BuildingObjects
{
    public class Ranch : BuildingObject, IBreedingBuilding
    {
        public override BuildingProductionType ProductionType { get; } = BuildingProductionType.Breeding;

        public override string Name { get; } = "Ranch";

        public override int Strength { get; } = 10000;

        public int Capacity { get; } = 50000;

        public int MaxEquipmentCount { get; } = 3;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(1500, new() {
                    { ItemObjectSet.Wood, 150 },
                    { ItemObjectSet.Iron, 50 }
                });

                return _receipe;
            }
        }

        public override ImmutableHashSet<ItemObject> ProducibleItems
        {
            get
            {
                _producibleItems ??= [
                    ItemObjectSet.Cattle,
                    ItemObjectSet.Sheep,
                    ItemObjectSet.Chicken
                    ];

                return _producibleItems;
            }
        }

        public override int MaxWorker { get; } = 15;

        public override ImmutableHashSet<PersonObject> WorkablePeople
        {
            get
            {
                _workablePeople ??= [
                    ItemObjectSet.Person,
                    ItemObjectSet.Medic
                    ];

                return _workablePeople;
            }
        }


    }
}
