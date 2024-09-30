using System.Collections.Immutable;
using War.Server.Domain.Items.BuildingObjects;
using War.Server.Domain.Items.PersonObjects;

namespace War.Server.Domain.Items.Attributes
{
    public interface IBuildingAttribute
    {
        int MaxAmount { get; }
        int MaxWorker { get; }
        ImmutableHashSet<ItemObject> ProducibleItems { get; }
        BuildingObject.BuildingProductionType ProductionType { get; }
        ImmutableHashSet<BuildingObject> RequiredBuildings { get; }
        ImmutableHashSet<PersonObject> WorkablePeople { get; }
    }
}