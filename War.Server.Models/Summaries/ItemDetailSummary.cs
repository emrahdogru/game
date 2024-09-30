using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items;

namespace War.Server.Models.Summaries
{
    public class ItemDetailSummary(ItemObject item)
    {
        public string Key => item.Key;
        public string Name => item.Name;
        public bool IsCarryable => item.IsCarryable;
        public bool CanCarry => item.CanCarry;
        public int Mass => item.Mass;
        public int Strength => item.Strength;

        public ReceipeSummary Receipe => new(item.Receipe);

        public class ReceipeSummary(ItemObject.ReceipeDetail receipe)
        {
            public int Duration => Convert.ToInt32(receipe.Duration.TotalSeconds);
            public Dictionary<string, int> Items => receipe.Items.ToDictionary(x => x.Key.Key, x => x.Value);
            public IEnumerable<string> Techs => receipe.TechObjects.Select(x => x.Key);
        }
    }
}
