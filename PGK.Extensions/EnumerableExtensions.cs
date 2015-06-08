using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 	Extension methods for all kinds of (typed) enumerable data (Array, List, ...)
/// </summary>
public static class EnumerableExtensions
{
	/// <summary>
	/// 	Converts all items of a list and returns them as enumerable.
	/// </summary>
	/// <typeparam name = "TSource">The source data type</typeparam>
	/// <typeparam name = "TTarget">The target data type</typeparam>
	/// <param name = "source">The source data.</param>
	/// <returns>The converted data</returns>
	/// <example>
	/// 	var values = new[] { "1", "2", "3" };
	/// 	values.ConvertList&lt;string, int&gt;().ForEach(Console.WriteLine);
	/// </example>
	public static IEnumerable<TTarget> ConvertList<TSource, TTarget>(this IEnumerable<TSource> source)
	{
		if (source == null)
			throw new ArgumentNullException("source");
		return source.Select(value => value.ConvertTo<TTarget>());
	}

	/// <summary>
	/// 	Performs an action for each item in the enumerable
	/// </summary>
	/// <typeparam name = "T">The enumerable data type</typeparam>
	/// <param name = "values">The data values.</param>
	/// <param name = "action">The action to be performed.</param>
	/// <example>
	/// 	var values = new[] { "1", "2", "3" };
	/// 	values.ConvertList&lt;string, int&gt;().ForEach(Console.WriteLine);
	/// </example>
	/// <remarks>
	/// 	This method was intended to return the passed values to provide method chaining. Howver due to defered execution the compiler would actually never run the entire code at all.
	/// </remarks>
	public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
	{
		foreach (var value in values)
			action(value);
	}

	///<summary>
	///	Returns enumerable object based on target, which does not contains null references.
	///	If target is null reference, returns empty enumerable object.
	///</summary>
	///<typeparam name = "T">Type of items in target.</typeparam>
	///<param name = "target">Target enumerable object. Can be null.</param>
	///<example>
	///	object[] items = null;
	///	foreach(var item in items.NotNull()){
	///	// result of items.NotNull() is empty but not null enumerable
	///	}
	/// 
	///	object[] items = new object[]{ null, "Hello World!", null, "Good bye!" };
	///	foreach(var item in items.NotNull()){
	///	// result of items.NotNull() is enumerable with two strings
	///	}
	///</example>
	///<remarks>
	///	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	///</remarks>
	public static IEnumerable<T> IgnoreNulls<T>(this IEnumerable<T> target)
	{
		if (ReferenceEquals(target, null))
			yield break;

		foreach (var item in target.Where(item => !ReferenceEquals(item, null)))
			yield return item;
	}

