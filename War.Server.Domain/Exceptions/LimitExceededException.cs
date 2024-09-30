using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items;

namespace War.Server.Domain.Exceptions
{
    public class LimitExceededException : Exception
    {
        public LimitExceededException(ItemObject item, int maxAmount)
            :base($"Bir şehirde en fazla {maxAmount} {item.Name} inşa edebilirsiniz.")
        {

        }

        public static void ThrowIf(Func<bool> condition, ItemObject item, int maxAmount)
        {
            if(condition())
                throw new LimitExceededException(item, maxAmount);
        }
    }
}
