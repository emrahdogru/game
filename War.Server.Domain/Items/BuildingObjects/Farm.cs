using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BuildingObjects
{
    public class Farm : BuildingObject
    {
        public override BuildingProductionType ProductionType { get; } = BuildingProductionType.Continious;

        public override string Name { get; } = "Farm";

        public override int Strength { get; } = 1000;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(5000, new()
                {
                    { ItemObjectSet.Iron, 500 },
                    { ItemObjectSet.Stone, 20 }
                });

                return _receipe;
            }
        }
    }
}
