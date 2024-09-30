using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BuildingObjects
{
    public class Foundry : BuildingObject
    {
        public override string Name { get; } = "Foundry";

        public override int Mass { get; } = 0;

        public override int Strength { get; } = 5000;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(1200, new()
                {
                    { ItemObjectSet.Iron, 5000 }
                });

                return _receipe;
            }
        }

        public override BuildingProductionType ProductionType { get; } = BuildingProductionType.Continious;
    }
}
