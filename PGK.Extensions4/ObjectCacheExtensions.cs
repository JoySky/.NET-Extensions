using System;
using System.Runtime.Caching;

public static class ObjectCacheExtensions
{
    /// <summary>
    /// Adds the GetOrAdd method to the ObjectCache class, similar to the GetOrAdd method found in the ConcurrentDictionary class
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="this"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <param name="policy"></param>
    /// <returns></returns>
    public static TValue GetOrAdd<TKey, TValue>(this ObjectCache @this, TKey key, Func<TKey, TValue> valueFactory, CacheItemPolicy policy)
    {
        Lazy<TValue> lazy = new Lazy<TValue>(() => valueFactory(key), true);
        return ((Lazy<TValue>)@this.AddOrGetExisting(key.ToString(), lazy, policy) ?? lazy).Value;
    }
}

