using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using PGK.Extensions;

/// <summary>
/// 	Extension methods for the root data type object
/// </summary>
public static class ObjectExtensions
{
	/// <summary>
	/// 	Determines whether the object is equal to any of the provided values.
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "obj">The object to be compared.</param>
	/// <param name = "values">The values to compare with the object.</param>
	/// <returns></returns>
	public static bool EqualsAny<T>(this T obj, params T[] values)
	{
		return (Array.IndexOf(values, obj) != -1);
	}

	/// <summary>
	/// 	Determines whether the object is equal to none of the provided values.
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "obj">The object to be compared.</param>
	/// <param name = "values">The values to compare with the object.</param>
	/// <returns></returns>
	public static bool EqualsNone<T>(this T obj, params T[] values)
	{
		return (obj.EqualsAny(values) == false);
	}

	/// <summary>
	/// 	Converts an object to the specified target type or returns the default value if
	///     those 2 types are not convertible.
	///     <para>
	///     If the <paramref name="value"/> can't be convert even if the types are 
	///     convertible with each other, an exception is thrown.</para>
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "value">The value.</param>
	/// <returns>The target type</returns>
	public static T ConvertTo<T>(this object value)
	{
		return value.ConvertTo(default(T));
	}

	/// <summary>
	/// 	Converts an object to the specified target type or returns the default value.
	///     <para>Any exceptions are ignored. </para>
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "value">The value.</param>
	/// <returns>The target type</returns>
	public static T ConvertToAndIgnoreException<T>(this object value)
	{
		return value.ConvertToAndIgnoreException(default(T));
	}

	/// <summary>
	/// 	Converts an object to the specified target type or returns the default value.
	///     <para>Any exceptions are ignored. </para>
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "value">The value.</param>
	/// <param name = "defaultValue">The default value.</param>
	/// <returns>The target type</returns>
	public static T ConvertToAndIgnoreException<T>(this object value, T defaultValue)
	{
		return value.ConvertTo(defaultValue, true);
	}

	/// <summary>
	/// 	Converts an object to the specified target type or returns the default value if
	///     those 2 types are not convertible.
	///     <para>
	///     If the <paramref name="value"/> can't be convert even if the types are 
	///     convertible with each other, an exception is thrown.</para>
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "value">The value.</param>
	/// <param name = "defaultValue">The default value.</param>
	/// <returns>The target type</returns>
	public static T ConvertTo<T>(this object value, T defaultValue)
	{
		if (value != null)
		{
			var targetType = typeof(T);

			if (value.GetType() == targetType) return (T)value;

			var converter = TypeDescriptor.GetConverter(value);
			if (converter != null)
			{
				if (converter.CanConvertTo(targetType))
					return (T)converter.ConvertTo(value, targetType);
			}

			converter = TypeDescriptor.GetConverter(targetType);
			if (converter != null)
			{
				if (converter.CanConvertFrom(value.GetType()))
					return (T)converter.ConvertFrom(value);
			}
		}
		return defaultValue;
	}

	/// <summary>
	/// 	Converts an object to the specified target type or returns the default value if
	///     those 2 types are not convertible.
	///     <para>Any exceptions are optionally ignored (<paramref name="ignoreException"/>).</para>
	///     <para>
	///     If the exceptions are not ignored and the <paramref name="value"/> can't be convert even if 
	///     the types are convertible with each other, an exception is thrown.</para>
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "value">The value.</param>
	/// <param name = "defaultValue">The default value.</param>
	/// <param name = "ignoreException">if set to <c>true</c> ignore any exception.</param>
	/// <returns>The target type</returns>
	public static T ConvertTo<T>(this object value, T defaultValue, bool ignoreException)
	{
		if (ignoreException)
		{
			try
			{
				return value.ConvertTo<T>();
			}
			catch
			{
				return defaultValue;
			}
		}
		return value.ConvertTo<T>();
	}

