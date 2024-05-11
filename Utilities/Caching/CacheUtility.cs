using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Caching;

namespace Utilities.Caching
{
    /// <summary>
    /// Memory cache
    /// </summary>
    public class CacheUtility
    {
        /// <summary>
        /// Get value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }

        /// <summary>
        /// Set value with key and expiration time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absExpiration"></param>
        /// <returns></returns>
        public static bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }

        /// <summary>
        /// Clear value by key
        /// </summary>
        /// <param name="key"></param>
        public static void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }

        /// <summary>
        /// Clear all value
        /// </summary>
        public static void ClearAll()
        {
            Type m_Type = typeof(CacheKey);
            FieldInfo[] fields = m_Type.GetFields();
            foreach (FieldInfo fi in fields)
            {
                Delete(fi.GetValue(null).ToString());
            }
        }
    }

    /// <summary>
    /// Dictionary key
    /// </summary>
    public static class CacheKey
    {
        public static string Sitemap = "Sitemap";
    }
}
