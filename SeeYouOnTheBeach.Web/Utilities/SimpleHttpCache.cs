using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace SeeYouOnTheBeach.Web.Utilities
{
    public class SimpleHttpCache
    {
        public T Get<T>(string key, Func<T> callback)
        {
            var item = HttpRuntime.Cache.Get(key);

            if (item == null)
            {
                return callback();
            }

            return (T)item;
        }

        public async Task<T> Get<T>(string key, Func<Task<T>> callback)
        {
            var item = HttpRuntime.Cache.Get(key);

            if (item == null)
            {
                return await callback();
            }

            return (T)item;
        }

        public void Set<T>(string key, T item)
        {
            Set(key, item, 0);
        }

        public void Set<T>(string key, T item, int timeoutms)
        {
            lock (this)
            {
                if (timeoutms <= 0)
                    // no timeout
                    HttpRuntime.Cache.Insert(key, item);
                else
                    // sliding expiration
                    HttpRuntime.Cache.Insert(key, item, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMilliseconds(timeoutms));
            }
        }
    }
}