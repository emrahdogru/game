using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.BuildingObjects;
using War.Server.Domain.Items;
using War.Server.Domain.ObjectSets;
using War.Server.Domain.Exceptions;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using War.Server.Domain.Items.PersonObjects;
using System.Diagnostics;

namespace War.Server.Domain.MapObjects
{
    public partial class City
    {
        [BsonKnownTypes(
            typeof(BuildingContainerNone),
            typeof(BuildingContainerContinious),
            typeof(BuildingContainerSequental),
            typeof(BuildingContainerBreeding)
            )]
        public abstract class BuildingContainer
        {
            private BuildingObject? _building;


            [BsonElement]
            public ObjectId Id { get; private set; }

            [BsonElement]
            public int Index { get; internal set; }

            /// <summary>
            /// Creates a <see cref="BuildingContainer"/> object according to production type
            /// </summary>
            /// <param name="building"></param>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            internal static BuildingContainer Create(BuildingObject building)
            {
                BuildingContainer container = building.ProductionType switch
                {
                    BuildingObject.BuildingProductionType.None => new BuildingContainerNone(),
                    BuildingObject.BuildingProductionType.Sequental => new BuildingContainerSequental(),
                    BuildingObject.BuildingProductionType.Continious => new BuildingContainerContinious(),
                    BuildingObject.BuildingProductionType.Breeding => new BuildingContainerBreeding(),
                    _ => throw new NotImplementedException($"`{building.ProductionType}` üretim tipi için tanımlı container yok.")
                };

                container.Id = ObjectId.GenerateNewId();
                container._building = building;
                container.BuildingObjectKey = building.Key;
                container.ConstructionStartDate = DateTime.UtcNow;
                container.ConstructionEndDate = DateTime.UtcNow.Add(building.Receipe.Duration);

                return container;
            }

            [BsonElement]
            private string BuildingObjectKey { get; set; } = null!;

            [BsonIgnore]
            public BuildingObject Building
            {
                get
                {
                    if (_building == null || _building.Key != BuildingObjectKey)
                        _building = ItemObjectSet.FindByKey<BuildingObject>(BuildingObjectKey);

                    return _building;
                }
            }

            /// <summary>
            /// Construction start date
            /// </summary>
            [BsonElement]
            public DateTime ConstructionStartDate { get; private set; }

            /// <summary>
            /// Construction completion date
            /// </summary>
            [BsonElement]
            public DateTime ConstructionEndDate { get; private set; }

            /// <summary>
            /// Returns true if construction completed
            /// </summary>
            /// <returns></returns>
            public bool IsConstructionCompleted()
            {
                return ConstructionEndDate <= DateTime.UtcNow;
            }

            /// <summary>
            /// The city where the building is located
            /// </summary>
            [BsonIgnore]
            public City City { get; internal set; } = null!;

            /// <summary>
            /// Calculates production time
            /// </summary>
            /// <param name="item">Item to be produced</param>
            /// <param name="amount">Production amount</param>
            /// <returns>Production time</returns>
            protected virtual TimeSpan CalculateProductionDuration(ItemObject item, int amount)
            {
                var totalDuration = item.Receipe.Duration.TotalSeconds * amount;

                Workers
                    .Select(x => new {
                        x.Key.EfficiencyFactor,
                        Amount = x.Value
                    })
                    .OrderByDescending(x => x.EfficiencyFactor)
                    .ToList()
                    .ForEach(x =>
                    {
                        for (int k = 0; k < x.Amount; k++)
                            totalDuration *= (1 - (0.05 * x.EfficiencyFactor));
                    });

                return new TimeSpan(0, 0, Convert.ToInt32(Math.Round(totalDuration)));
            }

            [BsonElement]
            private Dictionary<string, int> _workers
            {
                get
                {
                    return Workers.ToDictionary(x => x.Key.Key, x => x.Value);
                }
                set
                {
                    var dict = (value ?? []).ToDictionary(x => (PersonObject)ItemObjectSet.FindByKey(x.Key), x => x.Value);
                    Workers = new ItemCollection<PersonObject>(dict);
                }
            }

            /// <summary>
            /// List of <see cref="Items.PersonObjects.PersonObject"/> working in this building.
            /// </summary>
            [BsonIgnore]
            public ItemCollection<PersonObject> Workers { get; protected set; } = [];

            /// <summary>
            /// Set the workers working in the building
            /// </summary>
            /// <param name="workerCount"></param>
            public virtual void SetWorkers(ItemCollection<PersonObject> workers)
            {
                ArgumentNullException.ThrowIfNull(nameof(workers));

                if (workers.Any(x => x.Value < 0))
                    throw new ArgumentOutOfRangeException(nameof(workers), "Worker count could not be negative.");

                var nonworkable = workers.Keys.FirstOrDefault(x => !Building.WorkablePeople.Contains(x));
                if (nonworkable != null)
                    throw new UserException($"A {nonworkable.Name} cannot work at the {Building.Name}");

                if (workers.Sum(x => x.Value) > Building.MaxWorker)
                    throw new UserException($"A maximum of {Building.MaxWorker} people can work in this building");

                var workersCast = workers.CastTo<ItemObject>();

                var allItems = City.Resources + this.Workers.CastTo<ItemObject>();

                allItems.CheckEnough(workersCast);

                Workers = workers;
                City.Resources = allItems - workersCast;
            }

            public abstract void MoveProductsToCityResources();

            public override bool Equals(object? obj)
            {
                if (obj is BuildingContainer container)
                    return container.Id == this.Id;

                return false;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }
    }
}
