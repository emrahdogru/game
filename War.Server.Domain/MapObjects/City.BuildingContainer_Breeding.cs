using MongoDB.Bson.Serialization.Attributes;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Items;
using War.Server.Domain.Items.Attributes;
using War.Server.Domain.Items.BreedingObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.MapObjects
{
    public partial class City
    {
        public class BuildingContainerBreeding : BuildingContainer
        {
            [BsonElement]
            private string? ProductKey
            {
                get => Product?.Key;
                set => Product = value == null ? null : ItemObjectSet.FindByKey<BreedingObject>(value);
            }

            [BsonIgnore]
            public BreedingObject? Product { get; set; }

            [BsonElement]
            private double RootAmount { get; set; }

            /// <summary>
            /// Calculates the current amount based on the root amount and the elapsed time
            /// </summary>
            /// <returns></returns>
            public double CalculateCurrentAmount()
            {
                if (ProductionStartDate.HasValue && Product != null)
                {
                    var elapsedTime = (int)(DateTime.UtcNow - ProductionStartDate.Value).TotalSeconds;

                    var productionDuration = elapsedTime / (int)CalculateProductionDuration(Product, 1).TotalSeconds;
                    double amount = RootAmount * (Math.Pow(1.1, productionDuration));
                    return amount;
                }
                else if(Product == null)
                {
                    return 0;
                }
                else 
                {
                    return RootAmount;
                }
            }

            public int Amount => Convert.ToInt32(Math.Floor(CalculateCurrentAmount()));

            [BsonElement]
            private Dictionary<string, int> _equipment
            {
                get => Equipment.ToDictionary(x => x.Key.Key, x => x.Value);
                set => Equipment = new ItemCollection<ItemObject>(value);
            }


            /// <summary>
            /// Equipment using in this building.
            /// </summary>
            [BsonIgnore]
            public ItemCollection<ItemObject> Equipment { get; private set; } = [];

            [BsonElement]
            protected DateTime? ProductionStartDate { get; set; }

            public virtual void SetEquipment(ItemCollection<ItemObject> equipments)
            {
                ArgumentNullException.ThrowIfNull(nameof(equipments));

                if (equipments.Any(x => x.Value < 0))
                    throw new ArgumentOutOfRangeException(nameof(equipments), "Equipment count could not be negative.");

                if (equipments.Sum(x => x.Value) > ((IBreedingBuilding)Building).MaxEquipmentCount)
                    throw new UserException($"Equipment slots full.");

                // TODO Check if the equipment can be used in this building.

                RootAmount = CalculateCurrentAmount();
                ProductionStartDate = DateTime.UtcNow;

                var allItems = City.Resources + Equipment;

                allItems.CheckEnough(equipments);

                Equipment = equipments;
                City.Resources = allItems - equipments;
            }

            public void SetProduct(BreedingObject? item, int amount)
            {
                if (Product != null)
                    City.AddResource(Product, Amount);

                if (item == null || amount == 0)
                {
                    Product = null;
                    RootAmount = 0;
                    ProductionStartDate = null;
                }
                else
                {
                    Product = item;
                    double resuming = 0;

                    if (item == Product)
                    {
                        var currentAmount = CalculateCurrentAmount();
                        resuming = currentAmount - Amount;
                    }
                    RootAmount = amount + resuming;
                    City.RemoveResource(item, amount);
                    ProductionStartDate = DateTime.UtcNow;
                }
            }

            protected override TimeSpan CalculateProductionDuration(ItemObject item, int amount)
            {
                var duration = base.CalculateProductionDuration(item, amount);
                return duration;
            }


            public override void MoveProductsToCityResources()
            {
                //
            }
        }
    }
}
