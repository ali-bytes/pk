using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using Domain.EStore.Properties;

namespace Domain.EStore.Repositories
{
    public class Cache
    {

        private static readonly System.Web.Caching.Cache CacheServer;
        private static readonly TimeSpan TimeSpan = new TimeSpan(
             Settings.Default.DefaultCacheDuration_Days,
             Settings.Default.DefaultCacheDuration_Hours,
             Settings.Default.DefaultCacheDuration_Minutes, 0);

        static Cache()
        {
            CacheServer = HttpContext.Current.Cache;
        }

        public object Get(string cacheKey)
        {
            return CacheServer.Get(cacheKey);
        }

        public IEnumerable<string> GetCacheKeys()
        {
            var keys = new List<string>();
            IDictionaryEnumerator ca = CacheServer.GetEnumerator();
            while (ca.MoveNext())
            {
                keys.Add(ca.Key.ToString());
            }
            return keys;
        }

        public void Set(string cacheKey, object cacheObject)
        {

            Set(cacheKey, cacheObject, TimeSpan);

        }

        public void Set(string cacheKey, object cacheObject, DateTime expiration, CacheItemPriority priority = CacheItemPriority.Normal)
        {
            CacheServer.Insert(cacheKey, cacheObject, null, expiration, System.Web.Caching.Cache.NoSlidingExpiration, priority, null);
        }

        public void Set(string cacheKey, object cacheObject, TimeSpan expiration, CacheItemPriority priority = CacheItemPriority.Normal)
        {
            CacheServer.Insert(cacheKey, cacheObject, null, System.Web.Caching.Cache.NoAbsoluteExpiration, expiration, priority, null);
        }

        public void Delete(string cacheKey)
        {
            if (Exists(cacheKey))
                CacheServer.Remove(cacheKey);
        }

        private bool Exists(string cacheKey)
        {
            if (CacheServer[cacheKey] != null)
                return true;
            return false;
        }

        public void Flush()
        {
            foreach (string s in GetCacheKeys())
            {
                Delete(s);
            }
        }

    }
}