using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Switcheroo
{
    public class HotKey
    {
        public void LoadSettings() { }
    }
}

namespace Switcheroo.Properties
{
    public class Settings
    {
        public static Settings Default = new Settings();

        public List<object> Exceptions
        {
            get { return new List<object>(); }
        }
    }
}

namespace System.Runtime.Caching
{
    public class MemoryCache
    {
        public static MemoryCache Default = new MemoryCache();

        System.Web.Caching.Cache cache;

        public MemoryCache()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, "http://switcheroo.io", null),
                new HttpResponse(null));

            cache = HttpContext.Current.Cache;
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }

        public void Add(string key, object value, DateTimeOffset dateTimeOffset)
        {
            cache.Add(key, value, null, dateTimeOffset.UtcDateTime, System.Web.Caching.Cache.NoSlidingExpiration, Web.Caching.CacheItemPriority.Default, null);
        }
    }
}