using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items;
using War.Server.Domain.Techs;

namespace War.Server.Domain.ObjectSets
{
    public static partial class TechObjectSet
    {
        private static Dictionary<string, TechObject>? _techs;
        private static IEnumerable<TechObject>? _techList;
        private static readonly object _lockObject = new();

        public static IDictionary<string, TechObject> AllTechs
        {
            get
            {
                lock (_lockObject)
                {
                    if (_techs == null)
                    {
                        IEnumerable<PropertyInfo> properties = typeof(TechObjectSet)
                            .GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => typeof(TechObject).IsAssignableFrom(p.PropertyType));

                        _techs = properties
                            .Select(x => x.GetValue(null) as TechObject ?? throw new Exception("Static property access failed."))
                            .ToDictionary(x => x.Key, x => x);
                    }
                }

                return _techs;
            }
        }

        public static TechObject FindByKey(string key)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(key, nameof(key));
            return AllTechs[key];
        }

        public static IEnumerable<TechObject> GetAll()
        {
            _techList ??= AllTechs.Select(x => x.Value).ToArray();
            return _techList;
        }

    }
}
