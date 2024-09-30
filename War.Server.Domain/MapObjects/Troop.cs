using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics;
using System.Numerics;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.Items.UnitObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.MapObjects
{
    /// <summary>
    /// Birlik
    /// </summary>
    public class Troop : MapObject
    {
        private UnitObject? _unit;

        public Troop(GameBoard game, Point location) : base(game)
        {
            if (!game.IsValidPoint(location))
                throw new InvalidPointException(location);

            StartDate = EndDate = DateTime.UtcNow;
            StartPoint = EndPoint = location;
        }

        [BsonElement]
        public Point? StartPoint { get; private set; }

        [BsonElement]
        public DateTime? StartDate { get; private set; }

        [BsonElement]
        public Point EndPoint { get; private set; }

        [BsonElement]
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// Current location of the troop
        /// </summary>
        public override Point Location
        {
            get
            {
                if (EndDate <= DateTime.UtcNow || !StartDate.HasValue || !StartPoint.HasValue)
                    return EndPoint;

                var totalDuration = this.EndDate - StartDate.Value;
                var elapsedDuration = DateTime.UtcNow - StartDate.Value;

                return Point.CalculatePosition(this.StartPoint.Value, this.EndPoint, totalDuration, elapsedDuration);
            }
        }

        public double MaxSpeed => Unit?.Speed ?? 0;

        [BsonElement]
        public double Speed { get; private set; }

        [BsonElement]
        private string? UnitKey { get; set; }

        [BsonIgnore]
        public UnitObject? Unit
        {
            get
            {
                if (UnitKey == null)
                    return null;

                if (_unit == null || _unit.Key != UnitKey)
                    _unit = ItemObjectSet.FindByKey<UnitObject>(UnitKey);

                return _unit;
            }
            private set
            {
                _unit = value;
                UnitKey = value?.Key;
            }
        }

        [BsonElement]
        public int Amount { get; private set; }

        [BsonElement]
        public override int CargoCapacity => (this.Unit?.CargoCapacity ?? 0) * this.Amount;

        public double FuelCapacity => Unit?.FuelCapacity * Amount ?? 0;

        public double FuelConsumption => Unit?.FuelConsumption * Amount ?? 0;

        [BsonElement]
        private double FilledFuel { get; set; }

        public double AvailableFuel
        {
            get
            {
                double avalableFuel = 0;
                if (FuelCapacity == 0)
                {
                    avalableFuel = 0;
                }
                else if (StartPoint.HasValue)
                {
                    avalableFuel = FilledFuel - CalculateFuelConsumption(StartPoint.Value, Location);
                }
                else
                {
                    avalableFuel = FilledFuel;
                }
                return avalableFuel;
            }
        }

        public void SetUnit(MapObject source, UnitObject newUnit, int amount)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(newUnit, nameof(newUnit));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount, nameof(amount));
            GameBoardMismatchException.ThrowIfNotEqual(source.GameBoard, GameBoard);

            if (source.IsMoving() || IsMoving())
                throw new UserException("Transfer can not be carried out between moving troops.");

            var distance = source.Location.CalculateDistance(Location);

            if (distance > 1)
                throw new InvalidPointException(Location, "Troop is too far for transfer.");

            if (Unit != null && Unit.IsCarryable == false && source is Troop)
                throw new UserException($"{Unit.Name} cannot be carried as cargo, so it cannot be moved to the source troop.");

            var fuel = AvailableFuel;
            var speed = Speed;

            source.RemoveResource(newUnit, amount); // Remove the new units from the source

            if(Unit == newUnit)
            {
                Amount += amount;
            }
            else
            {
                if(Unit != null)
                    source.AddResource(Unit, Amount);
                Unit = Unit;
                Amount = amount;
            }

            Speed = MaxSpeed;

            if (FuelCapacity >= fuel)
            {
                FilledFuel = fuel;
            }
            else
            {
                FilledFuel = FuelCapacity;
                int returningFuel = Convert.ToInt32(Math.Floor(fuel - FuelCapacity));
                if(returningFuel > 0)
                {
                    var petrol = ItemObjectSet.FindByKey(nameof(Petrol));
                    try
                    {
                        AddResource(petrol, returningFuel);
                    }
                    catch(NotEnoughCapacityException)
                    {
                        try
                        {
                            source.AddResource(petrol, returningFuel);
                        }
                        catch(NotEnoughCapacityException)
                        {

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Birliği <paramref name="destination"/> konumuna yönlendirir.
        /// </summary>
        /// <param name="destination"></param>
        /// <exception cref="UserException"></exception>
        public void GoTo(Point destination)
        {
            if (Speed <= Math.Max(0, (Unit?.Speed ?? 0)) * 0.5)
                throw new UserException("Troop cannot move. Speed is zero!");

            var fuelConsumption = CalculateFuelConsumption(Location, destination);
            if (fuelConsumption > AvailableFuel)
                throw new UserException("There is not enough fuel to reach this location.");

            var distance = this.Location.CalculateDistance(destination);
            var duration = (distance / Speed);    // Dakika

            FilledFuel = AvailableFuel;
            StartPoint = Location;
            EndPoint = destination;
            StartDate = DateTime.UtcNow;
            EndDate = StartDate.Value.AddMinutes(duration);
        }

        public void Stop()
        {
            EndPoint = Location;
            StartDate = null;
            StartPoint = null;
            FilledFuel = AvailableFuel;
        }

        public double CalculateFuelConsumption(Point source, Point destination)
        {
            double consumption = 0;

            // consumption by distance
            var distance = source.CalculateDistance(destination);
            consumption += (FuelConsumption * distance) * 0.6;

            // consumption by duration
            var duration = distance / Speed;
            consumption += (FuelConsumption * duration) * 0.1;

            // consumption by cargo mass
            var cargoMass = Resources.Sum(x => x.Key.Mass * x.Value);
            consumption += (FuelConsumption * (cargoMass / CargoCapacity)) * 0.3;

            return consumption;
        }

        /// <summary>
        /// Fills the fuel tank from the troop resources
        /// </summary>
        /// <param name="amount">Amount of fuel</param>
        /// <exception cref="NotEnoughCapacityException"></exception>
        /// <exception cref="NotEnoughResourceException"></exception>
        public void FillFuel(int amount)
        {
            var petrol = ItemObjectSet.FindByKey(nameof(Petrol));
            Resources.CheckEnough(petrol, amount);

            var freeFuelCapacity = FuelCapacity - AvailableFuel;

            if(amount > freeFuelCapacity)
                throw new NotEnoughCapacityException(Convert.ToInt32(freeFuelCapacity), amount);

            FilledFuel += amount;
            RemoveResource(petrol, amount);
        }

        public override bool IsMoving()
        {
            return EndDate > DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a new troop
        /// </summary>
        /// <param name="context"></param>
        /// <param name="player"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <exception cref="InvalidPointException"></exception>
        public static Troop Create(MapObject source, UnitObject unit, int amount, Point location)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(unit, nameof(unit));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount, nameof(amount));
            if (!source.GameBoard.IsValidPoint(location))
                throw new InvalidPointException(location);

            if (source.User == null)
                throw new UserException("Source object do not have an owner.");

            source.RemoveResource(unit, amount);

            var troop = new Troop(source.GameBoard, location);
            troop.User = source.User;
            troop.StartDate = null;
            troop.EndDate = DateTime.UtcNow;
            troop.StartPoint = null;
            troop.EndPoint = location;
            troop.Unit = unit;
            troop.Amount = amount;
            return troop;
        }

    }
}
