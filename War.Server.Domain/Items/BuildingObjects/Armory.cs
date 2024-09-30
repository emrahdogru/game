using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.Items.MidObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BuildingObjects
{
    public class Armory : BuildingObject
    {
        public override BuildingProductionType ProductionType { get; } = BuildingProductionType.Sequental;

        public override string Name { get; } = "Armory";

        public override int Strength { get; } = 1000;

        public override int MaxWorker { get; } = 15;


        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(300, new()
                {
                    { ItemObjectSet.Stone, 300 },
                    { ItemObjectSet.Iron, 500 }
                });

                return _receipe;
            }
        }

        public override ImmutableHashSet<ItemObject> ProducibleItems
        {
            get
            {
                _producibleItems ??= [ItemObjectSet.Rifle];
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
    }
}
