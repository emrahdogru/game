using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.BuildingObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Test
{
    public class InspectTest
    {
        [Fact]
        public void Test_Items()
        {
            var items = ItemObjectSet.GetAll();

            foreach(var item in items)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Name), $"[{item.Key}] Item ismi boş olmamalı.");
                Assert.False(item.Receipe == null, $"[{item.Key}] Item.Repeipe must not be null.");
                Assert.False(item.Receipe?.Duration.TotalSeconds <= 0, $"[{item.Key}] Receipe.Duration must be larger than zero.");

                Assert.False(item.Receipe?.Items.Any(x => x.GetType().IsAssignableTo(typeof(BuildingObject))), $"[{item.Key}], reçetesinde bina içeriyor.");

                if(item is BuildingObject building)
                {
                    Assert.False(building.ProducibleItems.Any(x => x.GetType().IsAssignableTo(typeof(BuildingObject))), $"[{building.Key}], ProducibleItems listesinde bina içeriyor.");
                    Assert.False(building.ProducibleItems.Any() && !building.WorkablePeople.Any(), $"[{building.Key}] Üretilebilir ürün varsa, çalışabilir kişi tipleri de belirtilmeli.");
                    Assert.False(!building.ProducibleItems.Any() && building.WorkablePeople.Any(), $"[{building.Key}] Çalışabilir kişi varsa, üretilebilir ürün tipleri de belirtilmeli.");
                    Assert.False(building.WorkablePeople.Any() && building.MaxWorker <= 0, $"[{building.Key}] Çalışabilir kişi varsa, MaxWorker 0 olamaz.");
                }
            }
        }
    }
}
