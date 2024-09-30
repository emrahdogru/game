using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items;
using War.Server.Domain.Items.BuildingObjects;

namespace War.Server.Domain.ObjectSets
{
    public static partial class ItemObjectSet
    {
        private static Dictionary<string, ItemObject>? _items;
        private static IEnumerable<ItemObject>? _itemList;

        private static readonly object _lockObject = new();

        /// <summary>
        /// Oyundaki tüm itemler
        /// </summary>
        public static IDictionary<string, ItemObject> AllItems
        {
            get
            {
                lock (_lockObject)
                {
                    if (_items == null)
                    {
                        IEnumerable<PropertyInfo> properties = typeof(ItemObjectSet)
                            .GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => typeof(ItemObject).IsAssignableFrom(p.PropertyType));

                        _items = properties
                            .Select(x => x.GetValue(null) as ItemObject ?? throw new Exception("Static property access failed."))
                            .ToDictionary(x => x.Key, x => x);
                    }
                }

                return _items;
            }
        }

        public static ItemObject FindByKey(string key)
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            return AllItems[key];
        }

        public static T FindByKey<T>(string key) where T : ItemObject
        {
            var result = FindByKey(key) as T;
            return result ?? throw new KeyNotFoundException($"A {typeof(T).Name} could not be found with the key value {key}.");
        }

        public static IEnumerable<ItemObject> GetAll()
        {
            _itemList ??= AllItems.Select(x => x.Value).ToArray();
            return _itemList;
        }

        public static IEnumerable<T> GetAll<T>() where T : ItemObject
        {
            return GetAll().OfType<T>();
        }
    }
}
