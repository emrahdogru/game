using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items;
using War.Server.Domain.Items.BreedingObjects;
using War.Server.Domain.Items.BuildingObjects;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.Items.MidObjects;
using War.Server.Domain.Items.UnitObjects;
using static War.Server.Domain.Items.BuildingObjects.BuildingObject;

namespace War.Server.Models.Results
{
    public class ItemObjectResult(Domain.Items.ItemObject item)
    {
        public static ItemObjectResult Create(Domain.Items.ItemObject item)
        {
            return item.Type switch
            {
                nameof(BuildingObject) => new BuildingObjectResult((BuildingObject)item),
                nameof(ResourceObject) => new ResourceObjectResult((Domain.Items.ResourceObjects.ResourceObject)item),
                nameof(MidObject) => new ToolObjectResult((MidObject)item),
                nameof(UnitObject) => new UnitObjectResult((UnitObject)item),
                nameof(PersonObject) => new PersonObjectResult((PersonObject)item),
                nameof(BreedingObject) => new ItemObjectResult(item),
                _ => throw new NotImplementedException("Unknown object type: " + item.Type)
            };
        }

        public string Key { get; } = item.Key;
        public string[] Attributes { get; } = item.Attributes;
        public string Name { get; } = item.Name;
        public bool CanCarry { get; } = item.CanCarry;
        public bool IsCarryable { get; } = item.IsCarryable;
        public int Strength { get; } = item.Strength;
        public int Mass { get; } = item.Mass;
        public string Type { get; } = item.Type;
        public ReceipeDetailResult Receipe { get; } = new ReceipeDetailResult(item.Receipe);

        public class ReceipeDetailResult
        {
            public ReceipeDetailResult(Domain.Items.ItemObject.ReceipeDetail receipe)
            {
                this.Duration = Convert.ToInt32(receipe.Duration.TotalSeconds);
                this.Items = receipe.Items.ToDictionary(x => x.Key.Key, x => x.Value);
                this.TechObjects = receipe.TechObjects.Select(x => x.Key);
            }

            public int Duration { get; }
            public Dictionary<string, int> Items { get; }
            public IEnumerable<string> TechObjects { get; }
        }
    }

    public class BuildingObjectResult(BuildingObject item) : ItemObjectResult(item)
    {
        public int MaxAmount { get; } = item.MaxAmount;
        public int MaxWorker { get; } = item.MaxWorker;
        public BuildingProductionType ProductionType { get; } = item.ProductionType;
        public IEnumerable<string> ProducibleItems { get; } = item.ProducibleItems.Select(x => x.Key).ToArray();
        public IEnumerable<string> WorkablePeople { get; } = item.WorkablePeople.Select(x => x.Key).ToArray();
    }

    public class ResourceObjectResult(Domain.Items.ResourceObjects.ResourceObject item) : ItemObjectResult(item) { }

    public class ToolObjectResult(MidObject item) : ItemObjectResult(item) { }

    public class UnitObjectResult(UnitObject item) : ItemObjectResult(item) { }
    public class PersonObjectResult(PersonObject item) : UnitObjectResult(item) { }

}
