using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using War.Server.Domain.Items.PersonObjects;
using static War.Server.Domain.Items.BuildingObjects.BuildingObject;
using static War.Server.Domain.MapObjects.City;

namespace War.Server.Models.Results
{
    public abstract class BuildingContainerResult(BuildingContainer bc)
    {
        public ObjectId Id => bc.Id;

        public int Index { get; } = bc.Index;
        public string BuildingKey => bc.Building.Key;
        public ItemCollectionResult<PersonObject> Workers => new ItemCollectionResult<PersonObject>(bc.Workers);

        public bool IsConstructionCompleted => bc.IsConstructionCompleted();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalConstructionTime { get; } = bc.IsConstructionCompleted() ? null : Convert.ToInt32((bc.ConstructionEndDate - bc.ConstructionStartDate).TotalSeconds);

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? RemainingTimeForConstruction => bc.IsConstructionCompleted() ? null : Convert.ToInt32(Math.Max(0, (bc.ConstructionEndDate - DateTime.UtcNow).TotalSeconds));

        public BuildingProductionType ProductionType => bc.Building.ProductionType;

        public static BuildingContainerResult Create(BuildingContainer bc)
        {
            return bc.GetType().Name switch
            {
                nameof(BuildingContainerContinious) => new BuildingContainerContiniousResult((BuildingContainerContinious)bc),
                nameof(BuildingContainerSequental) => new BuildingContainerSequentalResult((BuildingContainerSequental)bc),
                nameof(BuildingContainerNone) => new BuildingContainerNoneResult((BuildingContainerNone)bc),
                _ => throw new NotImplementedException($"Result type is not defined for type`{bc.GetType().FullName}`")
            };
        }

    }

    public class BuildingContainerContiniousResult : BuildingContainerResult
    {
        private BuildingContainerContinious bc;

        public  BuildingContainerContiniousResult(BuildingContainerContinious bc)
            : base(bc) {
            this.bc = bc;
        }

        public string? ProductKey => bc.Product?.Key;
        public int DurationPerItem => Convert.ToInt32(bc.DurationPerItem.TotalSeconds);
    }

    public class BuildingContainerSequentalResult: BuildingContainerResult
    {
        private readonly BuildingContainerSequental bc;

        public BuildingContainerSequentalResult(BuildingContainerSequental bc): base(bc)
        {
            this.bc = bc;
        }

        public IEnumerable<ProductionInstructionResult> ProductionQueue => bc.ProductionQueue.Select(x => new ProductionInstructionResult(x));
    }

    public class BuildingContainerNoneResult(BuildingContainer bc) : BuildingContainerResult(bc)
    {

    }
}
