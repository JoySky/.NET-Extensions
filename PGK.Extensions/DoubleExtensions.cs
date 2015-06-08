using System;

/// <summary>
/// 	Extension methods for the Double data type
/// </summary>
public static class DoubleExtensions
{
	/// <summary>Checks whether the value is in range</summary>
	/// <param name="value">The Value</param>
	/// <param name="minValue">The minimum value</param>
	/// <param name="maxValue">The maximum value</param>
	public static bool InRange(this double value, double minValue, double maxValue)
	{
		return (value >= minValue && value <= maxValue);
	}

	/// <summary>Checks whether the value is in range or returns the default value</summary>
	/// <param name="value">The Value</param>
	/// <param name="minValue">The minimum value</param>
	/// <param name="maxValue">The maximum value</param>
	/// <param name="defaultValue">The default value</param>
	public static double InRange(this double value, double minValue, double maxValue, double defaultValue)
	{
		return value.InRange(minValue, maxValue) ? value : defaultValue;
	}

	/// <summary>
	/// Gets a TimeSpan from a double number of days.
	/// </summary>
	/// <param name="days">The number of days the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of days.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Days(this double days)
	{
		return TimeSpan.FromDays(days);
	}

	/// <summary>
	/// Gets a TimeSpan from a double number of hours.
	/// </summary>
	/// <param name="days">The number of hours the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of hours.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Hours(this double hours)
	{
		return TimeSpan.FromHours(hours);
	}

	/// <summary>
	/// Gets a TimeSpan from a double number of milliseconds.
	/// </summary>
	/// <param name="days">The number of milliseconds the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of milliseconds.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Milliseconds(this double milliseconds)
	{
		return TimeSpan.FromMilliseconds(milliseconds);
	}

	/// <summary>
	/// Gets a TimeSpan from a double number of minutes.
	/// </summary>
	/// <param name="days">The number of minutes the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of minutes.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Minutes(this double minutes)
	{
		return TimeSpan.FromMinutes(minutes);
	}

	/// <summary>
	/// Gets a TimeSpan from a double number of seconds.
	/// </summary>
	/// <param name="days">The number of seconds the TimeSpan will contain.</param>
	/// <returns>A TimeSpan containing the specified number of seconds.</returns>
	/// <remarks>
	///		Contributed by jceddy
	/// </remarks>
	public static TimeSpan Seconds(this double seconds)
	{
		return TimeSpan.FromSeconds(seconds);
	}
}