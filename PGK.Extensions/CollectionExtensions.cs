using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

/// <summary>
/// 	Extension methods for all kind of Collections implementing the ICollection&lt;T&gt; interface
/// </summary>
public static class CollectionExtensions
{
	/// <summary>
	/// 	Adds a value uniquely to to a collection and returns a value whether the value was added or not.
	/// </summary>
	/// <typeparam name = "T">The generic collection value type</typeparam>
	/// <param name = "collection">The collection.</param>
	/// <param name = "value">The value to be added.</param>
	/// <returns>Indicates whether the value was added or not</returns>
	/// <example>
	/// 	<code>
	/// 		list.AddUnique(1); // returns true;
	/// 		list.AddUnique(1); // returns false the second time;
	/// 	</code>
	/// </example>
	public static bool AddUnique<T>(this ICollection<T> collection, T value)
	{
		var alreadyHas = collection.Contains(value) ;
		if (!alreadyHas)
		{
			collection.Add(value);
		}
		return alreadyHas;
	}

	/// <summary>
	/// 	Adds a range of value uniquely to a collection and returns the amount of values added.
	/// </summary>
	/// <typeparam name = "T">The generic collection value type.</typeparam>
	/// <param name = "collection">The collection.</param>
	/// <param name = "values">The values to be added.</param>
	/// <returns>The amount if values that were added.</returns>
	public static int AddRangeUnique<T>(this ICollection<T> collection, IEnumerable<T> values)
	{
		var count = 0;
		foreach (var value in values)
		{
			if (collection.AddUnique(value))
				count++;
		}
		return count;
	}

	///<summary>
	///	Remove an item from the collection with predicate
	///</summary>
	///<param name = "collection"></param>
	///<param name = "predicate"></param>
	///<typeparam name = "T"></typeparam>
	///<exception cref = "ArgumentNullException"></exception>
	/// <remarks>
	/// 	Contributed by Michael T, http://about.me/MichaelTran
	/// 	Renamed by James Curran, to match corresponding HashSet.RemoveWhere()
	/// </remarks>
	public static void RemoveWhere<T>(this ICollection<T> collection, Predicate<T> predicate)
	{
		if (collection == null)
			throw new ArgumentNullException("collection");
		var deleteList = collection.Where(child => predicate(child)).ToList();
		deleteList.ForEach(t => collection.Remove(t));
	}

    /// <summary>
    /// Tests if the collection is empty.
    /// </summary>
    /// <param name="collection">The collection to test.</param>
    /// <returns>True if the collection is empty.</returns>
    public static bool IsEmpty(this ICollection collection)
    {
        collection.ExceptionIfNullOrEmpty("The collection cannot be null.","collection");

        return collection.Count == 0;
    }

    /// <summary>
    /// Tests if the collection is empty.
    /// </summary>
    /// <typeparam name="T">The type of the items in 
    /// the collection.</typeparam>
    /// <param name="collection">The collection to test.</param>
    /// <returns>True if the collection is empty.</returns>
    public static bool IsEmpty<T>(this ICollection<T> collection)
    {
        collection.ExceptionIfNullOrEmpty(
            "The collection cannot be null.",
            "collection");

        return collection.Count == 0;
    }

    /// <summary>
    /// Tests if the collection is empty.
    /// </summary>
    /// <param name="collection">The collection to test.</param>
    /// <returns>True if the collection is empty.</returns>
    public static bool IsEmpty(this IList collection)
    {
        collection.ExceptionIfNullOrEmpty(
            "The collection cannot be null.",
            "collection");

        return collection.Count == 0;
    }

    /// <summary>
    /// Tests if the collection is empty.
    /// </summary>
    /// <typeparam name="T">The type of the items in 
    /// the collection.</typeparam>
    /// <param name="collection">The collection to test.</param>
    /// <returns>True if the collection is empty.</returns>
    public static bool IsEmpty<T>(this IList<T> collection)
    {
        collection.ExceptionIfNullOrEmpty(
            "The collection cannot be null.",
            "collection");

        return collection.Count == 0;
    }
}
