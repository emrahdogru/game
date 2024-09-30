using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Items;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.MapObjects
{
    public partial class City
    {
        /// <summary>
        /// Container for building that producing continuously.
        /// </summary>
        public class BuildingContainerContinious : BuildingContainer
        {
            private ItemObject? product;

            ItemObject? lastProduct;
            double lastProductCompletionRatio = 0;

            [BsonElement]
            private string? ProductKey { get; set; }

            /// <summary>
            /// Item to be produced.
            /// </summary>
            public ItemObject? Product
            {
                get
                {
                    if (ProductKey == null)
                        return null;

                    if(product == null || product.Key != ProductKey)
                        product = ItemObjectSet.FindByKey(ProductKey);

                    return product;
                }
            }

            /// <summary>
            /// Starts the production.
            /// </summary>
            /// <param name="item">Item to be produced.</param>
            /// <exception cref="UserException"></exception>
            public void StartProduction(ItemObject item)
            {
                ArgumentNullException.ThrowIfNull(item, nameof(item));

                if (!IsConstructionCompleted())
                    throw new UserException($"{Building.Name} construction is still ongoing.");

                if (!Building.ProducibleItems.Contains(item))
                    throw new UserException($"You cannot produce {item.Name} in {Building.Name}");

                if (Building.WorkablePeople.IsEmpty && Workers.Sum(x => x.Value) <= 0)
                        throw new UserException("Production requires at least one worker.");

                TimeSpan durationPerItem = CalculateProductionDuration(item, 1);
                TimeSpan previoutProductCompletedDuration;

                if (lastProduct != null && lastProductCompletionRatio > 0)
                    previoutProductCompletedDuration = durationPerItem * lastProductCompletionRatio;
                else
                    previoutProductCompletedDuration = new TimeSpan(0);

                product = item;
                ProductKey = item.Key;
                ProductionStartDate = DateTime.UtcNow.Add(-previoutProductCompletedDuration);
                DurationPerItem = CalculateProductionDuration(item, 1);
            }

            public void StopProduction()
            {
                if (Product == null)
                    throw new UserException("Production already stopped.");

                MoveProductsToCityResources();

                Math.DivRem(Convert.ToInt32((DateTime.UtcNow - ProductionStartDate).TotalSeconds), Convert.ToInt32(DurationPerItem.TotalSeconds), out int elapsedTimeForLastProduct);
                lastProductCompletionRatio = (double)elapsedTimeForLastProduct / DurationPerItem.TotalSeconds;
                lastProduct = Product;

                if (lastProductCompletionRatio < 0 || lastProductCompletionRatio >= 1)
                    throw new Exception("Bu oranın 0-1 arası olması lazım. Bir şeyler yanlış!");

                ProductKey = null;
                product = null;
            }

            public override void SetWorkers(ItemCollection<PersonObject> workers)
            {
                var product = this.Product;
                if(product != null) StopProduction();
                base.SetWorkers(workers);
                if(product != null) StartProduction(product);
            }

            public override void MoveProductsToCityResources()
            {
                if (Product != null)
                {
                    var amount = Math.Max(0, NetAmount);
                    City.AddResource(Product, amount);
                    MovedAmount += amount;
                }
            }

            /// <summary>
            /// The start time of the production
            /// </summary>
            [BsonElement]
            public DateTime ProductionStartDate { get; private set; }

            /// <summary>
            /// Production duration per amount
            /// </summary>
            [BsonElement]
            public TimeSpan DurationPerItem { get; private set; }

            /// <summary>
            /// The total amount produced since the production started
            /// </summary>
            internal int ProducedAmount => Convert.ToInt32(Math.Floor((DateTime.UtcNow - ProductionStartDate).TotalSeconds / DurationPerItem.TotalSeconds));

            /// <summary>
            /// The amount of production that moved to city resources
            /// </summary>
            [BsonElement]
            internal int MovedAmount { get; set; }

            /// <summary>
            /// Completed amount that can be moved to city resources
            /// </summary>
            internal int NetAmount => ProducedAmount - MovedAmount;
            
        }
    }
}
