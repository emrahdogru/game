using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Items;
using War.Server.Domain.Items.Attributes;
using War.Server.Domain.Items.BreedingObjects;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.MapObjects
{
    public partial class City
    {
        public class BuildingContainerBreeding : BuildingContainer
        {
            [BsonElement]
            private Dictionary<string, double> _inputs
            {
                get => RootInputs.ToDictionary(x => x.Key.Key, x => x.Value);
                set => RootInputs = value.ToDictionary(x => ItemObjectSet.FindByKey<BreedingObject>(x.Key), x => x.Value);
            }

            /// <summary>
            /// Amount of items when the production started
            /// </summary>
            [BsonIgnore]
            private Dictionary<BreedingObject, double> RootInputs = [];


            [BsonElement]
            private Dictionary<string, int> _equipment
            {
                get => Equipment.ToDictionary(x => x.Key.Key, x => x.Value);
                set => Equipment = new ItemCollection<ItemObject>(value);
            }

            /// <summary>
            /// Calculates the current amounts based on the root input and the elapsed time
            /// </summary>
            /// <returns></returns>
            private Dictionary<BreedingObject, double> CalculateCurrentAmounts()
            {
                if (ProductionStartDate.HasValue)
                {
                    var elapsedTime = (int)(DateTime.UtcNow - ProductionStartDate.Value).TotalSeconds;

                    return RootInputs.Select(x =>
                    {
                        var productionDuration = elapsedTime / (int)CalculateProductionDuration(x.Key, 1).TotalSeconds;
                        double amount = x.Value * (Math.Pow(1.1, productionDuration));

                        return new
                        {
                            item = x.Key,
                            amount = amount
                        };
                    })
                    .ToDictionary(x => x.item, x => x.amount);
                }
                else
                {
                    return RootInputs;
                }
            }

            /// <summary>
            /// Animals housed in the building
            /// </summary>
            [BsonIgnore]
            public ItemCollection<BreedingObject> Inputs
            {
                get
                {
                    var inputs = CalculateCurrentAmounts().ToDictionary(x => x.Key, x => Convert.ToInt32(Math.Floor(x.Value)));
                    return new ItemCollection<BreedingObject>(inputs);
                }
            }


            /// <summary>
            /// Equipment using in this building.
            /// </summary>
            [BsonIgnore]
            public ItemCollection<ItemObject> Equipment { get; private set; } = [];

            public BuildingContainerBreeding(ItemCollection<ItemObject> equipment)
            {
                Equipment = equipment;
            }

            [BsonElement]
            protected DateTime? ProductionStartDate { get; set; }

            public virtual void AddInputs(ItemCollection<BreedingObject> animals)
            {
                ArgumentNullException.ThrowIfNull(nameof(animals));

                if (animals.Any(x => x.Value < 0))
                    throw new ArgumentOutOfRangeException(nameof(animals), "Animal count could not be negative.");

                var inputs = CalculateCurrentAmounts();

                var totalMass = inputs.Sum(x => x.Key.Mass * x.Value) + animals.Sum(x => x.Key.Mass * x.Value);

                if (totalMass > ((IBreedingBuilding)Building).Capacity)
                    throw new UserException($"Building capacity exceeded.");

                var animalsCast = animals.CastTo<ItemObject>();
                City.Resources.CheckEnough(animalsCast);

                foreach(var a in animals)
                {
                    var amount = inputs.GetValueOrDefault(a.Key, 0) + a.Value;
                    inputs[a.Key] = amount;
                }

                RootInputs = inputs;
                City.Resources -= animalsCast;

                ProductionStartDate = DateTime.UtcNow;
            }

            public virtual void SetEquipment(ItemCollection<ItemObject> equipments)
            {
                ArgumentNullException.ThrowIfNull(nameof(equipments));

                if (equipments.Any(x => x.Value < 0))
                    throw new ArgumentOutOfRangeException(nameof(equipments), "Equipment count could not be negative.");

                if (equipments.Sum(x => x.Value) > ((IBreedingBuilding)Building).MaxEquipmentCount)
                    throw new UserException($"Equipment slots full.");

                // TODO Check if the equipment can be used in this building.



                var allItems = City.Resources + Equipment;

                allItems.CheckEnough(equipments);

                Equipment = equipments;
                City.Resources = allItems - equipments;


            }

            protected override TimeSpan CalculateProductionDuration(ItemObject item, int amount)
            {
                var duration = base.CalculateProductionDuration(item, amount);
                return duration;
            }


            public override void MoveProductsToCityResources()
            {
                throw new NotImplementedException();
            }
        }
    }
}
