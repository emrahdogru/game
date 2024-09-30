using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Items.Attributes
{
    public interface IFoodAttribute
    {
        /// <summary>
        /// Nutritional value of food.
        /// </summary>
        int NutritionalValue { get; }
    }
}
