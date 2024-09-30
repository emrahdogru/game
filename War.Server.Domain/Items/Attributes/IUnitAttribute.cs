namespace War.Server.Domain.Items.Attributes
{
    public interface IUnitAttribute
    {
        int CargoCapacity { get; }
        double FuelCapacity { get; }
        double FuelConsumption { get; }
        int Range { get; }
        double Speed { get; }
    }
}