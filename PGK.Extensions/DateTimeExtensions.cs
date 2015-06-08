using System;
using System.Globalization;
using System.Text;
using PGK.Extensions.SystemDependencies;

/// <summary>
/// 	Extension methods for the DateTimeOffset data type.
/// </summary>
public static class DateTimeExtensions
{
    const int EveningEnds = 2;
    const int MorningEnds = 12;
    const int AfternoonEnds = 6;
    static readonly DateTime Date1970 = new DateTime(1970, 1, 1);

    ///<summary>
    ///	Return System UTC Offset
    ///</summary>
    public static double UtcOffset
    {
        get { return DateTime.Now.Subtract(DateTime.UtcNow).TotalHours; }
    }

    /// <summary>
    /// 	Calculates the age based on today.
    /// </summary>
    /// <param name = "dateOfBirth">The date of birth.</param>
    /// <returns>The calculated age.</returns>
    public static int CalculateAge(this DateTime dateOfBirth)
    {
        return CalculateAge(dateOfBirth, Clock.Now.Date);
    }

    /// <summary>
    /// 	Calculates the age based on a passed reference date.
    /// </summary>
    /// <param name = "dateOfBirth">The date of birth.</param>
    /// <param name = "referenceDate">The reference date to calculate on.</param>
    /// <returns>The calculated age.</returns>
    public static int CalculateAge(this DateTime dateOfBirth, DateTime referenceDate)
    {
        var years = referenceDate.Year - dateOfBirth.Year;
        if (referenceDate.Month < dateOfBirth.Month || (referenceDate.Month == dateOfBirth.Month && referenceDate.Day < dateOfBirth.Day))
            --years;
        return years;
    }

    /// <summary>
    /// 	Returns the number of days in the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The number of days.</returns>
    public static int GetCountDaysOfMonth(this DateTime date)
    {
        var nextMonth = date.AddMonths(1);
        return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
    }

    /// <summary>
    /// 	Returns the first day of the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The first day of the month</returns>
    public static DateTime GetFirstDayOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    /// <summary>
    /// 	Returns the first day of the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "dayOfWeek">The desired day of week.</param>
    /// <returns>The first day of the month</returns>
    public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
    {
        var dt = date.GetFirstDayOfMonth();
        while (dt.DayOfWeek != dayOfWeek)
            dt = dt.AddDays(1);
        return dt;
    }

