using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items;

namespace War.Server.Models.Summaries
{
    public class ItemSummary(ItemObject item)
    {
        public string Key => item.Key;
        public string Name => item.Name;
    }
}
