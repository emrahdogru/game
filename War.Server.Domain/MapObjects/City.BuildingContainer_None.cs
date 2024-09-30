using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Items;
using War.Server.Domain.Items.PersonObjects;

namespace War.Server.Domain.MapObjects
{
    public partial class City
    {
        /// <summary>
        /// Üretim yapmayan binalar
        /// </summary>
        public class BuildingContainerNone : BuildingContainer
        {
            public override void SetWorkers(ItemCollection<PersonObject> workers)
            {
                throw new UserException("This is not a building where workers can work.");
            }

            public override void MoveProductsToCityResources()
            {
                // There is no product in this building
            }
        }
    }
}