	/// <summary>
	/// 	Determines whether the value can (in theory) be converted to the specified target type.
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "value">The value.</param>
	/// <returns>
	/// 	<c>true</c> if this instance can be convert to the specified target type; otherwise, <c>false</c>.
	/// </returns>
	public static bool CanConvertTo<T>(this object value)
	{
		if (value != null)
		{
			var targetType = typeof(T);

			var converter = TypeDescriptor.GetConverter(value);
			if (converter != null)
			{
				if (converter.CanConvertTo(targetType))
					return true;
			}

			converter = TypeDescriptor.GetConverter(targetType);
			if (converter != null)
			{
				if (converter.CanConvertFrom(value.GetType()))
					return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 	Converts the specified value to a different type.
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "value">The value.</param>
	/// <returns>An universal converter suppliying additional target conversion methods</returns>
	/// <example>
	/// 	<code>
	/// 		var value = "123";
	/// 		var numeric = value.ConvertTo().ToInt32();
	/// 	</code>
	/// </example>
	public static IConverter<T> ConvertTo<T>(this T value)
	{
		return new Converter<T>(value);
	}

	/// <summary>
	/// 	Dynamically invokes a method using reflection
	/// </summary>
	/// <param name = "obj">The object to perform on.</param>
	/// <param name = "methodName">The name of the method.</param>
	/// <param name = "parameters">The parameters passed to the method.</param>
	/// <returns>The return value</returns>
	/// <example>
	/// 	<code>
	/// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
	/// 		var file = type.CreateInstance(@"c:\autoexec.bat");
	/// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
	/// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
	/// 		Console.WriteLine(reader.ReadToEnd());
	/// 		reader.Close();
	/// 		}
	/// 	</code>
	/// </example>
	public static object InvokeMethod(this object obj, string methodName, params object[] parameters)
	{
		return InvokeMethod<object>(obj, methodName, parameters);
	}

	/// <summary>
	/// 	Dynamically invokes a method using reflection and returns its value in a typed manner
	/// </summary>
	/// <typeparam name = "T">The expected return data types</typeparam>
	/// <param name = "obj">The object to perform on.</param>
	/// <param name = "methodName">The name of the method.</param>
	/// <param name = "parameters">The parameters passed to the method.</param>
	/// <returns>The return value</returns>
	/// <example>
	/// 	<code>
	/// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
	/// 		var file = type.CreateInstance(@"c:\autoexec.bat");
	/// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
	/// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
	/// 		Console.WriteLine(reader.ReadToEnd());
	/// 		reader.Close();
	/// 		}
	/// 	</code>
	/// </example>
	public static T InvokeMethod<T>(this object obj, string methodName, params object[] parameters)
	{
		var type = obj.GetType();
		var method = type.GetMethod(methodName, parameters.Select(o=>o.GetType()).ToArray());

		if (method == null)
			throw new ArgumentException(string.Format("Method '{0}' not found.", methodName), methodName);

		var value = method.Invoke(obj, parameters);
		return (value is T ? (T)value : default(T));
	}

	/// <summary>
	/// 	Dynamically retrieves a property value.
	/// </summary>
	/// <param name = "obj">The object to perform on.</param>
	/// <param name = "propertyName">The Name of the property.</param>
	/// <returns>The property value.</returns>
	/// <example>
	/// 	<code>
	/// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
	/// 		var file = type.CreateInstance(@"c:\autoexec.bat");
	/// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
	/// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
	/// 		Console.WriteLine(reader.ReadToEnd());
	/// 		reader.Close();
	/// 		}
	/// 	</code>
	/// </example>
	public static object GetPropertyValue(this object obj, string propertyName)
	{
		return GetPropertyValue<object>(obj, propertyName, null);
	}

	/// <summary>
	/// 	Dynamically retrieves a property value.
	/// </summary>
	/// <typeparam name = "T">The expected return data type</typeparam>
	/// <param name = "obj">The object to perform on.</param>
	/// <param name = "propertyName">The Name of the property.</param>
	/// <returns>The property value.</returns>
	/// <example>
	/// 	<code>
	/// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
	/// 		var file = type.CreateInstance(@"c:\autoexec.bat");
	/// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
	/// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
	/// 		Console.WriteLine(reader.ReadToEnd());
	/// 		reader.Close();
	/// 		}
	/// 	</code>
	/// </example>
	public static T GetPropertyValue<T>(this object obj, string propertyName)
	{
		return GetPropertyValue(obj, propertyName, default(T));
	}

	/// <summary>
	/// 	Dynamically retrieves a property value.
	/// </summary>
	/// <typeparam name = "T">The expected return data type</typeparam>
	/// <param name = "obj">The object to perform on.</param>
	/// <param name = "propertyName">The Name of the property.</param>
	/// <param name = "defaultValue">The default value to return.</param>
	/// <returns>The property value.</returns>
	/// <example>
	/// 	<code>
	/// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
	/// 		var file = type.CreateInstance(@"c:\autoexec.bat");
	/// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
	/// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
	/// 		Console.WriteLine(reader.ReadToEnd());
	/// 		reader.Close();
	/// 		}
	/// 	</code>
	/// </example>
	public static T GetPropertyValue<T>(this object obj, string propertyName, T defaultValue)
	{
		var type = obj.GetType();
		var property = type.GetProperty(propertyName);

		if (property == null)
			throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);

		var value = property.GetValue(obj, null);
		return (value is T ? (T)value : defaultValue);
	}

	/// <summary>
	/// 	Dynamically sets a property value.
	/// </summary>
	/// <param name = "obj">The object to perform on.</param>
	/// <param name = "propertyName">The Name of the property.</param>
	/// <param name = "value">The value to be set.</param>
	public static void SetPropertyValue(this object obj, string propertyName, object value)
	{
		var type = obj.GetType();
		var property = type.GetProperty(propertyName);

		if (property == null)
			throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);
		if (!property.CanWrite)
			throw new ArgumentException(string.Format("Property '{0}' does not allow writes.", propertyName), propertyName);
		property.SetValue(obj, value, null);
	}

	/// <summary>
	/// 	Gets the first matching attribute defined on the data type.
	/// </summary>
	/// <typeparam name = "T">The attribute type tp look for.</typeparam>
	/// <param name = "obj">The object to look on.</param>
	/// <returns>The found attribute</returns>
	public static T GetAttribute<T>(this object obj) where T : Attribute
	{
		return GetAttribute<T>(obj, true);
	}

	/// <summary>
	/// 	Gets the first matching attribute defined on the data type.
	/// </summary>
	/// <typeparam name = "T">The attribute type tp look for.</typeparam>
	/// <param name = "obj">The object to look on.</param>
	/// <param name = "includeInherited">if set to <c>true</c> includes inherited attributes.</param>
	/// <returns>The found attribute</returns>
	public static T GetAttribute<T>(this object obj, bool includeInherited) where T : Attribute
	{
		var type = (obj as Type ?? obj.GetType());
		var attributes = type.GetCustomAttributes(typeof(T), includeInherited);
		return attributes.FirstOrDefault() as T;
	}

	/// <summary>
	/// 	Gets all matching attribute defined on the data type.
	/// </summary>
	/// <typeparam name = "T">The attribute type tp look for.</typeparam>
	/// <param name = "obj">The object to look on.</param>
	/// <returns>The found attributes</returns>
	public static IEnumerable<T> GetAttributes<T>(this object obj) where T : Attribute
	{
		return GetAttributes<T>(obj, false);
	}

	/// <summary>
	/// 	Gets all matching attribute defined on the data type.
	/// </summary>
	/// <typeparam name = "T">The attribute type tp look for.</typeparam>
	/// <param name = "obj">The object to look on.</param>
	/// <param name = "includeInherited">if set to <c>true</c> includes inherited attributes.</param>
	/// <returns>The found attributes</returns>
	public static IEnumerable<T> GetAttributes<T>(this object obj, bool includeInherited) where T : Attribute
	{
		return (obj as Type ?? obj.GetType()).GetCustomAttributes(typeof(T), includeInherited).OfType<T>().Select(attribute => attribute);
	}

	/// <summary>
	/// 	Determines whether the object is exactly of the passed generic type.
	/// </summary>
	/// <typeparam name = "T">The target type.</typeparam>
	/// <param name = "obj">The object to check.</param>
	/// <returns>
	/// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsOfType<T>(this object obj)
	{
		return obj.IsOfType(typeof(T));
	}

	/// <summary>
	/// 	Determines whether the object is excactly of the passed type
	/// </summary>
	/// <param name = "obj">The object to check.</param>
	/// <param name = "type">The target type.</param>
	/// <returns>
	/// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsOfType(this object obj, Type type)
	{
		return (obj.GetType().Equals(type));
	}

	/// <summary>
	/// 	Determines whether the object is of the passed generic type or inherits from it.
	/// </summary>
	/// <typeparam name = "T">The target type.</typeparam>
	/// <param name = "obj">The object to check.</param>
	/// <returns>
	/// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsOfTypeOrInherits<T>(this object obj)
	{
		return obj.IsOfTypeOrInherits(typeof(T));
	}

	/// <summary>
	/// 	Determines whether the object is of the passed type or inherits from it.
	/// </summary>
	/// <param name = "obj">The object to check.</param>
	/// <param name = "type">The target type.</param>
	/// <returns>
	/// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsOfTypeOrInherits(this object obj, Type type)
	{
		var objectType = obj.GetType();

		do
		{
			if (objectType.Equals(type))
				return true;
			if ((objectType == objectType.BaseType) || (objectType.BaseType == null))
				return false;
			objectType = objectType.BaseType;
		} while (true);
	}

	/// <summary>
	/// 	Determines whether the object is assignable to the passed generic type.
	/// </summary>
	/// <typeparam name = "T">The target type.</typeparam>
	/// <param name = "obj">The object to check.</param>
	/// <returns>
	/// 	<c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsAssignableTo<T>(this object obj)
	{
		return obj.IsAssignableTo(typeof(T));
	}

	/// <summary>
	/// 	Determines whether the object is assignable to the passed type.
	/// </summary>
	/// <param name = "obj">The object to check.</param>
	/// <param name = "type">The target type.</param>
	/// <returns>
	/// 	<c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsAssignableTo(this object obj, Type type)
	{
		var objectType = obj.GetType();
		return type.IsAssignableFrom(objectType);
	}

	/// <summary>
	/// 	Gets the type default value for the underlying data type, in case of reference types: null
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "value">The value.</param>
	/// <returns>The default value</returns>
	public static T GetTypeDefaultValue<T>(this T value)
	{
		return default(T);
	}

	/// <summary>
	/// 	Converts the specified value to a database value and returns DBNull.Value if the value equals its default.
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	/// <param name = "value">The value.</param>
	/// <returns></returns>
	public static object ToDatabaseValue<T>(this T value)
	{
		return (value.Equals(value.GetTypeDefaultValue()) ? DBNull.Value : (object)value);
	}

	/// <summary>
	/// 	Cast an object to the given type. Usefull especially for anonymous types.
	/// </summary>
	/// <typeparam name = "T">The type to cast to</typeparam>
	/// <param name = "value">The object to case</param>
	/// <returns>
	/// 	the casted type or null if casting is not possible.
	/// </returns>
	/// <remarks>
	/// 	Contributed by blaumeister, http://www.codeplex.com/site/users/view/blaumeiser
	/// </remarks>
	public static T CastTo<T>(this object value)
	{
		if (value == null || !(value is T))
			return default(T);

		return (T)value;
	}

	/// <summary>
	/// 	Returns TRUE, if specified target reference is equals with null reference.
	/// 	Othervise returns FALSE.
	/// </summary>
	/// <param name = "target">Target reference. Can be null.</param>
	/// <remarks>
	/// 	Some types has overloaded '==' and '!=' operators.
	/// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
	/// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
	/// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
	/// 
	/// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	/// </remarks>
	/// <example>
	/// 	object someObject = GetSomeObject();
	/// 	if ( someObject.IsNull() ) { /* the someObject is null */ }
	/// 	else { /* the someObject is not null */ }
	/// </example>
	public static bool IsNull(this object target)
	{
		var ret = IsNull<object>(target);
		return ret;
	}

	/// <summary>
	/// 	Returns TRUE, if specified target reference is equals with null reference.
	/// 	Othervise returns FALSE.
	/// </summary>
	/// <typeparam name = "T">Type of target.</typeparam>
	/// <param name = "target">Target reference. Can be null.</param>
	/// <remarks>
	/// 	Some types has overloaded '==' and '!=' operators.
	/// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
	/// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
	/// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
	/// 
	/// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	/// </remarks>
	/// <example>
	/// 	MyClass someObject = GetSomeObject();
	/// 	if ( someObject.IsNull() ) { /* the someObject is null */ }
	/// 	else { /* the someObject is not null */ }
	/// </example>
	public static bool IsNull<T>(this T target)
	{
		var result = ReferenceEquals(target, null);
		return result;
	}

	/// <summary>
	/// 	Returns TRUE, if specified target reference is equals with null reference.
	/// 	Othervise returns FALSE.
	/// </summary>
	/// <param name = "target">Target reference. Can be null.</param>
	/// <remarks>
	/// 	Some types has overloaded '==' and '!=' operators.
	/// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
	/// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
	/// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
	/// 
	/// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	/// </remarks>
	/// <example>
	/// 	object someObject = GetSomeObject();
	/// 	if ( someObject.IsNotNull() ) { /* the someObject is not null */ }
	/// 	else { /* the someObject is null */ }
	/// </example>
	public static bool IsNotNull(this object target)
	{
		var ret = IsNotNull<object>(target);
		return ret;
	}

	/// <summary>
	/// 	Returns TRUE, if specified target reference is equals with null reference.
	/// 	Othervise returns FALSE.
	/// </summary>
	/// <typeparam name = "T">Type of target.</typeparam>
	/// <param name = "target">Target reference. Can be null.</param>
	/// <remarks>
	/// 	Some types has overloaded '==' and '!=' operators.
	/// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
	/// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
	/// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
	/// 
	/// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	/// </remarks>
	/// <example>
	/// 	MyClass someObject = GetSomeObject();
	/// 	if ( someObject.IsNotNull() ) { /* the someObject is not null */ }
	/// 	else { /* the someObject is null */ }
	/// </example>
	public static bool IsNotNull<T>(this T target)
	{
		var result = !ReferenceEquals(target, null);
		return result;
	}

	/// <summary>
	/// 	If target is null, returns null.
	/// 	Othervise returns string representation of target using current culture format provider.
	/// </summary>
	/// <param name = "target">Target transforming to string representation. Can be null.</param>
	/// <example>
	/// 	float? number = null;
	/// 	string text1 = number.AsString();
	/// 
	/// 	number = 15.7892;
	/// 	string text2 = number.AsString();
	/// </example>
	/// <remarks>
	/// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	/// </remarks>
	public static string AsString(this object target)
	{
		return ReferenceEquals(target, null) ? null : string.Format("{0}", target);
	}

	/// <summary>
	/// 	If target is null, returns null.
	/// 	Othervise returns string representation of target using specified format provider.
	/// </summary>
	/// <param name = "target">Target transforming to string representation. Can be null.</param>
	/// <param name = "formatProvider">Format provider used to transformation target to string representation.</param>
	/// <example>
	/// 	CultureInfo czech = new CultureInfo("cs-CZ");
	/// 
	/// 	float? number = null;
	/// 	string text1 = number.AsString( czech );
	/// 
	/// 	number = 15.7892;
	/// 	string text2 = number.AsString( czech );
	/// </example>
	/// <remarks>
	/// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	/// </remarks>
	public static string AsString(this object target, IFormatProvider formatProvider)
	{
		var result = string.Format(formatProvider, "{0}", target);
		return result;
	}

	/// <summary>
	/// 	If target is null, returns null.
	/// 	Othervise returns string representation of target using invariant format provider.
	/// </summary>
	/// <param name = "target">Target transforming to string representation. Can be null.</param>
	/// <example>
	/// 	float? number = null;
	/// 	string text1 = number.AsInvariantString();
	/// 
	/// 	number = 15.7892;
	/// 	string text2 = number.AsInvariantString();
	/// </example>
	/// <remarks>
	/// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	/// </remarks>
	public static string AsInvariantString(this object target)
	{
		var result = string.Format(CultureInfo.InvariantCulture, "{0}", target);
		return result;
	}

	/// <summary>
	/// 	If target is null reference, returns notNullValue.
	/// 	Othervise returns target.
	/// </summary>
	/// <typeparam name = "T">Type of target.</typeparam>
	/// <param name = "target">Target which is maybe null. Can be null.</param>
	/// <param name = "notNullValue">Value used instead of null.</param>
	/// <example>
	/// 	const int DEFAULT_NUMBER = 123;
	/// 
	/// 	int? number = null;
	/// 	int notNullNumber1 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 123
	/// 
	/// 	number = 57;
	/// 	int notNullNumber2 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 57
	/// </example>
	/// <remarks>
	/// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	/// </remarks>
	public static T NotNull<T>(this T target, T notNullValue)
	{
		return ReferenceEquals(target, null) ? notNullValue : target;
	}

	/// <summary>
	/// 	If target is null reference, returns result from notNullValueProvider.
	/// 	Othervise returns target.
	/// </summary>
	/// <typeparam name = "T">Type of target.</typeparam>
	/// <param name = "target">Target which is maybe null. Can be null.</param>
	/// <param name = "notNullValueProvider">Delegate which return value is used instead of null.</param>
	/// <example>
	/// 	int? number = null;
	/// 	int notNullNumber1 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns random number from 10 to 20
	/// 
	/// 	number = 57;
	/// 	int notNullNumber2 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns 57
	/// </example>
	/// <remarks>
	/// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
	/// </remarks>
	public static T NotNull<T>(this T target, Func<T> notNullValueProvider)
	{
		return ReferenceEquals(target, null) ? notNullValueProvider() : target;
	}

	/// <summary>
	/// 	get a string representation of a given object.
	/// </summary>
	/// <param name = "o">the object to dump</param>
	/// <param name = "flags">BindingFlags to use for reflection</param>
	/// <param name = "maxArrayElements">Number of elements to show for IEnumerables</param>
	/// <returns></returns>
	public static string ToStringDump(this object o, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, int maxArrayElements = 5)
	{
		return ToStringDumpInternal(o.ToXElement(flags, maxArrayElements)).Aggregate(new StringBuilder(), (sb, el) => sb.Append(el)).ToString();
	}

	static IEnumerable<string> ToStringDumpInternal(XContainer toXElement)
	{
		foreach (var xElement in toXElement.Elements().OrderBy(o => o.Name.ToString()))
		{
			if (xElement.HasElements)
			{
				foreach (var el in ToStringDumpInternal(xElement))
					yield return "{" + String.Format("{0}={1}", xElement.Name, el) + "}";
			}
			else
				yield return "{" + String.Format("{0}={1}", xElement.Name, xElement.Value) + "}";
		}
	}

	/// <summary>
	/// 	get a html-table representation of a given object.
	/// </summary>
	/// <param name = "o">the object to dump</param>
	/// <param name = "flags">BindingFlags to use for reflection</param>
	/// <param name = "maxArrayElements">Number of elements to show for IEnumerables</param>
	/// <returns></returns>
	public static string ToHTMLTable(this object o, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, int maxArrayElements = 5)
	{
		return ToHTMLTableInternal(o.ToXElement(flags, maxArrayElements), 0).Aggregate(String.Empty, (str, el) => str + el);
	}

	static IEnumerable<string> ToHTMLTableInternal(XContainer xel, int padding)
	{
		yield return FormatHTMLLine("<table>", padding);
		yield return FormatHTMLLine("<tr><th>Attribute</th><th>Value</th></tr>", padding + 1);
		foreach (var xElement in xel.Elements().OrderBy(o => o.Name.ToString()))
		{
			if (xElement.HasElements)
			{
				yield return FormatHTMLLine(String.Format("<tr><td>{0}</td><td>", xElement.Name), padding + 1);
				foreach (var el in ToHTMLTableInternal(xElement, padding + 2))
					yield return el;
				yield return FormatHTMLLine("</td></tr>", padding + 1);
			}
			else
				yield return FormatHTMLLine(String.Format("<tr><td>{0}</td><td>{1}</td></tr>", xElement.Name, HttpUtility.HtmlEncode(xElement.Value)), padding + 1);
		}
		yield return FormatHTMLLine("</table>", padding);
	}

	static string FormatHTMLLine(string tag, int padding)
	{
		return String.Format("{0}{1}{2}", String.Empty.PadRight(padding, '\t'), tag, Environment.NewLine);
	}

	/// <summary>
	/// 	get a XElement representation of a given object.
	/// </summary>
	/// <param name = "o">the object to dump</param>
	/// <param name = "flags">BindingFlags to use for reflection</param>
	/// <param name = "maxArrayElements">Number of elements to show for IEnumerables</param>
	/// <returns></returns>
	public static XElement ToXElement(this object o, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, int maxArrayElements = 5)
	{
		try
		{
			return ToXElementInternal(o, new HashSet<object>(), flags, maxArrayElements);
		}
		catch
		{
			return new XElement(o.GetType().Name);
		}
	}

	// todo: Please document these methods
	static XElement ToXElementInternal(object o, ICollection<object> visited, BindingFlags flags, int maxArrayElements)
	{
		if (o == null)
			return new XElement("null");
		if (visited.Contains(o))
			return new XElement("cyclicreference");

		if (!o.GetType().IsValueType)
			visited.Add(o);

		var type = o.GetType();
		var elems = new XElement(CleanName(type.Name, type.IsArray));

		if (!NeedRecursion(type, o))
		{
			elems.Add(new XElement(CleanName(type.Name, type.IsArray), String.Empty + o));
			return elems;
		}
		if (o is IEnumerable)
		{
			var i = 0;
			foreach (var el in o as IEnumerable)
			{
				var subtype = el.GetType();
				elems.Add(NeedRecursion(subtype, el) ? ToXElementInternal(el, visited, flags, maxArrayElements) : new XElement(CleanName(subtype.Name, subtype.IsArray), el));
				if (i++ >= maxArrayElements)
					break;
			}
			return elems;
		}
		foreach (var propertyInfo in from propertyInfo in type.GetProperties(flags)
																 where propertyInfo.CanRead
																 select propertyInfo)
		{
			var value = GetValue(o, propertyInfo);
			elems.Add(NeedRecursion(propertyInfo.PropertyType, value)
															? new XElement(CleanName(propertyInfo.Name, propertyInfo.PropertyType.IsArray), ToXElementInternal(value, visited, flags, maxArrayElements))
															: new XElement(CleanName(propertyInfo.Name, propertyInfo.PropertyType.IsArray), String.Empty + value));
		}
		foreach (var fieldInfo in type.GetFields())
		{
			var value = fieldInfo.GetValue(o);
			elems.Add(NeedRecursion(fieldInfo.FieldType, value)
															? new XElement(CleanName(fieldInfo.Name, fieldInfo.FieldType.IsArray), ToXElementInternal(value, visited, flags, maxArrayElements))
															: new XElement(CleanName(fieldInfo.Name, fieldInfo.FieldType.IsArray), String.Empty + value));
		}
		return elems;
	}

	static bool NeedRecursion(Type type, object o)
	{
		return o != null && (!type.IsPrimitive && !(o is String || o is DateTime || o is DateTimeOffset || o is TimeSpan || o is Delegate || o is Enum || o is Decimal || o is Guid));
	}

	static object GetValue(object o, PropertyInfo propertyInfo)
	{
		object value;
		try
		{
			value = propertyInfo.GetValue(o, null);
		}
		catch
		{
			try
			{
				value = propertyInfo.GetValue(o,
						new object[]
					{
						0
					});
			}
			catch
			{
				value = null;
			}
		}
		return value;
	}

	static string CleanName(IEnumerable<char> name, bool isArray)
	{
		var sb = new StringBuilder();
		foreach (var c in name.Where(c => Char.IsLetterOrDigit(c) && c != '`').Select(c => c))
			sb.Append(c);
		if (isArray)
			sb.Append("Array");
		return sb.ToString();
	}

	/// <summary>
	/// 	Cast an object to the given type. Usefull especially for anonymous types.
	/// </summary>
	/// <param name="obj">The object to be cast</param>
	/// <param name="targetType">The type to cast to</param>
	/// <returns>
	/// 	the casted type or null if casting is not possible.
	/// </returns>
	/// <remarks>
	/// 	Contributed by Michael T, http://about.me/MichaelTran
	/// </remarks>
	public static object DynamicCast(this object obj, Type targetType)
	{
		// First, it might be just a simple situation
		if (targetType.IsAssignableFrom(obj.GetType()))
			return obj;

		// If not, we need to find a cast operator. The operator
		// may be explicit or implicit and may be included in
		// either of the two types...
		const BindingFlags pubStatBinding = BindingFlags.Public | BindingFlags.Static;
		var originType = obj.GetType();
		String[] names = { "op_Implicit", "op_Explicit" };

		var castMethod =
				targetType.GetMethods(pubStatBinding).Union(originType.GetMethods(pubStatBinding)).FirstOrDefault(
						itm => itm.ReturnType.Equals(targetType) && itm.GetParameters().Length == 1 && itm.GetParameters()[0].ParameterType.IsAssignableFrom(originType) && names.Contains(itm.Name));
		if (null != castMethod)
			return castMethod.Invoke(null, new[] { obj });
		throw new InvalidOperationException(
				String.Format(
						"No matching cast operator found from {0} to {1}.",
						originType.Name,
						targetType.Name));
	}

	/// <summary>
	/// Cast an object to the given type. Usefull especially for anonymous types.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj">The object to be cast</param>
	/// <returns>
	/// the casted type or null if casting is not possible.
	/// </returns>
	/// <remarks>
	/// Contributed by Michael T, http://about.me/MichaelTran
	/// </remarks>
	public static T CastAs<T>(this object obj) where T : class, new()
	{
		return obj as T;
	}

	/// <summary>
	/// Counts and returns the recursive execution of the passed function until it returns null.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="item">The item to start peforming on.</param>
	/// <param name="function">The function to be executed.</param>
	/// <returns>The number of executions until the function returned null</returns>
	public static int CountLoopsToNull<T>(this T item, Func<T, T> function) where T : class
	{
		var num = 0;
		while ((item = function(item)) != null)
		{
			num++;
		}
		return num;
	}

	/// <summary>
	/// Finds a type instance using a recursive call. The method is useful to find specific parents for example.
	/// </summary>
	/// <typeparam name="T">The source type to perform on.</typeparam>
	/// <typeparam name="K">The targte type to be returned</typeparam>
	/// <param name="item">The item to start performing on.</param>
	/// <param name="function">The function to be executed.</param>
	/// <returns>An target type instance or null.</returns>
	/// <example><code>
	/// var tree = ...
	/// var node = tree.FindNodeByValue("");
	/// var parentByType = node.FindTypeByRecursion%lt;TheType&gt;(n => n.Parent);
	/// </code></example>
	public static K FindTypeByRecursion<T, K>(this T item, Func<T, T> function)
		where T : class
		where K : class, T
	{
		do
		{
			if (item is K) return (K)item;
		}
		while ((item = function(item)) != null);
		return null;
	}

	/// <summary>
	/// Perform a deep Copy of the object.
	/// </summary>
	/// <typeparam name="T">The type of object being copied.</typeparam>
	/// <param name="source">The object instance to copy.</param>
	/// <returns>The copied object.</returns>
	public static T Clone<T>(this T source)
	{
		if (!typeof(T).IsSerializable)
		{
			throw new ArgumentException("The type must be serializable.", "source");
		}

		// Don't serialize a null object, simply return the default for that object
		if (Object.ReferenceEquals(source, null))
		{
			return default(T);
		}

		IFormatter formatter = new BinaryFormatter();
		Stream stream = new MemoryStream();
		using (stream)
		{
			formatter.Serialize(stream, source);
			stream.Seek(0, SeekOrigin.Begin);
			return (T)formatter.Deserialize(stream);
		}
	}

    /// <summary>
    /// Casts the specified object to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to cast to</typeparam>
    /// <param name="o">The Object being casted</param>
    /// <returns>returns the object as casted type.</returns>
    public static T Cast<T>(this object o)
    {
        if (o == null)
            throw new NullReferenceException();
        return (T)Convert.ChangeType(o, typeof(T));
    }

    /// <summary>
    /// Casts the specified object. If the object is null a return type can be specified.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="o">The Object being casted</param>
    /// <param name="defaultValue">The default Type.</param>
    /// <returns>returns the object as casted type. If null the default type is returned.</returns>
    public static T Cast<T>(this object o, T defaultValue)
    {
        if (o == null)
            return defaultValue;
        return (T)Convert.ChangeType(o, typeof(T));
    }

    /// <summary>
    /// Copies the readable and writable public property values from the source object to the target
    /// </summary>
    /// <remarks>The source and target objects must be of the same type.</remarks>
    /// <param name="target">The target object</param>
    /// <param name="source">The source object</param>
    public static void CopyPropertiesFrom(this object target, object source)
    {
        CopyPropertiesFrom(target, source, string.Empty);
    }

    /// <summary>
    /// Copies the readable and writable public property values from the source object to the target
    /// </summary>
    /// <remarks>The source and target objects must be of the same type.</remarks>
    /// <param name="target">The target object</param>
    /// <param name="source">The source object</param>
    /// <param name="ignoreProperty">A single property name to ignore</param>
    public static void CopyPropertiesFrom(this object target, object source, string ignoreProperty)
    {
        CopyPropertiesFrom(target, source, new[] { ignoreProperty });
    }

    /// <summary>
    /// Copies the readable and writable public property values from the source object to the target
    /// </summary>
    /// <remarks>The source and target objects must be of the same type.</remarks>
    /// <param name="target">The target object</param>
    /// <param name="source">The source object</param>
    /// <param name="ignoreProperties">An array of property names to ignore</param>
    public static void CopyPropertiesFrom(this object target, object source, string[] ignoreProperties)
    {
        // Get and check the object types
        Type type = source.GetType();
        if (target.GetType() != type)
        {
            throw new ArgumentException("The source type must be the same as the target");
        }

        // Build a clean list of property names to ignore
        var ignoreList = new List<string>();
        foreach (string item in ignoreProperties)
        {
            if (!string.IsNullOrEmpty(item) && !ignoreList.Contains(item))
            {
                ignoreList.Add(item);
            }
        }

        // Copy the properties
        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.CanWrite
                && property.CanRead
                && !ignoreList.Contains(property.Name))
            {
                object val = property.GetValue(source, null);
                property.SetValue(target, val, null);
            }
        }
    }

    /// <summary>
    /// Returns a string representation of the objects property values
    /// </summary>
    /// <param name="source">The object for the string representation</param>
    /// <returns>A string</returns>
    public static string ToPropertiesString(this object source)
    {
        return ToPropertiesString(source, Environment.NewLine);
    }

    /// <summary>
    /// Returns a string representation of the objects property values
    /// </summary>
    /// <param name="source">The object for the string representation</param>
    /// <param name="delimiter">The line terminstor string to use between properties</param>
    /// <returns>A string</returns>
    public static string ToPropertiesString(this object source, string delimiter)
    {
        if (source == null)
        {
            return string.Empty;
        }

        Type type = source.GetType();

        var sb = new StringBuilder(type.Name);
        sb.Append(delimiter);

        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.CanWrite
                && property.CanRead)
            {
                object val = property.GetValue(source, null);
                sb.Append(property.Name);
                sb.Append(": ");
                sb.Append(val == null ? "[NULL]" : val.ToString());
                sb.Append(delimiter);
            }
        }

        return sb.ToString();
    }

   /// <summary>
    /// Serializes the object into an XML string, using the encoding method specified in
    /// <see cref="ExtensionMethodsSettings.DefaultEncoding"/>
    /// </summary>
    /// <remarks>
    /// The object to be serialized should be decorated with the 
    /// <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
    /// </remarks>
    /// <param name="source">The object to serialize</param>
    /// <returns>An XML encoded string representation of the source object</returns>
    public static string ToXml(this object source)
    {
        return ToXml(source, ExtensionMethodSetting.DefaultEncoding);
    }

    /// <summary>
    /// Serializes the object into an XML string
    /// </summary>
    /// <remarks>
    /// The object to be serialized should be decorated with the 
    /// <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
    /// </remarks>
    /// <param name="source">The object to serialize</param>
    /// <param name="encoding">The Encoding scheme to use when serializing the data to XML</param>
    /// <returns>An XML encoded string representation of the source object</returns>
    public static string ToXml(this object source, Encoding encoding)
    {
        if (source == null)
        {
            throw new ArgumentException("The source object cannot be null.");
        }

        if (encoding == null)
        {
            throw new Exception("You must specify an encoder to use for serialization.");
        }

        using (var stream = new MemoryStream())
        {
            var serializer = new XmlSerializer(source.GetType());
            serializer.Serialize(stream, source);
            stream.Position = 0;
            return encoding.GetString(stream.ToArray());
        }
    }

    /// <summary>
    /// Serializes the object into an XML string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static string ToXml<T>(this T @this)
    {
        if (@this == null) throw new NullReferenceException();

        XmlSerializer ser = new XmlSerializer(typeof(T));

        using (StringWriter writer = new StringWriter())
        {
            ser.Serialize(writer, @this);
            return writer.ToString();
        }
    }

    /// <summary>
    /// Throws an <see cref="System.ArgumentNullException"/> 
    /// if the the value is null.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="message">The message to display if the value is null.</param>
    /// <param name="name">The name of the parameter being tested.</param>
    public static void ExceptionIfNullOrEmpty(this object value, string message, string name)
    {
        if (value == null)
            throw new ArgumentNullException(name, message);
    }
    
}
