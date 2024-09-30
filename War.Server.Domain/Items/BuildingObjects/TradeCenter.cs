using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.BuildingObjects
{
    public class TradeCenter : BuildingObject
    {
        public override string Name { get; } = "Trade Center";

        public override int Mass { get; } = 0;

        public override int Strength { get; } = 1000;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(60 * 10, new() {
                    { ItemObjectSet.Iron, 50 },
                    { ItemObjectSet.Stone, 100 }
                });
                return _receipe;
            }
        }

        public override BuildingProductionType ProductionType { get; } = BuildingProductionType.None;
    }
}
