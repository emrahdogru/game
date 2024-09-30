using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Items.Attributes
{
    public interface IBreedingBuilding
    {
        int Capacity { get; }

        int MaxEquipmentCount { get; }
    }
}
