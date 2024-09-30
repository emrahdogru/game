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

namespace War.Server.Domain.ObjectSets
{
    public static partial class ItemObjectSet
    {
        #region reource
        public static Coal Coal { get; } = new();
        public static Food Food { get; } = new();
        public static Grain Grain { get; } = new();
        public static Iron Iron { get; } = new();
        public static Petrol Petrol { get; } = new();
        public static Stone Stone { get; } = new();
        public static Water Water { get; } = new();
        public static Wood Wood { get; } = new();
        public static Milk Milk { get; } = new();
        public static Cattle Cattle { get; } = new();
        public static Sheep Sheep { get; } = new();
        public static Fertilizer Fertilizer { get; } = new();
        #endregion

        #region building
        public static Armory Armory { get; } = new();
        public static Barracks Barracks { get; } = new();
        public static Foundry Foundry { get; } = new();
        public static Ranch Ranch { get; } = new();
        public static TownCenter TownCenter { get; } = new();
        public static TradeCenter TradeCenter { get; } = new();
        public static Well Well { get; } = new();
        public static Farm Farm { get; } = new();
        public static Dairy Dairy { get; } = new();
        #endregion



        #region tool
        public static Grenade Grenade { get; } = new();
        public static GunPowder GunPowder { get; } = new();
        public static Rifle Rifle { get; } = new();
        public static Butter Butter { get; } = new();
        public static Cheese Cheese { get; } = new();
        
        #endregion

        #region unit
        #region person
        public static Engineer Engineer { get; } = new();
        public static Medic Medic { get; } = new();
        public static Person Person { get; } = new();
        #endregion

        public static Artillery Artillery { get; } = new();
        public static Sniper Sniper { get; } = new();
        public static Soldier Soldier { get; } = new();
        public static SpecialForce SpecialForce { get; } = new();
        public static Tank Tank { get; } = new();
        public static Truck Truck { get; } = new();
        public static Wool Wool { get; } = new();
        public static Meat Meat { get; } = new();
        public static Leather Leather { get; } = new();
        public static Egg Egg { get; } = new();

        #endregion

    }
}
