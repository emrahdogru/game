using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Exceptions
{
    public class NotEnoughCapacityException : Exception
    {
        public NotEnoughCapacityException(int freeCapacity, int requeiredCapacity)
        {
            FreeCapacity = freeCapacity;
            RequeiredCapacity = requeiredCapacity;
        }

        public int FreeCapacity { get;  }
        public int RequeiredCapacity { get; }

        public override string Message => $"Yeterli kapasite yok. Gereken: {RequeiredCapacity} Boş kapasite: {FreeCapacity}";

        public static void ThrowIfNotEnough(int freeCapacity, int requeiredCapacity)
        {
            if (freeCapacity < requeiredCapacity)
                throw new NotEnoughCapacityException(freeCapacity, requeiredCapacity);
        }
    }
}
