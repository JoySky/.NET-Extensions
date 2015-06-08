using System;

/// <summary>
/// 	Extension methods for the Long data type
/// </summary>
public static class LongExtensions
{
	/// <summary>
	/// 	Performs the specified action n times based on the underlying long value.
	/// </summary>
	/// <param name = "value">The value.</param>
	/// <param name = "action">The action.</param>
	public static void Times(this long value, Action action)
	{
		for (var i = 0; i < value; i++)
			action();
	}

	/// <summary>
	/// 	Performs the specified action n times based on the underlying long value.
	/// </summary>
	/// <param name = "value">The value.</param>
	/// <param name = "action">The action.</param>
	public static void Times(this long value, Action<long> action)
	{
		for (var i = 0; i < value; i++)
			action(i);
	}

	/// <summary>
	/// 	Determines whether the value is even
	/// </summary>
	/// <param name = "value">The Value</param>
	/// <returns>true or false</returns>
	public static bool IsEven(this long value)
	{
		return value%2 == 0;
	}

	/// <summary>
	/// 	Determines whether the value is odd
	/// </summary>
	/// <param name = "value">The Value</param>
	/// <returns>true or false</returns>
	public static bool IsOdd(this long value)
	{
		return value % 2 != 0;
	}

	/// <summary>Checks whether the value is in range</summary>
	/// <param name="value">The Value</param>
	/// <param name="minValue">The minimum value</param>
	/// <param name="maxValue">The maximum value</param>
	public static bool InRange(this long value, long minValue, long maxValue)
	{
		return (value >= minValue && value <= maxValue);
	}

	/// <summary>Checks whether the value is in range or returns the default value</summary>
	/// <param name="value">The Value</param>
	/// <param name="minValue">The minimum value</param>
	/// <param name="maxValue">The maximum value</param>
	/// <param name="defaultValue">The default value</param>
	public static long InRange(this long value, long minValue, long maxValue, long defaultValue)
	{
		return value.InRange(minValue, maxValue) ? value : defaultValue;
	}

	/// <summary>
	/// A prime number (or a prime) is a natural number that has exactly two distinct natural number divisors: 1 and itself.
	/// </summary>
	/// <param name="candidate">Object value</param>
	/// <returns>Returns true if the value is a prime number.</returns>
	public static bool IsPrime(this long candidate)
	{
		if ((candidate & 1) == 0)
		{
			if (candidate == 2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		for (long i = 3; (i * i) <= candidate; i += 2)
		{
			if ((candidate % i) == 0)
			{
				return false;
			}
		}

		return candidate != 1;
	}

	/// <summary>
	/// Converts the value to ordinal string. (English)
	/// </summary>
	/// <param name="i">Object value</param>
	/// <returns>Returns string containing ordinal indicator adjacent to a numeral denoting. (English)</returns>
	public static string ToOrdinal(this long i)
	{
		string suffix = "th";
		switch (i % 100)
		{
			case 11:
			case 12:
			case 13:
				break;
			default:
				switch (i % 10)
				{
					case 1:
						suffix = "st";
						break;
					case 2:
						suffix = "nd";
						break;
					case 3:
						suffix = "rd";
						break;
				}
				break;
		}

		return string.Format("{0}{1}", i, suffix);
	}

	/// <summary>
	/// Converts the value to ordinal string with specified format. (English)
	/// </summary>
	/// <param name="i">Object value</param>
	/// <param name="format">A standard or custom format string that is supported by the object to be formatted.</param>
	/// <returns>Returns string containing ordinal indicator adjacent to a numeral denoting. (English)</returns>
	public static string ToOrdinal(this long i, string format)
	{
		return string.Format(format, i.ToOrdinal());
	}

	/// <summary>
	/// Gets a TimeSpan from a long number of days.
	/// </summary>
	/// <param name="days">The number of days the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of days.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Days(this long days)
	{
		return TimeSpan.FromDays(days);
	}

	/// <summary>
	/// Gets a TimeSpan from a long number of hours.
	/// </summary>
	/// <param name="days">The number of hours the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of hours.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Hours(this long hours)
	{
		return TimeSpan.FromHours(hours);
	}

	/// <summary>
	/// Gets a TimeSpan from a long number of milliseconds.
	/// </summary>
	/// <param name="days">The number of milliseconds the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of milliseconds.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Milliseconds(this long milliseconds)
	{
		return TimeSpan.FromMilliseconds(milliseconds);
	}

	/// <summary>
	/// Gets a TimeSpan from a long number of minutes.
	/// </summary>
	/// <param name="days">The number of minutes the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of minutes.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Minutes(this long minutes)
	{
		return TimeSpan.FromMinutes(minutes);
	}

	/// <summary>
	/// Gets a TimeSpan from a long number of seconds.
	/// </summary>
	/// <param name="days">The number of seconds the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of seconds.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Seconds(this long seconds)
	{
		return TimeSpan.FromSeconds(seconds);
	}

	/// <summary>
	/// Gets a TimeSpan from a long number of ticks.
	/// </summary>
	/// <param name="days">The number of ticks the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of ticks.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Ticks(this long ticks)
	{
		return TimeSpan.FromTicks(ticks);
	}
}