	/// <summary>
	/// 	Returns the maximum item based on a provided selector.
	/// </summary>
	/// <typeparam name = "TItem">The item type</typeparam>
	/// <typeparam name = "TValue">The value item</typeparam>
	/// <param name = "items">The items.</param>
	/// <param name = "selector">The selector.</param>
	/// <param name = "maxValue">The max value as output parameter.</param>
	/// <returns>The maximum item</returns>
	/// <example>
	/// 	<code>
	/// 		int age;
	/// 		var oldestPerson = persons.MaxItem(p =&gt; p.Age, out age);
	/// 	</code>
	/// </example>
	public static TItem MaxItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector, out TValue maxValue)
		where TItem : class
		where TValue : IComparable
	{
		TItem maxItem = null;
		maxValue = default(TValue);

		foreach (var item in items)
		{
			if (item == null)
				continue;

			var itemValue = selector(item);

			if ((maxItem != null) && (itemValue.CompareTo(maxValue) <= 0))
				continue;

			maxValue = itemValue;
			maxItem = item;
		}

		return maxItem;
	}

	/// <summary>
	/// 	Returns the maximum item based on a provided selector.
	/// </summary>
	/// <typeparam name = "TItem">The item type</typeparam>
	/// <typeparam name = "TValue">The value item</typeparam>
	/// <param name = "items">The items.</param>
	/// <param name = "selector">The selector.</param>
	/// <returns>The maximum item</returns>
	/// <example>
	/// 	<code>
	/// 		var oldestPerson = persons.MaxItem(p =&gt; p.Age);
	/// 	</code>
	/// </example>
	public static TItem MaxItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector)
		where TItem : class
		where TValue : IComparable
	{
		TValue maxValue;

		return items.MaxItem(selector, out maxValue);
	}

	/// <summary>
	/// 	Returns the minimum item based on a provided selector.
	/// </summary>
	/// <typeparam name = "TItem">The item type</typeparam>
	/// <typeparam name = "TValue">The value item</typeparam>
	/// <param name = "items">The items.</param>
	/// <param name = "selector">The selector.</param>
	/// <param name = "minValue">The min value as output parameter.</param>
	/// <returns>The minimum item</returns>
	/// <example>
	/// 	<code>
	/// 		int age;
	/// 		var youngestPerson = persons.MinItem(p =&gt; p.Age, out age);
	/// 	</code>
	/// </example>
	public static TItem MinItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector, out TValue minValue)
		where TItem : class
		where TValue : IComparable
	{
		TItem minItem = null;
		minValue = default(TValue);

		foreach (var item in items)
		{
			if (item == null)
				continue;
			var itemValue = selector(item);

			if ((minItem != null) && (itemValue.CompareTo(minValue) >= 0))
				continue;
			minValue = itemValue;
			minItem = item;
		}

		return minItem;
	}

	/// <summary>
	/// 	Returns the minimum item based on a provided selector.
	/// </summary>
	/// <typeparam name = "TItem">The item type</typeparam>
	/// <typeparam name = "TValue">The value item</typeparam>
	/// <param name = "items">The items.</param>
	/// <param name = "selector">The selector.</param>
	/// <returns>The minimum item</returns>
	/// <example>
	/// 	<code>
	/// 		var youngestPerson = persons.MinItem(p =&gt; p.Age);
	/// 	</code>
	/// </example>
	public static TItem MinItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector)
		where TItem : class
		where TValue : IComparable
	{
		TValue minValue;

		return items.MinItem(selector, out minValue);
	}

	///<summary>
	///	Get Distinct
	///</summary>
	///<param name = "source"></param>
	///<param name = "expression"></param>
	///<typeparam name = "T"></typeparam>
	///<typeparam name = "TKey"></typeparam>
	///<returns></returns>
	/// <remarks>
	/// 	Contributed by Michael T, http://about.me/MichaelTran
	/// </remarks>
	public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> expression)
	{
		return source == null ? Enumerable.Empty<T>() : source.GroupBy(expression).Select(i => i.First());
	}

	/// <summary>
	/// Removes matching items from a sequence
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The source.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns></returns>
	/// 
	/// <remarks>
	/// 	Renamed by James Curran, to match corresponding HashSet.RemoveWhere()
	/// 	</remarks>

	public static IEnumerable<T> RemoveWhere<T>(this IEnumerable<T> source, Predicate<T> predicate)
	{
		if (source == null)
			yield break;

		foreach (T t in source)
			if (!predicate(t))
				yield return t;
	}

	///<summary>
	///	Remove item from a list
	///</summary>
	///<param name = "source"></param>
	///<param name = "predicate"></param>
	///<typeparam name = "T"></typeparam>
	///<returns></returns>
	/// <remarks>
	/// 	Contributed by Michael T, http://about.me/MichaelTran
	/// </remarks>
	[Obsolete("Use RemoveWhere instead..")]
	public static IEnumerable<T> RemoveAll<T>(this IEnumerable<T> source, Predicate<T> predicate)
	{
		if (source == null)
			return Enumerable.Empty<T>();

		var list = source.ToList();
		list.RemoveAll(predicate);
		return list;
	}


	///<summary>
	/// Turn the list of objects to a string of Common Seperated Value
	///</summary>
	///<param name="source"></param>
	///<param name="separator"></param>
	///<typeparam name="T"></typeparam>
	///<returns></returns>
	/// <example>
	/// 	<code>
	/// 		var values = new[] { 1, 2, 3, 4, 5 };
	///			string csv = values.ToCSV(';');
	/// 	</code>
	/// </example>
	/// <remarks>
	/// 	Contributed by Moses, http://mosesofegypt.net
	/// </remarks>
	public static string ToCSV<T>(this IEnumerable<T> source, char separator)
	{
		if (source == null)
			return String.Empty;

		var csv = new StringBuilder();
		source.ForEach(value => csv.AppendFormat("{0}{1}", value, separator));
		return csv.ToString(0, csv.Length - 1);
	}

	///<summary>
	/// Turn the list of objects to a string of Common Seperated Value
	///</summary>
	///<param name="source"></param>
	///<typeparam name="T"></typeparam>
	///<returns></returns>
	/// <example>
	/// 	<code>
	/// 		var values = new[] {1, 2, 3, 4, 5};
	///			string csv = values.ToCSV();
	/// 	</code>
	/// </example>
	/// <remarks>
	/// 	Contributed by Moses, http://mosesofegypt.net
	/// </remarks>
	public static string ToCSV<T>(this IEnumerable<T> source)
	{
		return source.ToCSV(',');
	}

	/// <summary>
	/// Overload the Select to allow null as a return
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="source"></param>
	/// <param name="selector"></param>
	/// <param name="allowNull"></param>
	/// <returns>An <see cref="IEnumerable{TResult}"/> using the selector containing null or non-null results based on <see cref="allowNull"/>.</returns>
	/// <example>
	/// <code>
	/// var list = new List{object}{ new object(), null, null };
	/// var noNulls = list.Select(x => x, false);
	/// </code>
	/// </example>
	/// <remarks>
	/// Contributed by thinktech_coder
	/// </remarks>
	public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, bool allowNull = true)
	{
		foreach (var item in source)
		{
			var select = selector(item);
			if (allowNull || !Equals(select, default(TSource)))
				yield return select;
		}
	}

	/// <summary>
	/// Returns true if the <paramref name="source"/> is null or without any items.
	/// </summary>
	public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
	{
		return (source == null || !source.Any());
	}

	/// <summary>
	/// Returns true if the <paramref name="source"/> is contains at least one item.
	/// </summary>
	public static bool IsNotEmpty<T>(this IEnumerable<T> source)
	{
		return !source.IsNullOrEmpty();
	}

	/// <summary>
	/// Returns the first item or the <paramref name="defaultValue"/> if the <paramref name="source"/>
	/// does not contain any item.
	/// </summary>
	public static T FirstOrDefault<T>(this IEnumerable<T> source, T defaultValue)
	{
		return (source.IsNotEmpty() ? source.First() : defaultValue);
	}

	/// <summary>
	///     Appends an element to the end of the current collection and returns the new collection.
	/// </summary>
	/// <typeparam name="T">The enumerable data type</typeparam>
	/// <param name="source">The data values.</param>
	/// <param name="item">The element to append the current collection with.</param>
	/// <returns>
	///     The modified collection.
	/// </returns>
	/// <example>
	///     var integers = Enumerable.Range(0, 3);  // 0, 1, 2
	///     integers = integers.Append(3);          // 0, 1, 2, 3
	/// </example>
	public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
	{
		foreach (var i in source)
			yield return i;

		yield return item;
	}

	/// <summary>
	///     Prepends an element to the start of the current collection and returns the new collection.
	/// </summary>
	/// <typeparam name="T">The enumerable data type</typeparam>
	/// <param name="source">The data values.</param>
	/// <param name="item">The element to prepend the current collection with.</param>
	/// <returns>
	///     The modified collection.
	/// </returns>
	/// <example>
	///     var integers = Enumerable.Range(1, 3);  // 1, 2, 3
	///     integers = integers.Prepend(0);         // 0, 1, 2, 3
	/// </example>
	public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
	{
		yield return item;

		foreach (var i in source)
			yield return i;
	}

	/// <summary>
	///     Creates an Array from an IEnumerable&lt;T&gt; using the specified transform function.
	/// </summary>
	/// <typeparam name="TSource">The source data type</typeparam>
	/// <typeparam name="TResult">The target data type</typeparam>
	/// <param name="source">The source data.</param>
	/// <param name="selector">A transform function to apply to each element.</param>
	/// <returns>An Array of the target data type</returns>
	/// <example>
	///     var integers = Enumerable.Range(1, 3);
	///     var intStrings = values.ToArray(i => i.ToString());
	/// </example>
	/// <remarks>
	///     This method is a shorthand for the frequently use pattern IEnumerable&lt;T&gt;.Select(Func).ToArray()
	/// </remarks>
	public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		return source.Select(selector).ToArray();
	}

	/// <summary>
	///     Creates a List&lt;T&gt; from an IEnumerable&lt;T&gt; using the specified transform function.
	/// </summary>
	/// <typeparam name="TSource">The source data type</typeparam>
	/// <typeparam name="TResult">The target data type</typeparam>
	/// <param name="source">The source data.</param>
	/// <param name="selector">A transform function to apply to each element.</param>
	/// <returns>An IEnumerable&lt;T&gt; of the target data type</returns>
	/// <example>
	///     var integers = Enumerable.Range(1, 3);
	///     var intStrings = values.ToList(i => i.ToString());
	/// </example>
	/// <remarks>
	///     This method is a shorthand for the frequently use pattern IEnumerable&lt;T&gt;.Select(Func).ToList()
	/// </remarks>
	public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		return source.Select(selector).ToList();
	}

	/// <summary>
	/// Computes the sum of a sequence of UInt32 values.
	/// </summary>
	/// <param name="source">A sequence of UInt32 values to calculate the sum of.</param>
	/// <returns>The sum of the values in the sequence.</returns>
	public static uint Sum(this IEnumerable<uint> source)
	{
		return source.Aggregate(0U, (current, number) => current + number);
	}

	/// <summary>
	/// Computes the sum of a sequence of UInt64 values.
	/// </summary>
	/// <param name="source">A sequence of UInt64 values to calculate the sum of.</param>
	/// <returns>The sum of the values in the sequence.</returns>
	public static ulong Sum(this IEnumerable<ulong> source)
	{
		return source.Aggregate(0UL, (current, number) => current + number);
	}

	/// <summary>
	/// Computes the sum of a sequence of nullable UInt32 values.
	/// </summary>
	/// <param name="source">A sequence of nullable UInt32 values to calculate the sum of.</param>
	/// <returns>The sum of the values in the sequence.</returns>
	public static uint? Sum(this IEnumerable<uint?> source)
	{
		return source.Where(nullable => nullable.HasValue).Aggregate(0U, (current, nullable) => current + nullable.GetValueOrDefault());
	}

	/// <summary>
	/// Computes the sum of a sequence of nullable UInt64 values.
	/// </summary>
	/// <param name="source">A sequence of nullable UInt64 values to calculate the sum of.</param>
	/// <returns>The sum of the values in the sequence.</returns>
	public static ulong? Sum(this IEnumerable<ulong?> source)
	{
		return source.Where(nullable => nullable.HasValue).Aggregate(0UL, (current, nullable) => current + nullable.GetValueOrDefault());
	}

	/// <summary>
	/// Computes the sum of a sequence of UInt32 values that are obtained by invoking a transformation function on each element of the intput sequence.
	/// </summary>
	/// <param name="source">A sequence of values that are used to calculate a sum.</param>
	/// <param name="selection">A transformation function to apply to each element.</param>
	/// <returns>The sum of the projected values.</returns>
	public static uint Sum<T>(this IEnumerable<T> source, Func<T, uint> selection)
	{
		return ElementsNotNullFrom(source).Select(selection).Sum();
	}

	private static IEnumerable<T> ElementsNotNullFrom<T>(IEnumerable<T> source)
	{
		return source.Where(x => x.IsNotNull());
	}

	/// <summary>
	/// Computes the sum of a sequence of nullable UInt32 values that are obtained by invoking a transformation function on each element of the intput sequence.
	/// </summary>
	/// <param name="source">A sequence of values that are used to calculate a sum.</param>
	/// <param name="selection">A transformation function to apply to each element.</param>
	/// <returns>The sum of the projected values.</returns>
	public static uint? Sum<T>(this IEnumerable<T> source, Func<T, uint?> selection)
	{
		return ElementsNotNullFrom(source).Select(selection).Sum();
	}

	/// <summary>
	/// Computes the sum of a sequence of UInt64 values that are obtained by invoking a transformation function on each element of the intput sequence.
	/// </summary>
	/// <param name="source">A sequence of values that are used to calculate a sum.</param>
	/// <param name="selector">A transformation function to apply to each element.</param>
	/// <returns>The sum of the projected values.</returns>
	public static ulong Sum<T>(this IEnumerable<T> source, Func<T, ulong> selector)
	{
		return ElementsNotNullFrom(source).Select(selector).Sum();
	}

	/// <summary>
	/// Computes the sum of a sequence of nullable UInt64 values that are obtained by invoking a transformation function on each element of the intput sequence.
	/// </summary>
	/// <param name="source">A sequence of values that are used to calculate a sum.</param>
	/// <param name="selector">A transformation function to apply to each element.</param>
	/// <returns>The sum of the projected values.</returns>
	public static ulong? Sum<T>(this IEnumerable<T> source, Func<T, ulong?> selector)
	{
		return ElementsNotNullFrom(source).Select(selector).Sum();
	}

	/// <summary>
	/// Converts an enumeration of groupings into a Dictionary of those groupings.
	/// </summary>
	/// <typeparam name="TKey">Key type of the grouping and dictionary.</typeparam>
	/// <typeparam name="TValue">Element type of the grouping and dictionary list.</typeparam>
	/// <param name="groupings">The enumeration of groupings from a GroupBy() clause.</param>
	/// <returns>A dictionary of groupings such that the key of the dictionary is TKey type and the value is List of TValue type.</returns>
	public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
	{
		return groupings.ToDictionary(group => group.Key, group => group.ToList());
	}

	/// <summary>
	///   Returns whether the sequence contains a certain amount of elements.
	/// </summary>
	/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
	/// <param name = "source">The source for this extension method.</param>
	/// <param name = "count">The amount of elements the sequence should contain.</param>
	/// <returns>True when the sequence contains the specified amount of elements, false otherwise.</returns>
	public static bool HasCountOf<T>(this IEnumerable<T> source, int count)
	{
		return source.Take(count + 1).Count() == count;
	}

	/// <summary>
	/// Allows you to create Enumerable List of the Enum's Values.
	/// </summary>
	/// <typeparam name="T">Enum Type to enumerate</typeparam>
	/// <returns></returns>
	public static IEnumerable<T> EnumValuesToList<T>(this IEnumerable<T> collection)
	{
		Type enumType = typeof(T);

		// Can't use generic type constraints on value types,
		// so have to do check like this
        // consider using - enumType.IsEnum()
		if (enumType.BaseType != typeof(Enum))
			throw new ArgumentException("T must be of type System.Enum");

		Array enumValArray = Enum.GetValues(enumType);
		var enumValList = new List<T>(enumValArray.Length);
		enumValList.AddRange(from int val in enumValArray select (T)Enum.Parse(enumType, val.ToString()));

		return enumValList;
	}

	/// <summary>
	/// Allows you to create a enumerable string list of the items name in the Enum.
	/// </summary>
	/// <typeparam name="T">Enum Type to enumerate</typeparam>
	/// <returns></returns>
	public static IEnumerable<string> EnumNamesToList<T>(this IEnumerable<T> collection)
	{
		Type cls = typeof(T);

		Type[] enumArrayList = cls.GetInterfaces();

		return (from objType in enumArrayList where objType.IsEnum select objType.Name).ToList();
	}

	/// <summary>
	/// Concatenate a list of items using the provided separator.
	/// </summary>
	/// <param name="items">An enumerable collection of items to concatenate.</param>
	/// <param name="separator">The separator to use for the concatenation (defaults to ",").</param>
	/// <param name="formatString">An optional string formatter for the items in the output list.</param>
	/// <returns>The enumerable collection of items concatenated into a single string.</returns>
	/// <example>
	/// 	<code>
	/// 		List&lt;double&gt; doubles = new List&lt;double&gt;() { 123.4567, 123.4, 123.0, 4, 5 };
	/// 		string concatenated = doubles.ConcatWith(":", "0.00");  // concatenated = 123.46:123.40:123.00:4.00:5.00
	/// 	</code>
	/// </example>
	/// <remarks>
	///     Contributed by Joseph Eddy, http://www.codeplex.com/site/users/view/jceddy
	/// </remarks>
	public static string ConcatWith<T>(this IEnumerable<T> items, string separator = ",", string formatString = "")
	{
		if (items == null) throw new ArgumentNullException("items");
		if (separator == null) throw new ArgumentNullException("separator");

		// shortcut for string enumerable
		if (typeof(T) == typeof(string))
		{
			return string.Join(separator, ((IEnumerable<string>)items).ToArray());
		}

		if (string.IsNullOrEmpty(formatString))
		{
			formatString = "{0}";
		}
		else
		{
			formatString = string.Format("{{0:{0}}}", formatString);
		}

		return string.Join(separator, items.Select(x => string.Format(formatString, x)).ToArray());
	}
}
