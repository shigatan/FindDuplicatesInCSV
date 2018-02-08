using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    /// <summary>
    /// utils to work with collections
    /// </summary>
    public static class CollectionExtenstions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, List<TValue>> source, TKey key, TValue value)
        {
            List<TValue> currentValue = null;
            if (source.TryGetValue(key, out currentValue))
            {
                source[key].Add(value);
            }
            else
            {
                var list = new List<TValue>() { value };
                source.Add(key, list);
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, List<TValue>> source, TKey key, IEnumerable<TValue> value)
        {
            if (source.ContainsKey(key))
            {
                source[key].AddRange(value);
            }
            else
            {
                var list = value.ToList();
                source.Add(key, list);
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Count() == 0;
        }

        public static T SafeGetByIndex<T>(this T[] source, int index)
        {
            if (source.Count() <= index)
                return default(T);
            else
                return source[index];
        }
    }
}
