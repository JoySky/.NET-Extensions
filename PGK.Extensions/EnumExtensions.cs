using System;
using System.Linq;
using System.Reflection;

/// <summary>
/// Extension methods for the enum data type
/// </summary>
public static class EnumExtensions
{
	/// <summary>
	/// Removes a flag and returns the new value
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="variable">Source enum</param>
	/// <param name="flag">Dumped flag</param>
	/// <returns>Result enum value</returns>
	/// <remarks>
	/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
	/// </remarks>
	public static T ClearFlag<T>(this Enum variable, T flag)
	{
		return ClearFlags(variable, flag);
	}

	/// <summary>
	/// Removes flags and returns the new value
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="variable">Source enum</param>
	/// <param name="flags">Dumped flags</param>
	/// <returns>Result enum value</returns>
	/// <remarks>
	/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
	/// </remarks>
	public static T ClearFlags<T>(this Enum variable, params T[] flags)
	{
		var result = Convert.ToUInt64(variable);
		foreach (T flag in flags)
			result &= ~Convert.ToUInt64(flag);
		return (T)Enum.Parse(variable.GetType(), result.ToString());
	}

	/// <summary>
	/// Includes a flag and returns the new value
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="variable">Source enum</param>
	/// <param name="flag">Established flag</param>
	/// <returns>Result enum value</returns>
	/// <remarks>
	/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
	/// </remarks>
	public static T SetFlag<T>(this Enum variable, T flag)
	{
		return SetFlags(variable, flag);
	}

	/// <summary>
	/// Includes flags and returns the new value
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="variable">Source enum</param>
	/// <param name="flags">Established flags</param>
	/// <returns>Result enum value</returns>
	/// <remarks>
	/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
	/// </remarks>
	public static T SetFlags<T>(this Enum variable, params T[] flags)
	{
		var result = Convert.ToUInt64(variable);
		foreach (T flag in flags)
			result |= Convert.ToUInt64(flag);
		return (T)Enum.Parse(variable.GetType(), result.ToString());
	}

	/// <summary>
	/// Check to see if enumeration has a specific flag
	/// </summary>
	/// <param name="variable">Enumeration to check</param>
	/// <param name="flag">Flag to check for</param>
	/// <returns>Result of check</returns>
	/// <remarks>
	/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
	/// </remarks>
	/*  This will never be called. Enum.HasFlag is a native method of Enum
	public static bool HasFlag<E>(this E variable, E flag)
		where E : struct, IComparable, IFormattable, IConvertible
	{
		return HasFlags(variable, flag);
	}
	 */

	/// <summary>
	/// Check to see if enumeration has a specific flag set
	/// </summary>
	/// <param name="variable">Enumeration to check</param>
	/// <param name="flags">Flags to check for</param>
	/// <returns>Result of check</returns>
	/// <remarks>
	/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
	/// </remarks>
	public static bool HasFlags<E>(this E variable, params E[] flags)
		where E : struct, IComparable, IFormattable, IConvertible
	{
		if (!typeof(E).IsEnum)
			throw new ArgumentException("variable must be an Enum", "variable");

		foreach (var flag in flags)
		{
			if (!Enum.IsDefined(typeof(E), flag))
				return false;

			ulong numFlag = Convert.ToUInt64(flag);
			if ((Convert.ToUInt64(variable) & numFlag) != numFlag)
				return false;
		}

		return true;
	}

	/// <summary>
	/// Description, specified by attribute <c>DisplayStringAttribute</c>.
	/// <para>If the attribute is not specified, returns the default name obtained by the method <c>ToString()</c></para>
	/// </summary>
	/// <param name="value"></param>
	/// <returns>
	/// Returns the description given by the attribute <c>DisplayStringAttribute</c>. 
	/// <para>If the attribute is not specified, returns the default name obtained by the method <c>ToString()</c></para>
	/// </returns>
	/// <see cref="DisplayStringAttribute"/>
	/// <example>
	///     <code>
	///         enum OperatingSystem
	///         {
	///            [DisplayString("MS-DOS")]
	///            Msdos,
	///         
	///            [DisplayString("Windows 98")]
	///            Win98,
	///         
	///            [DisplayString("Windows XP")]
	///            Xp,
	///         
	///            [DisplayString("Windows Vista")]
	///            Vista,
	///         
	///            [DisplayString("Windows 7")]
	///            Seven,
	///         }
	///         
	///         public string GetMyOSName()
	///         {
	///             var myOS = OperatingSystem.Seven;
	///             return myOS.DisplayString();
	///         }
	///     </code>
	/// </example>
	/// <remarks>
	/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
	/// </remarks>
	public static string DisplayString(this Enum value)
	{
		FieldInfo info = value.GetType().GetField(value.ToString());
		var attributes = (DisplayStringAttribute[])info.GetCustomAttributes(typeof(DisplayStringAttribute), false);
		return attributes.Length >= 1 ? attributes[0].DisplayString : value.ToString();
	}
}