using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.Attributes;

namespace War.Server.Domain.Items.ResourceObjects
{
    public class Grain : ResourceObject, IFoodAttribute
    {
        public override string Name { get; } = "Grain";

        public override int Mass { get; } = 1;

        public int NutritionalValue { get; } = 4;
    }
}
