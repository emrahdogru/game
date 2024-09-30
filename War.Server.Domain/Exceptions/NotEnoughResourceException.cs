using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items;

namespace War.Server.Domain.Exceptions
{
    /// <summary>
    /// When there is not enough resource...
    /// </summary>
    public class NotEnoughResourceException(ItemObject item, int required, int exist) : Exception
    {
        public ItemObject Item { get; } = item;
        public int Required { get; } = required;
        public int Exist { get; } = exist;

        public override string Message => $"Not enough {Item.Name}. Required: {Required} Available: {Exist}";

        public static void ThrowIfNotEnough(ItemObject item, int required, int exist)
        {
            if(required > exist)
                throw new NotEnoughResourceException(item, required, exist);
        }
    }
}
