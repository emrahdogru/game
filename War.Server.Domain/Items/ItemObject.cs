using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.UnitObjects;
using War.Server.Domain.ObjectSets;
using War.Server.Domain.Techs;

namespace War.Server.Domain.Items
{

    public abstract class ItemObject
    {
        private string? _key = null;
        private string[]? _attrubutes = null;
        protected ReceipeDetail? _receipe = null;

        public string Key
        {
            get
            {
                _key ??= GetType().Name;
                return _key;
            }
        }

        public string[] Attributes
        {
            get
            {
                _attrubutes ??= GetType().GetInterfaces().Where(x => x.Name.EndsWith("Attribute")).Select(x => x.Name[1..^9].ToLower()).ToArray();
                return _attrubutes;
            }
        }

        /// <summary>
        /// Object type
        /// </summary>
        public abstract string Type { get; }

        public abstract string Name { get; }


        /// <summary>
        /// Mass. Mass is related to carrying and capacity.
        /// </summary>
        public abstract int Mass { get; }

        /// <summary>
        /// Can carry to other items
        /// </summary>
        public abstract bool CanCarry { get; }

        /// <summary>
        /// Can be carried by items with the <see cref="CanCarry"/> property.
        /// </summary>
        public abstract bool IsCarryable { get; }

        /// <summary>
        /// Strength
        /// </summary>
        public abstract int Strength { get; }

        /// <summary>
        /// Production receipe
        /// </summary>
        public abstract ReceipeDetail Receipe { get; }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is ItemObject other)
            {
                return this.Key == other.Key;
            }

            return false;
        }

        public static bool operator ==(ItemObject? p1, ItemObject? p2)
        {
            return p1?.Key == p2?.Key;
        }

        public static bool operator !=(ItemObject? p1, ItemObject? p2)
        {
            return !(p1 == p2);
        }

        public class ReceipeDetail
        {
            public ReceipeDetail(TimeSpan duration, ItemCollection<ItemObject> items)
            {
                ArgumentOutOfRangeException.ThrowIfNegative(duration.TotalSeconds, nameof(duration));
                ArgumentNullException.ThrowIfNull(items, nameof(items));

                Duration = duration;
                Items = items;
                TechObjects = [];
            }
            public ReceipeDetail(int durationAsSecond, Dictionary<ItemObject, int> items, params TechObject[] techs)
            {
                ArgumentOutOfRangeException.ThrowIfNegative(durationAsSecond, nameof(durationAsSecond));
                ArgumentNullException.ThrowIfNull(items, nameof(items));
                ArgumentNullException.ThrowIfNull(techs, nameof(techs));

                Duration = new TimeSpan(0, 0, durationAsSecond);
                this.Items = new ItemCollection<ItemObject>(items);
                this.TechObjects = techs;
            }

            /// <summary>
            /// Üretim süresi
            /// </summary>
            public TimeSpan Duration { get; }

            /// <summary>
            /// Gereken itemler ve miktarları
            /// </summary>
            public ItemCollection<ItemObject> Items { get; } 

            /// <summary>
            /// Gereken teknolojiler
            /// </summary>
            public IEnumerable<TechObject> TechObjects { get; }

        }
    }
}
