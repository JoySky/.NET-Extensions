using System;
using System.Data;

/// <summary>
/// 	Extension methods for all kind of ADO.NET DataReaders (SqlDataReader, OracleDataReader, ...)
/// </summary>
public static class DataReaderExtensions {
    /// <summary>
    /// 	Gets the record value casted to the specified data type or the data types default value.
    /// </summary>
    /// <typeparam name = "T">The return data type</typeparam>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static T Get<T>(this IDataReader reader, string field) {
        return reader.Get(field, default(T));
    }

    /// <summary>
    /// 	Gets the record value casted to the specified data type or the specified default value.
    /// </summary>
    /// <typeparam name = "T">The return data type</typeparam>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static T Get<T>(this IDataReader reader, string field, T defaultValue) {
        var value = reader[field];
        if (value == DBNull.Value)
            return defaultValue;

        if (value is T)
            return (T)value;

        return value.ConvertTo(defaultValue);
    }

    /// <summary>
    /// 	Gets the record value casted as byte array.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static byte[] GetBytes(this IDataReader reader, string field) {
        return (reader[field] as byte[]);
    }

    /// <summary>
    /// 	Gets the record value casted as string or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static string GetString(this IDataReader reader, string field) {
        return reader.GetString(field, null);
    }

    /// <summary>
    /// 	Gets the record value casted as string or the specified default value.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static string GetString(this IDataReader reader, string field, string defaultValue) {
        var value = reader[field];
        return (value is string ? (string)value : defaultValue);
    }

    /// <summary>
    /// 	Gets the record value casted as Guid or Guid.Empty.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static Guid GetGuid(this IDataReader reader, string field) {
        var value = reader[field];
        return (value is Guid ? (Guid)value : Guid.Empty);
    }

    /// <summary>
    /// 	Gets the record value casted as Guid? or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static Guid? GetNullableGuid(this IDataReader reader, string field) {
        var value = reader[field];
        return (value is Guid ? (Guid?)value : null);
    }

    /// <summary>
    /// 	Gets the record value casted as DateTime or DateTime.MinValue.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTime GetDateTime(this IDataReader reader, string field) {
        return reader.GetDateTime(field, DateTime.MinValue);
    }

    /// <summary>
    /// 	Gets the record value casted as DateTime or the specified default value.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static DateTime GetDateTime(this IDataReader reader, string field, DateTime defaultValue) {
        var value = reader[field];
        return (value is DateTime ? (DateTime)value : defaultValue);
    }

    /// <summary>
    /// 	Gets the record value casted as DateTime or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTime? GetNullableDateTime(this IDataReader reader, string field) {
        var value = reader[field];
        return (value is DateTime ? (DateTime?)value : null);
    }

    /// <summary>
    /// 	Gets the record value casted as DateTimeOffset (UTC) or DateTime.MinValue.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTimeOffset GetDateTimeOffset(this IDataReader reader, string field) {
        return new DateTimeOffset(reader.GetDateTime(field), TimeSpan.Zero);
    }

    /// <summary>
    /// 	Gets the record value casted as DateTimeOffset (UTC) or the specified default value.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static DateTimeOffset GetDateTimeOffset(this IDataReader reader, string field, DateTimeOffset defaultValue) {
        var dt = reader.GetDateTime(field);
        return (dt != DateTime.MinValue ? new DateTimeOffset(dt, TimeSpan.Zero) : defaultValue);
    }

    /// <summary>
    /// 	Gets the record value casted as DateTimeOffset (UTC) or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTimeOffset? GetNullableDateTimeOffset(this IDataReader reader, string field) {
        var dt = reader.GetNullableDateTime(field);
        return (dt != null ?  (DateTimeOffset?) new DateTimeOffset(dt.Value, TimeSpan.Zero) : null);
    }

    /// <summary>
    /// 	Gets the record value casted as int or 0.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static int GetInt32(this IDataReader reader, string field) {
        return reader.GetInt32(field, 0);
    }

    /// <summary>
    /// 	Gets the record value casted as int or the specified default value.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static int GetInt32(this IDataReader reader, string field, int defaultValue) {
        var value = reader[field];
        return (value is int ? (int)value : defaultValue);
    }

    /// <summary>
    /// 	Gets the record value casted as int or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static int? GetNullableInt32(this IDataReader reader, string field) {
        var value = reader[field];
        return (value is int ? (int?)value : null);
    }

    /// <summary>
    /// 	Gets the record value casted as long or 0.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static long GetInt64(this IDataReader reader, string field) {
        return reader.GetInt64(field, 0);
    }

    /// <summary>
    /// 	Gets the record value casted as long or the specified default value.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static long GetInt64(this IDataReader reader, string field, int defaultValue) {
        var value = reader[field];
        return (value is long ? (long)value : defaultValue);
    }

    /// <summary>
    /// 	Gets the record value casted as long or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static long? GetNullableInt64(this IDataReader reader, string field) {
        var value = reader[field];
        return (value is long ? (long?)value : null);
    }

    /// <summary>
    /// 	Gets the record value casted as decimal or 0.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static decimal GetDecimal(this IDataReader reader, string field) {
        return reader.GetDecimal(field, 0);
    }

    /// <summary>
    /// 	Gets the record value casted as decimal or the specified default value.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static decimal GetDecimal(this IDataReader reader, string field, long defaultValue) {
        var value = reader[field];
        return (value is decimal ? (decimal)value : defaultValue);
    }

    /// <summary>
    /// 	Gets the record value casted as decimal or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static decimal? GetNullableDecimal(this IDataReader reader, string field) {
        var value = reader[field];
        return (value is decimal ? (decimal?)value : null);
    }

    /// <summary>
    /// 	Gets the record value casted as bool or false.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static bool GetBoolean(this IDataReader reader, string field) {
        return reader.GetBoolean(field, false);
    }

    /// <summary>
    /// 	Gets the record value casted as bool or the specified default value.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static bool GetBoolean(this IDataReader reader, string field, bool defaultValue) {
        var value = reader[field];
        return (value is bool ? (bool)value : defaultValue);
    }

    /// <summary>
    /// 	Gets the record value casted as bool or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static bool? GetNullableBoolean(this IDataReader reader, string field) {
        var value = reader[field];
        return (value is bool ? (bool?)value : null);
    }

    /// <summary>
    /// 	Gets the record value as Type class instance or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static Type GetType(this IDataReader reader, string field) {
        return reader.GetType(field, null);
    }

    /// <summary>
    /// 	Gets the record value as Type class instance or the specified default value.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static Type GetType(this IDataReader reader, string field, Type defaultValue) {
        var classType = reader.GetString(field);
        if (classType.IsNotEmpty()) {
            var type = Type.GetType(classType);
            if (type != null)
                return type;
        }
        return defaultValue;
    }

    /// <summary>
    /// 	Gets the record value as class instance from a type name or null.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static object GetTypeInstance(this IDataReader reader, string field) {
        return reader.GetTypeInstance(field, null);
    }

    /// <summary>
    /// 	Gets the record value as class instance from a type name or the specified default type.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static object GetTypeInstance(this IDataReader reader, string field, Type defaultValue) {
        var type = reader.GetType(field, defaultValue);
        return (type != null ? Activator.CreateInstance(type) : null);
    }

    /// <summary>
    /// 	Gets the record value as class instance from a type name or null.
    /// </summary>
    /// <typeparam name = "T">The type to be casted to</typeparam>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static T GetTypeInstance<T>(this IDataReader reader, string field) where T : class {
        return (reader.GetTypeInstance(field, null) as T);
    }

    /// <summary>
    /// 	Gets the record value as class instance from a type name or the specified default type.
    /// </summary>
    /// <typeparam name = "T">The type to be casted to</typeparam>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <param name = "type">The type.</param>
    /// <returns>The record value</returns>
    public static T GetTypeInstanceSafe<T>(this IDataReader reader, string field, Type type) where T : class {
        var instance = (reader.GetTypeInstance(field, null) as T);
        return (instance ?? Activator.CreateInstance(type) as T);
    }

    /// <summary>
    /// 	Gets the record value as class instance from a type name or an instance from the specified type.
    /// </summary>
    /// <typeparam name = "T">The type to be casted to</typeparam>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static T GetTypeInstanceSafe<T>(this IDataReader reader, string field) where T : class, new() {
        var instance = (reader.GetTypeInstance(field, null) as T);
        return (instance ?? new T());
    }

    /// <summary>
    /// 	Determines whether the record value is DBNull.Value
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "field">The name of the record field.</param>
    /// <returns>
    /// 	<c>true</c> if the value is DBNull.Value; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDBNull(this IDataReader reader, string field) {
        var value = reader[field];
        return (value == DBNull.Value);
    }

    /// <summary>
    /// 	Reads all all records from a data reader and performs an action for each.
    /// </summary>
    /// <param name = "reader">The data reader.</param>
    /// <param name = "action">The action to be performed.</param>
    /// <returns>
    /// 	The count of actions that were performed.
    /// </returns>
    public static int ReadAll(this IDataReader reader, Action<IDataReader> action) {
        var count = 0;
        while (reader.Read()) {
            action(reader);
            count++;
        }
        return count;
    }

    /// <summary>
    /// Returns the index of a column by name (case insensitive) or -1
    /// </summary>
    /// <param name="this"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int IndexOf(this IDataRecord @this, string name)
    {
        for (int i = 0; i < @this.FieldCount; i++)
        {
            if (string.Compare(@this.GetName(i), name, true) == 0) return i;
        }
        return -1;
    }
}
