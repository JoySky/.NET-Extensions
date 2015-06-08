using System;

/// <summary>
/// 	Extension methods for the DateTimeOffset data type.
/// </summary>
public static class DateTimeOffsetExtensions
{
	/// <summary>
	/// 	Indicates whether the date is today.
	/// </summary>
	/// <param name = "dto">The date.</param>
	/// <returns>
	/// 	<c>true</c> if the specified date is today; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsToday(this DateTimeOffset dto)
	{
		return (dto.Date.IsToday());
	}

	/// <summary>
	/// 	Sets the time on the specified DateTimeOffset value using the local system time zone.
	/// </summary>
	/// <param name = "date">The base date.</param>
	/// <param name = "hours">The hours to be set.</param>
	/// <param name = "minutes">The minutes to be set.</param>
	/// <param name = "seconds">The seconds to be set.</param>
	/// <returns>The DateTimeOffset including the new time value</returns>
	public static DateTimeOffset SetTime(this DateTimeOffset date, int hours, int minutes, int seconds)
	{
		return date.SetTime(new TimeSpan(hours, minutes, seconds));
	}

	/// <summary>
	/// 	Sets the time on the specified DateTime value using the local system time zone.
	/// </summary>
	/// <param name = "date">The base date.</param>
	/// <param name = "time">The TimeSpan to be applied.</param>
	/// <returns>
	/// 	The DateTimeOffset including the new time value
	/// </returns>
	public static DateTimeOffset SetTime(this DateTimeOffset date, TimeSpan time)
	{
		return date.SetTime(time, null);
	}

	/// <summary>
	/// 	Sets the time on the specified DateTime value using the specified time zone.
	/// </summary>
	/// <param name = "date">The base date.</param>
	/// <param name = "time">The TimeSpan to be applied.</param>
	/// <param name = "localTimeZone">The local time zone.</param>
	/// <returns>/// The DateTimeOffset including the new time value/// </returns>
	public static DateTimeOffset SetTime(this DateTimeOffset date, TimeSpan time, TimeZoneInfo localTimeZone)
	{
		var localDate = date.ToLocalDateTime(localTimeZone);
		localDate.SetTime(time);
		return localDate.ToDateTimeOffset(localTimeZone);
	}

	/// <summary>
	/// 	Converts a DateTimeOffset into a DateTime using the local system time zone.
	/// </summary>
	/// <param name = "dateTimeUtc">The base DateTimeOffset.</param>
	/// <returns>The converted DateTime</returns>
	public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc)
	{
		return dateTimeUtc.ToLocalDateTime(null);
	}

	/// <summary>
	/// 	Converts a DateTimeOffset into a DateTime using the specified time zone.
	/// </summary>
	/// <param name = "dateTimeUtc">The base DateTimeOffset.</param>
	/// <param name = "localTimeZone">The time zone to be used for conversion.</param>
	/// <returns>The converted DateTime</returns>
	public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc, TimeZoneInfo localTimeZone)
	{
		localTimeZone = (localTimeZone ?? TimeZoneInfo.Local);

		return TimeZoneInfo.ConvertTime(dateTimeUtc, localTimeZone).DateTime;
	}
}