    /// <summary>
    /// 	Returns the last day of the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The last day of the month.</returns>
    public static DateTime GetLastDayOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, GetCountDaysOfMonth(date));
    }

    /// <summary>
    /// 	Returns the last day of the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "dayOfWeek">The desired day of week.</param>
    /// <returns>The date time</returns>
    public static DateTime GetLastDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
    {
        var dt = date.GetLastDayOfMonth();
        while (dt.DayOfWeek != dayOfWeek)
            dt = dt.AddDays(-1);
        return dt;
    }

    /// <summary>
    /// 	Indicates whether the date is today.
    /// </summary>
    /// <param name = "dt">The date.</param>
    /// <returns>
    /// 	<c>true</c> if the specified date is today; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsToday(this DateTime dt)
    {
        return (dt.Date == DateTime.Today);
    }

    /// <summary>
    /// 	Sets the time on the specified DateTime value.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "hours">The hours to be set.</param>
    /// <param name = "minutes">The minutes to be set.</param>
    /// <param name = "seconds">The seconds to be set.</param>
    /// <returns>The DateTime including the new time value</returns>
    public static DateTime SetTime(this DateTime date, int hours, int minutes, int seconds)
    {
        return date.SetTime(new TimeSpan(hours, minutes, seconds));
    }

    /// <summary>
    /// 	Sets the time on the specified DateTime value.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name="hours">The hour</param>
    /// <param name="minutes">The minute</param>
    /// <param name="seconds">The second</param>
    /// <param name="milliseconds">The millisecond</param>
    /// <returns>The DateTime including the new time value</returns>
    /// <remarks>Added overload for milliseconds - jtolar</remarks>
    public static DateTime SetTime(this DateTime date, int hours, int minutes, int seconds, int milliseconds)
    {
        return date.SetTime(new TimeSpan(0, hours, minutes, seconds, milliseconds));
    }

    /// <summary>
    /// 	Sets the time on the specified DateTime value.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "time">The TimeSpan to be applied.</param>
    /// <returns>
    /// 	The DateTime including the new time value
    /// </returns>
    public static DateTime SetTime(this DateTime date, TimeSpan time)
    {
        return date.Date.Add(time);
    }

    /// <summary>
    /// 	Converts a DateTime into a DateTimeOffset using the local system time zone.
    /// </summary>
    /// <param name = "localDateTime">The local DateTime.</param>
    /// <returns>The converted DateTimeOffset</returns>
    public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime)
    {
        return localDateTime.ToDateTimeOffset(null);
    }

    /// <summary>
    /// 	Converts a DateTime into a DateTimeOffset using the specified time zone.
    /// </summary>
    /// <param name = "localDateTime">The local DateTime.</param>
    /// <param name = "localTimeZone">The local time zone.</param>
    /// <returns>The converted DateTimeOffset</returns>
    public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime, TimeZoneInfo localTimeZone)
    {
        localTimeZone = (localTimeZone ?? TimeZoneInfo.Local);

        if (localDateTime.Kind != DateTimeKind.Unspecified)
            localDateTime = new DateTime(localDateTime.Ticks, DateTimeKind.Unspecified);

        return TimeZoneInfo.ConvertTimeToUtc(localDateTime, localTimeZone);
    }

    /// <summary>
    /// 	Gets the first day of the week using the current culture.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The first day of the week</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static DateTime GetFirstDayOfWeek(this DateTime date)
    {
        return date.GetFirstDayOfWeek(ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    /// 	Gets the first day of the week using the specified culture.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
    /// <returns>The first day of the week</returns>
    public static DateTime GetFirstDayOfWeek(this DateTime date, CultureInfo cultureInfo)
    {
        cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

        var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        while (date.DayOfWeek != firstDayOfWeek)
            date = date.AddDays(-1);

        return date;
    }

    /// <summary>
    /// 	Gets the last day of the week using the current culture.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The first day of the week</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static DateTime GetLastDayOfWeek(this DateTime date)
    {
        return date.GetLastDayOfWeek(ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    /// 	Gets the last day of the week using the specified culture.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
    /// <returns>The first day of the week</returns>
    public static DateTime GetLastDayOfWeek(this DateTime date, CultureInfo cultureInfo)
    {
        return date.GetFirstDayOfWeek(cultureInfo).AddDays(6);
    }
    
    /// <summary>
    /// 	Gets the next occurence of the specified weekday within the current week using the current culture.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "weekday">The desired weekday.</param>
    /// <returns>The calculated date.</returns>
    /// <example>
    /// 	<code>
    /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
    /// 	</code>
    /// </example>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static DateTime GetWeeksWeekday(this DateTime date, DayOfWeek weekday)
    {
        return date.GetWeeksWeekday(weekday, ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    /// 	Gets the next occurence of the specified weekday within the current week using the specified culture.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "weekday">The desired weekday.</param>
    /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
    /// <returns>The calculated date.</returns>
    /// <example>
    /// 	<code>
    /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
    /// 	</code>
    /// </example>
    public static DateTime GetWeeksWeekday(this DateTime date, DayOfWeek weekday, CultureInfo cultureInfo)
    {
        var firstDayOfWeek = date.GetFirstDayOfWeek(cultureInfo);
        return firstDayOfWeek.GetNextWeekday(weekday);
    }

    /// <summary>
    /// 	Gets the next occurence of the specified weekday.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "weekday">The desired weekday.</param>
    /// <returns>The calculated date.</returns>
    /// <example>
    /// 	<code>
    /// 		var lastMonday = DateTime.Now.GetNextWeekday(DayOfWeek.Monday);
    /// 	</code>
    /// </example>
    public static DateTime GetNextWeekday(this DateTime date, DayOfWeek weekday)
    {
        while (date.DayOfWeek != weekday)
            date = date.AddDays(1);
        return date;
    }

    /// <summary>
    /// 	Gets the previous occurence of the specified weekday.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "weekday">The desired weekday.</param>
    /// <returns>The calculated date.</returns>
    /// <example>
    /// 	<code>
    /// 		var lastMonday = DateTime.Now.GetPreviousWeekday(DayOfWeek.Monday);
    /// 	</code>
    /// </example>
    public static DateTime GetPreviousWeekday(this DateTime date, DayOfWeek weekday)
    {
        while (date.DayOfWeek != weekday)
            date = date.AddDays(-1);
        return date;
    }

    /// <summary>
    /// 	Determines whether the date only part of twi DateTime values are equal.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "dateToCompare">The date to compare with.</param>
    /// <returns>
    /// 	<c>true</c> if both date values are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDateEqual(this DateTime date, DateTime dateToCompare)
    {
        return (date.Date == dateToCompare.Date);
    }

    /// <summary>
    /// 	Determines whether the time only part of two DateTime values are equal.
    /// </summary>
    /// <param name = "time">The time.</param>
    /// <param name = "timeToCompare">The time to compare.</param>
    /// <returns>
    /// 	<c>true</c> if both time values are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsTimeEqual(this DateTime time, DateTime timeToCompare)
    {
        return (time.TimeOfDay == timeToCompare.TimeOfDay);
    }

    /// <summary>
    /// 	Get milliseconds of UNIX area. This is the milliseconds since 1/1/1970
    /// </summary>
    /// <param name = "datetime">Up to which time.</param>
    /// <returns>number of milliseconds.</returns>
    /// <remarks>
    /// 	Contributed by blaumeister, http://www.codeplex.com/site/users/view/blaumeiser
    /// </remarks>
    public static long GetMillisecondsSince1970(this DateTime datetime)
    {
        var ts = datetime.Subtract(Date1970);
        return (long)ts.TotalMilliseconds;
    }

    /// <summary>
    /// Get milliseconds of UNIX area. This is the milliseconds since 1/1/1970
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    /// <remarks>This is the same as GetMillisecondsSince1970 but more descriptive</remarks>
    public static long ToUnixEpoch(this DateTime dateTime)
    {
        return GetMillisecondsSince1970(dateTime);
    }

    /// <summary>
    /// 	Indicates whether the specified date is a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>
    /// 	<c>true</c> if the specified date is a weekend; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek.EqualsAny(DayOfWeek.Saturday, DayOfWeek.Sunday);
    }

    /// <summary>
    /// 	Adds the specified amount of weeks (=7 days gregorian calendar) to the passed date value.
    /// </summary>
    /// <param name = "date">The origin date.</param>
    /// <param name = "value">The amount of weeks to be added.</param>
    /// <returns>The enw date value</returns>
    public static DateTime AddWeeks(this DateTime date, int value)
    {
        return date.AddDays(value * 7);
    }

    ///<summary>
    ///	Get the number of days within that year.
    ///</summary>
    ///<param name = "year">The year.</param>
    ///<returns>the number of days within that year</returns>
    /// <remarks>
    /// 	Contributed by Michael T, http://about.me/MichaelTran
    ///     Modified by JTolar to implement Culture Settings
    /// </remarks>
    public static int GetDays(int year)
    {
        return GetDays(year, ExtensionMethodSetting.DefaultCulture);
    }

    ///<summary>
    ///	Get the number of days within that year. Uses the culture specified.
    ///</summary>
    ///<param name = "year">The year.</param>
    ///<param name="culture">Specific culture</param>
    ///<returns>the number of days within that year</returns>
    /// <remarks>
    /// 	Contributed by Michael T, http://about.me/MichaelTran
    ///     Modified by JTolar to implement Culture Settings
    /// </remarks>
    public static int GetDays(int year, CultureInfo culture)
    {
        var first = new DateTime(year, 1, 1, culture.Calendar);
        var last = new DateTime(year + 1, 1, 1, culture.Calendar);
        return GetDays(first, last);
    }


    ///<summary>
    ///	Get the number of days within that date year. Allows user to specify culture.
    ///</summary>
    ///<param name = "date">The date.</param>
    ///<param name="culture">Specific culture</param>
    ///<returns>the number of days within that year</returns>
    /// <remarks>
    /// 	Contributed by Michael T, http://about.me/MichaelTran
    ///     Modified by JTolar to implement Culture Settings 
    /// </remarks>
    public static int GetDays(this DateTime date)
    {
        return GetDays(date.Year, ExtensionMethodSetting.DefaultCulture);
    }

    ///<summary>
    ///	Get the number of days within that date year. Allows user to specify culture
    ///</summary>
    ///<param name = "date">The date.</param>
    ///<param name="culture">Specific culture</param>
    ///<returns>the number of days within that year</returns>
    /// <remarks>
    /// 	Contributed by Michael T, http://about.me/MichaelTran
    ///     Modified by JTolar to implement Culture Settings 
    /// </remarks>
    public static int GetDays(this DateTime date, CultureInfo culture)
    {
        return GetDays(date.Year, culture);
    }

    ///<summary>
    ///	Get the number of days between two dates.
    ///</summary>
    ///<param name = "fromDate">The origin year.</param>
    ///<param name = "toDate">To year</param>
    ///<returns>The number of days between the two years</returns>
    /// <remarks>
    /// 	Contributed by Michael T, http://about.me/MichaelTran
    /// </remarks>
    public static int GetDays(this DateTime fromDate, DateTime toDate)
    {
        return Convert.ToInt32(toDate.Subtract(fromDate).TotalDays);
    }

    ///<summary>
    ///	Return a period "Morning", "Afternoon", or "Evening"
    ///</summary>
    ///<param name = "date">The date.</param>
    ///<returns>The period "morning", "afternoon", or "evening"</returns>
    /// <remarks>
    /// 	Contributed by Michael T, http://about.me/MichaelTran
    /// </remarks>
    public static string GetPeriodOfDay(this DateTime date)
    {
        var hour = date.Hour;
        if (hour < EveningEnds)
            return "evening";
        if (hour < MorningEnds)
            return "morning";
        return hour < AfternoonEnds ? "afternoon" : "evening";
    }

    /// <summary>
    /// Gets the week number for a provided date time value based on a specific culture.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="culture">Specific culture</param>
    /// <returns>The week number</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static int GetWeekOfYear(this DateTime dateTime, CultureInfo culture)
    {
        var calendar = culture.Calendar;
        var dateTimeFormat = culture.DateTimeFormat;

        return calendar.GetWeekOfYear(dateTime, dateTimeFormat.CalendarWeekRule, dateTimeFormat.FirstDayOfWeek);
    }

    /// <summary>
    /// Gets the week number for a provided date time value based on the current culture settings. 
    /// Uses DefaultCulture from ExtensionMethodSetting
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns>The week number</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static int GetWeekOfYear(this DateTime dateTime)
    {
        return GetWeekOfYear(dateTime, ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    ///     Indicates whether the specified date is Easter in the Christian calendar.
    /// </summary>
    /// <param name="date">Instance value.</param>
    /// <returns>True if the instance value is a valid Easter Date.</returns>
    public static bool IsEaster(this DateTime date)
    {
        int Y = date.Year;
        int a = Y % 19;
        int b = Y / 100;
        int c = Y % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int L = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * L) / 451;
        int Month = (h + L - 7 * m + 114) / 31;
        int Day = ((h + L - 7 * m + 114) % 31) + 1;

        DateTime dtEasterSunday = new DateTime(Y, Month, Day);

        return date == dtEasterSunday;
    }

    /// <summary>
    ///     Indicates whether the source DateTime is before the supplied DateTime.
    /// </summary>
    /// <param name="source">The source DateTime.</param>
    /// <param name="other">The compared DateTime.</param>
    /// <returns>True if the source is before the other DateTime, False otherwise</returns>
    public static bool IsBefore(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    ///     Indicates whether the source DateTime is before the supplied DateTime.
    /// </summary>
    /// <param name="source">The source DateTime.</param>
    /// <param name="other">The compared DateTime.</param>
    /// <returns>True if the source is before the other DateTime, False otherwise</returns>
    public static bool IsAfter(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) > 0;
    }

    /// <summary>
    /// Gets a DateTime representing Next Day
    /// </summary>
    /// <param name="date">The current day</param>
    /// <returns></returns>
    public static DateTime Tomorrow(this DateTime date)
    {
        return date.AddDays(1);
    }

    /// <summary>
    /// Gets a DateTime representing Previous Day
    /// </summary>
    /// <param name="date">The current day</param>
    /// <returns></returns>
    public static DateTime Yesterday(this DateTime date)
    {
        return date.AddDays(-1);
    }

    /// <summary>
    /// The ToFriendlyString() method represents dates in a user friendly way. 
    /// For example, when displaying a news article on a webpage, you might want 
    /// articles that were published one day ago to have their publish dates 
    /// represented as "yesterday at 12:30 PM". Or if the article was publish today, 
    /// show the date as "Today, 3:33 PM".
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="culture">Specific Culture</param>
    /// <returns>string</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>/// <remarks></remarks>
    public static string ToFriendlyDateString(this DateTime date, CultureInfo culture)
    {
        var sbFormattedDate = new StringBuilder();
        if (date.Date == DateTime.Today)
        {
            sbFormattedDate.Append("Today");
        }
        else if (date.Date == DateTime.Today.AddDays(-1))
        {
            sbFormattedDate.Append("Yesterday");
        }
        else if (date.Date > DateTime.Today.AddDays(-6))
        {
            // *** Show the Day of the week
            sbFormattedDate.Append(date.ToString("dddd").ToString(culture));
        }
        else
        {
            sbFormattedDate.Append(date.ToString("MMMM dd, yyyy").ToString(culture));
        }

        //append the time portion to the output
        sbFormattedDate.Append(" at ").Append(date.ToString("t").ToLower());
        return sbFormattedDate.ToString();
    }
    
    ///<summary>
    /// The ToFriendlyString() method represents dates in a user friendly way. 
    /// For example, when displaying a news article on a webpage, you might want 
    /// articles that were published one day ago to have their publish dates 
    /// represented as "yesterday at 12:30 PM". Or if the article was publish today, 
    /// show the date as "Today, 3:33 PM". Uses DefaultCulture from ExtensionMethodSetting.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns>string</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>/// <remarks></remarks>
    public static string ToFriendlyDateString(this DateTime date)
    {
        return ToFriendlyDateString(date, ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    /// Returns the date at 23:59.59.999 for the specified DateTime
    /// </summary>
    /// <param name="date">The DateTime to be processed</param>
    /// <returns>The date at 23:50.59.999</returns>
    public static DateTime EndOfDay(this DateTime date)
    {
        return date.SetTime(23, 59, 59, 999);
    }

    /// <summary>
    /// Returns the date at 12:00:00 for the specified DateTime
    /// </summary>
    /// <param name="time">The current date</param>
    public static DateTime Noon(this DateTime time)
    {
        return time.SetTime(12, 0, 0);
    }

    /// <summary>
    /// Returns the date at 00:00:00 for the specified DateTime
    /// </summary>
    /// <param name="time">The current date</param>
    public static DateTime Midnight(this DateTime time)
    {
        return time.SetTime(0, 0, 0, 0);
    }

    /// <summary>
    /// Returns whether the DateTime falls on a weekday
    /// </summary>
    /// <param name="date">The date to be processed</param>
    /// <returns>Whether the specified date occurs on a weekday</returns>
    public static bool IsWeekDay(this DateTime date)
    {
        return !date.IsWeekend();
    }

   
}
