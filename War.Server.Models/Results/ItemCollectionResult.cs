using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items;

namespace War.Server.Models.Results
{
    public class ItemCollectionResult<TKey> : IReadOnlyDictionary<string, int> where TKey : ItemObject
    {
        private Dictionary<string, int> items;

        public ItemCollectionResult(ItemCollection<TKey> items)
        {
            this.items = items.Where(x => x.Value > 0).ToDictionary(x => x.Key.Key, x => x.Value);
        }

        public int this[string key] { get => items[key]; set => items[key] = value; }

        public int Count => items.Count;

        public bool IsReadOnly => true;

        IEnumerable<string> IReadOnlyDictionary<string, int>.Keys => items.Keys;

        IEnumerable<int> IReadOnlyDictionary<string, int>.Values => items.Values;

        public bool ContainsKey(string key)
        {
            return items.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out int value)
        {
            return items.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
