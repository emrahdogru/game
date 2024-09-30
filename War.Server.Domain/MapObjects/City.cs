using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Items;
using War.Server.Domain.Items.BuildingObjects;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.MapObjects
{
    public partial class City : MapObject
    {
        private bool isCompletedProductionsMovedToCity = false;
        private string name = "City " + Guid.NewGuid().ToString().Split('-')[0];

        private IEnumerable<BuildingContainer> _buildings = [];


        public City(GameBoard gameBoard)
            :base(gameBoard)
        { }

        public City(GameBoard gameBoard, User user, Point location)
            :base(gameBoard)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(location, nameof(location));

            User = user;
            Location = location;
        }

        [BsonElement]
        public string Name
        {
            get
            {
                name ??= "City " + Guid.NewGuid().ToString().Split('-')[0];
                return name;
            }
            set
            {
                name = value;
            }
        }

        // TODO Şehirler için şimdilik bir kapasite limiti yok. Kaynaklar özelinde onları depolamaya yönelik özel binalar yapılabilir.
        public override int CargoCapacity => int.MaxValue;


        /// <summary>
        /// Moves the completed products to the city resources
        /// </summary>
        internal void MoveCompletedProductionsToCity()
        {
            if (!isCompletedProductionsMovedToCity)
            {
                isCompletedProductionsMovedToCity = true;
                foreach(var b in Buildings)
                {
                    b.MoveProductsToCityResources();
                }
            }
        }

        [BsonIgnore]
        public override ItemCollection<ItemObject> Resources
        { 
            get
            {
                MoveCompletedProductionsToCity();
                return base.Resources;
            }
            protected set
            {
                base.Resources = value;
            }
        }

        /// <summary>
        /// Buildings in the city.
        /// </summary>
        [BsonElement]
        public IEnumerable<BuildingContainer> Buildings
        {
            get
            {
                MoveCompletedProductionsToCity();
                return _buildings;
            }
            set
            {
                _buildings = value;
                foreach (var b in _buildings)
                {
                    b.City = this;
                }
            }
        }

        public IEnumerable<BuildingObject> GetConstructibleBuildings()
        {
            return ItemObjectSet.GetAll<BuildingObject>().Where(x =>
                x.MaxAmount > Buildings.Count(b => b.Building == x)    // Max amount limit not exceeded
                && (x.RequiredBuildings.Count == 0 || x.RequiredBuildings.All(rb => this.Buildings.Any(cb => cb.Building == rb)))   // All required buildings constructed
            );
        }

        /// <summary>
        /// Checks that is there enough resources in this city to produces <paramref name="amount"/> of pieces <paramref name="item"/> in this city.
        /// </summary>
        /// <param name="item">Item to be produces</param>
        /// <param name="amount">Production amount</param>
        /// <exception cref="NotEnoughResourceException"></exception>
        public void CheckReceipeItemsEnough(ItemObject item, int amount)
        {
            MoveCompletedProductionsToCity();
            foreach (var receipeItem in item.Receipe.Items)
            {
                Resources.CheckEnough(receipeItem.Key, receipeItem.Value * amount);
            }
        }

        /// <summary>
        /// Starts the building construction
        /// </summary>
        /// <param name="building">Building to be constructed</param>
        public void Build(BuildingObject building)
        {
            ArgumentNullException.ThrowIfNull(building, nameof(building));
            LimitExceededException.ThrowIf(() => Buildings.Count(x => x.Building == building) >= building.MaxAmount, building, building.MaxAmount);

            MoveCompletedProductionsToCity();

            int index = Buildings.Where(x => x.Building == building).Select(x => x.Index).DefaultIfEmpty(0).Max() + 1;

            var container = BuildingContainer.Create(building);
            container.Index = index;
            Buildings = Buildings.Append(container);
            Resources -= building.Receipe.Items;
        }

        /// <summary>
        /// Cancels the ongoing construction. Adds the remaining resources to city.
        /// </summary>
        /// <param name="buildingContainer">Building container</param>
        /// <returns>Returns the resources returned to the city</returns>
        /// <exception cref="NotFoundException"></exception>
        public ItemCollection<ItemObject>? CancelBuild(BuildingContainer buildingContainer)
        {
            ArgumentNullException.ThrowIfNull(buildingContainer, nameof(buildingContainer));

            if (buildingContainer.IsConstructionCompleted())
                throw new UserException("Construction already completed.");

            ItemCollection<ItemObject>? returnedResources = null;

            if (Buildings.Contains(buildingContainer))
            {
                MoveCompletedProductionsToCity();

                // Add remaining resources to city resources
                var totalDuration = (buildingContainer.ConstructionEndDate - buildingContainer.ConstructionStartDate).TotalSeconds;
                var remainingDuration = (buildingContainer.ConstructionEndDate - DateTime.UtcNow).TotalSeconds;
                var remainingRatio = (remainingDuration / totalDuration);
                returnedResources = (buildingContainer.Building.Receipe.Items * remainingRatio);
                Resources += returnedResources;
                Buildings = Buildings.Where(x => x != buildingContainer);
            }

            return returnedResources;
        }

        public int GetTotalPopulation()
        {
            int result = 0;
            result += Resources.Where(x => x.Key.Type == nameof(PersonObject)).Sum(x => x.Value);
            result += Buildings.SelectMany(x => x.Workers).Sum(x => x.Value);
            return result;
        }

        public override bool IsMoving()
        {
            return false;
        }

    }
}
