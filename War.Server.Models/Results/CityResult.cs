using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain;
using War.Server.Domain.MapObjects;
using War.Server.Models.Summaries;

namespace War.Server.Models.Results
{
    public class CityResult(City city)
    {
        public ObjectId Id { get; } = city.Id;
        public int CargoCapacity { get; } = city.CargoCapacity;
        public Point Location { get; } = city.Location;
        public string Name { get; } = city.Name;
        public int FreeCapacity { get; } = city.FreeCapacity;
        public int UsedCapacity { get; } = city.UsedCapacity;
        public Dictionary<string, int> Resources { get; } = city.Resources.ToDictionary(x => x.Key.Key, x => x.Value);
        public IEnumerable<BuildingContainerResult> Buildings { get; } = city.Buildings.Select(x => BuildingContainerResult.Create(x));
        public IEnumerable<string> ConstructibleBuildings { get; } = city.GetConstructibleBuildings().Select(x => x.Key);
        public UserSummary? User { get; } = city.User == null ? null : new (city.User);
    }
}
