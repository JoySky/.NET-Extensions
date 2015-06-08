using System;
using System.Collections.Concurrent;

public static class ConcurrentExtensions
{
    /// <summary>
    /// Uses Lazy<T> to synchronize a valueFactory
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="this"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    /// <remarks>
    /// Contributed by Chris Gessler
    /// </remarks>
    public static TValue GetOrAdd<TKey, TValue>(this ConcurrentDictionary<TKey, Lazy<TValue>> @this, TKey key, Func<TKey, TValue> valueFactory)
    {
        return @this.GetOrAdd(key, new Lazy<TValue>(() => valueFactory(key), true)).Value;
    }

    /// <summary>
    /// Uses Lazy<T> to synchronize a valueFactory and an updateFactory
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="this"></param>
    /// <param name="key"></param>
    /// <param name="addValueFactory"></param>
    /// <param name="updateValueFactory"></param>
    /// <returns></returns>
    /// <remarks>
    /// Contributed by Chris Gessler
    /// </remarks>
    public static TValue AddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, Lazy<TValue>> @this, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
    {
        return @this.AddOrUpdate(key, new Lazy<TValue>(() => addValueFactory(key), true), (k, v) => new Lazy<TValue>(() => updateValueFactory(k, v.Value), true)).Value;
    }
}



