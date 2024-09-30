using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Items.UnitObjects;
using War.Server.Domain.ObjectSets;
using War.Server.Domain.Items.BuildingObjects;
using War.Server.Domain.Items.PersonObjects;
using System.ComponentModel;

namespace War.Server.Domain.MapObjects
{
    public partial class City
    {
        /// <summary>
        /// Buinding that make sequental production.
        /// </summary>
        public class BuildingContainerSequental : BuildingContainer
        {
            /// <summary>
            /// Production queue. When a new production instruction arrives, it is added to queue and produced sequentally.
            /// </summary>
            [BsonElement]
            public IEnumerable<ProductionInstruction> ProductionQueue { get; internal set; } = [];

            public override void SetWorkers(ItemCollection<PersonObject> workers)
            {
                if (this.ProductionQueue.Any(x => x.EndDate > DateTime.UtcNow))
                    throw new UserException("You can not change the workers while production is ongoing.");

                base.SetWorkers(workers);
            }

            /// <summary>
            /// Adds the production instruction to the queue for the <paramref name="amount"/> pieces <paramref name="item"/>.
            /// </summary>
            /// <param name="item">Item to be produced</param>
            /// <param name="amount">Production amount</param>
            public void AddToProductionQueue(ItemObject item, int amount)
            {
                ArgumentNullException.ThrowIfNull(item, nameof(item));
                ArgumentOutOfRangeException.ThrowIfNegative(amount);
                if (item is BuildingObject) throw new UserException("You cannot produce building in a building.");

                if (!IsConstructionCompleted())
                    throw new UserException($"{Building.Name} construction is still ongoing.");

                City.CheckReceipeItemsEnough(item, amount);

                if (!Building.ProducibleItems.Contains(item))
                    throw new UserException($"You cannot produce {item.Name} in {Building.Name}.");

                if (Workers.Sum(x => x.Value) <= 0)
                    throw new UserException("Production requires at least one worker.");

                var production = new ProductionInstruction(item, amount);
                var duration = CalculateProductionDuration(item, amount);
                production.StartDate = ProductionQueue.Any() ? ProductionQueue.Max(x => x.EndDate) : DateTime.UtcNow;
                production.EndDate = production.StartDate.Add(duration);

                ProductionQueue = ProductionQueue.Append(production);
                City.Resources -= (item.Receipe.Items * amount);
            }

            /// <summary>
            /// Cancels the production instruction. Adds the remaining resources and
            /// completed production to the city resources for production that was interrupted.
            /// </summary>
            /// <param name="production">Production instruction to be cancelled</param>
            public void CancelProductionInQueue(ProductionInstruction production)
            {
                ArgumentNullException.ThrowIfNull(production, nameof(production));

                if (ProductionQueue.Contains(production))
                {
                    if (production.EndDate < DateTime.UtcNow)
                        throw new UserException("Production instruction already completed.");

                    City.MoveCompletedProductionsToCity();

                    // Move the remaining resources to the city
                    var remainingResources = ((production.Item.Receipe.Items * production.Amount) * (1 - production.CompletionRatio)).ToDictionary();
                    City.Resources += new ItemCollection<ItemObject>(remainingResources);

                    // Move the instructions at the end of the queue to the front.
                    var remainingTime = new[] { DateTime.UtcNow, production.StartDate }.Max() - production.EndDate;

                    var queue = ProductionQueue.ToList();
                    var index = queue.IndexOf(production);

                    for (int i = index + 1; i < queue.Count; i++)
                    {
                        queue[i].StartDate -= remainingTime;
                        queue[i].EndDate -= remainingTime;
                    }

                    // Remove the canceled production from queue
                    ProductionQueue = ProductionQueue.Where(x => x != production).ToArray();
                }
            }

            /// <summary>
            /// Removes all production instructions in the queue. Adds the remaining resources and
            /// completed products to city resources for the production that was interruğted.
            /// </summary>
            public void CancelAllProductionsInQueue()
            {
                // Move the completed productions and interrupted production's remaining resources to the city.
                City.MoveCompletedProductionsToCity();

                // Move the resources of production instructions in queue to the city
                var returningResources = ProductionQueue.Skip(1).SelectMany(x => x.Item.Receipe.Items * x.RemainingAmount).ToDictionary();
                if (returningResources != null)
                    City.Resources += new ItemCollection<ItemObject>(returningResources);

                ProductionQueue = [];   // Clear the queue
            }

            /// <summary>
            /// Clears the productions that completed and moved to city resources.
            /// </summary>
            internal void Clear()
            {
                ProductionQueue = ProductionQueue.Where(x => x.EndDate > DateTime.UtcNow || x.MovedAmount < x.Amount).ToArray();
            }

            public override void MoveProductsToCityResources()
            {
                var queue = ProductionQueue.Where(x => x.MovedAmount < x.CompletedAmount);
                foreach (var pq in queue)
                {
                    City.AddResource(pq.Item, pq.CompletedAmount - pq.MovedAmount);
                    pq.MovedAmount = pq.CompletedAmount;
                }
                Clear();
            }

            /// <summary>
            /// Production insturction
            /// </summary>
            public class ProductionInstruction
            {
                private ItemObject item;

                internal ProductionInstruction(ItemObject item, int amount)
                {
                    ArgumentNullException.ThrowIfNull(item);

                    Id = ObjectId.GenerateNewId();
                    ItemKey = item.Key;
                    this.item = item;
                    Amount = amount;
                }

                [BsonElement]
                public ObjectId Id { get; private set; }

                [BsonElement]
                private string ItemKey { get; set; }

                /// <summary>
                /// Item to be produced
                /// </summary>
                [BsonIgnore]
                public ItemObject Item
                {
                    get
                    {
                        if (item == null || item.Key != ItemKey)
                            item = ItemObjectSet.FindByKey(ItemKey);

                        return item;
                    }
                }

                /// <summary>
                /// Total production amount
                /// </summary>
                [BsonElement]
                public int Amount { get; private set; }

                [BsonElement]
                public DateTime StartDate { get; set; }

                [BsonElement]
                public DateTime EndDate { get; set; }

                /// <summary>
                /// The completion ratio of total production (Between 0 - 1)
                /// </summary>
                public double CompletionRatio
                {
                    get
                    {
                        var totalDuration = (EndDate - StartDate).TotalMilliseconds;
                        var elapsedDuration = (DateTime.UtcNow - StartDate).TotalMilliseconds;
                        return Math.Max(double.Min(elapsedDuration / totalDuration, 1), 0);
                    }
                }

                /// <summary>
                /// Completed amount
                /// </summary>
                public int CompletedAmount => Convert.ToInt32(Math.Floor(Amount * CompletionRatio));

                /// <summary>
                /// Remaining amount
                /// </summary>
                public int RemainingAmount => Amount - CompletedAmount;

                /// <summary>
                /// The completed production amount that moved to city resources
                /// </summary>
                [BsonElement]
                internal int MovedAmount { get; set; }

                public override bool Equals(object? obj)
                {
                    if (obj is ProductionInstruction prod)
                    {
                        return prod.Id == this.Id;
                    }
                    return false;
                }

                public override int GetHashCode()
                {
                    return Id.GetHashCode();
                }
            }
        }
    }
}
