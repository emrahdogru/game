using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.Attributes;
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


    }
}
