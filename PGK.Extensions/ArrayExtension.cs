using System;
using System.Collections.Generic;

/// <summary>
/// 	Extension methods for the array data type
/// </summary>
public static class ArrayExtension
{

	///<summary>
	///	Check if the array is null or empty
	///</summary>
	///<param name = "source"></param>
	///<returns></returns>
	public static bool IsNullOrEmpty(this Array source)
	{
		return source != null ? source.Length <= 0 : false;
	}

	///<summary>
	///	Check if the index is within the array
	///</summary>
	///<param name = "source"></param>
	///<param name = "index"></param>
	///<returns></returns>
	/// <remarks>
	/// 	Contributed by Michael T, http://about.me/MichaelTran
	/// </remarks>
	public static bool WithinIndex(this Array source, int index)
	{
		return source != null && index >= 0 && index < source.Length;
	}

	///<summary>
	///	Check if the index is within the array
	///</summary>
	///<param name = "source"></param>
	///<param name = "index"></param>
	///<param name="dimension"></param>
	///<returns></returns>
	/// <remarks>
	/// 	Contributed by Michael T, http://about.me/MichaelTran
	/// </remarks>
	public static bool WithinIndex(this Array source, int index, int dimension = 0)
	{
		return source != null && index >= source.GetLowerBound(dimension) && index <= source.GetUpperBound(dimension);
	}


	/// <summary>
	/// Combine two arrays into one.
	/// </summary>
	/// <typeparam name="T">Type of Array</typeparam>
	/// <param name="combineWith">Base array in which arrayToCombine will add.</param>
	/// <param name="arrayToCombine">Array to combine with Base array.</param>
	/// <returns></returns>
	/// <example>
	/// 	<code>
	/// 		int[] arrayOne = new[] { 1, 2, 3, 4 };
	/// 		int[] arrayTwo = new[] { 5, 6, 7, 8 };
	/// 		Array combinedArray = arrayOne.CombineArray<int>(arrayTwo);
	/// 	</code>
	/// </example>
	/// <remarks>
	/// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
	/// </remarks>
	public static T[] CombineArray<T>(this T[] combineWith, T[] arrayToCombine)
	{
		if (combineWith != default(T[]) && arrayToCombine != default(T[]))
		{
			int initialSize = combineWith.Length;
			Array.Resize<T>(ref combineWith, initialSize + arrayToCombine.Length);
			Array.Copy(arrayToCombine, arrayToCombine.GetLowerBound(0), combineWith, initialSize, arrayToCombine.Length);
		}
		return combineWith;
	}

	/// <summary>
	/// To clear the contents of the array.
	/// </summary>
	/// <param name="clear"> The array to clear</param>
	/// <returns>Cleared array</returns>
	/// <example>
	///     <code>
	///         Array array = Array.CreateInstance(typeof(string), 2);
	///         array.SetValue("One", 0); array.SetValue("Two", 1);
	///         Array arrayToClear = array.ClearAll();
	///     </code>
	/// </example>
	/// <remarks>
	/// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
	/// </remarks>
	public static Array ClearAll(this Array clear)
	{
		if (clear != null)
			Array.Clear(clear, 0, clear.Length);
		return clear;
	}

	/// <summary>
	/// To clear the contents of the array.
	/// </summary>
	/// <typeparam name="T">The type of array</typeparam>
	/// <param name="clear"> The array to clear</param>
	/// <returns>Cleared array</returns>
	/// <example>
	///     <code>
	///         int[] result = new[] { 1, 2, 3, 4 }.ClearAll<int>();
	///     </code>
	/// </example>
	/// <remarks>
	/// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
	/// </remarks>
	public static T[] ClearAll<T>(this T[] arrayToClear)
	{
		if (arrayToClear != null)
			for (int i = arrayToClear.GetLowerBound(0); i <= arrayToClear.GetUpperBound(0); ++i)
				arrayToClear[i] = default(T);
		return arrayToClear;
	}

	/// <summary>
	/// To clear a specific item in the array.
	/// </summary>
	/// <param name="arrayToClear">The array in where to clean the item.</param>
	/// <param name="at">Which element to clear.</param>
	/// <returns></returns>
	/// <example>
	///     <code>
	///         Array array = Array.CreateInstance(typeof(string), 2);
	///         array.SetValue("One", 0); array.SetValue("Two", 1);
	///         Array result = array.ClearAt(2);
	///     </code>
	/// </example>
	/// <remarks>
	/// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
	/// </remarks>
	public static Array ClearAt(this Array arrayToClear, int at)
	{
		if (arrayToClear != null)
		{
			int arrayIndex = at.GetArrayIndex();
			if (arrayIndex.IsIndexInArray(arrayToClear))
				Array.Clear(arrayToClear, arrayIndex, 1);
		}
		return arrayToClear;
	}

	/// <summary>
	/// To clear a specific item in the array.
	/// </summary>
	/// <typeparam name="T">The type of array</typeparam>
	/// <param name="arrayToClear">Array to clear.</param>
	/// <param name="at">Which element to clear.</param>
	/// <returns></returns>
	/// <example>
	///     <code>
	///           string[] clearString = new[] { "A" }.ClearAt<string>(0);
	///     </code>
	/// </example>
	/// <remarks>
	/// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
	/// </remarks>
	public static T[] ClearAt<T>(this T[] arrayToClear, int at)
	{
		if (arrayToClear != null)
		{
			int arrayIndex = at.GetArrayIndex();
			if (arrayIndex.IsIndexInArray(arrayToClear))
				arrayToClear[arrayIndex] = default(T);
		}
		return arrayToClear;
	}

    /// <summary>
    /// Tests if the array is empty.
    /// </summary>
    /// <param name="array">The array to test.</param>
    /// <returns>True if the array is empty.</returns>
    public static bool IsEmpty(this Array array)
    {
        array.ExceptionIfNullOrEmpty(
            "The array cannot be null.",
            "array");

        return array.Length == 0;
    }

    #region BlockCopy

    /// <summary>
    /// Returns a block of items from an array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <remarks>Contributed by Chris Gessler</remarks>
    public static T[] BlockCopy<T>(this T[] array, int index, int length)
    {
        return BlockCopy(array, index, length, false);
    }

    /// <summary>
    /// Returns a block of items from an array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <param name="padToLength"></param>
    /// <returns></returns>
    /// <remarks>
    /// Test results prove that Array.Copy is many times faster than Skip/Take and LINQ
    /// Item count: 1,000,000
    /// Array.Copy:     15 ms 
    /// Skip/Take:  42,464 ms - 42.5 seconds
    /// LINQ:          881 ms
    /// 
    /// Contributed by Chris Gessler</remarks>
    public static T[] BlockCopy<T>(this T[] array, int index, int length, bool padToLength)
    {
        if (array == null) throw new NullReferenceException();

        int n = length;
        T[] b = null;

        if (array.Length < index + length)
        {
            n = array.Length - index;
            if (padToLength)
            {
                b = new T[length];
            }
        }

        if (b == null) b = new T[n];
        Array.Copy(array, index, b, 0, n);
        return b;
    }

    /// <summary>
    /// Allows enumeration over an Array in blocks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="count"></param>
    /// <param name="padToLength"></param>
    /// <returns></returns>
    /// <remarks>Contributed by Chris Gessler</remarks>
    public static IEnumerable<T[]> BlockCopy<T>(this T[] array, int count, bool padToLength = false)
    {
        for (int i = 0; i < array.Length; i += count)
            yield return array.BlockCopy(i, count, padToLength);
    }

    #endregion
}
