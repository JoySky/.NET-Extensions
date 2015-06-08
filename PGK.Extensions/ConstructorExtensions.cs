using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

public static class ConstructorExtensions
{
	/// <summary>
	/// Auto initialize an object source with an object data for all public accessible properties.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The source object to have its' properties initialized.</param>
	/// <param name="data">The data object used to initialize the source object.</param>
	/// <remarks>
	/// Contributed by Tri Tran, http://about.me/triqtran
	/// </remarks>
	public static T AutoInitialize<T>(this T source, object data)
	{
		const BindingFlags flags =
				BindingFlags.Instance |
				BindingFlags.Public |
				BindingFlags.GetProperty |
				BindingFlags.SetProperty;

		return source.AutoInitialize(data, flags); ;
	}

	/// <summary>
	/// Auto Initialize an object source with a object data for all properties specified by the binding flags.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The source object to have its' properties initialized.</param>
	/// <param name="data">The data object used to initialize the source object.</param>
	/// <param name="flags">The binding flags used for property accessability. See <see cref="BindingFlags"/></param>
	/// <remarks>
	/// Contributed by Tri Tran, http://about.me/triqtran
	/// </remarks>
	public static T AutoInitialize<T>(this T source, object data, BindingFlags flags)
	{
		// Check if data is not null
		if (data == null) return source;
		// Check that data is the same type as source
		if (data.GetType() != source.GetType()) return source;

		// Get all the public - instace properties that contains both getter and setter.
		PropertyInfo[] properties = source.GetType().GetProperties(flags);

		// For each property, set the value to the source from the data.
		foreach (PropertyInfo property in properties)
		{
			// Identify the type of this property.
			Type propertyType = property.PropertyType;
			try
			{
				// Retreive the value given the property name.
				object objectValue = property.GetValue(data, null);
				if (objectValue != null)
				{
					// If the object value is already of the property type
					if (objectValue.GetType().Equals(propertyType))
					{
						// Set the object value to the source
						property.SetValue(source, objectValue, null);
					}
					else
					{
						// Otherwise convert the object value using the property's converter
						TypeConverter converter = TypeDescriptor.GetConverter(propertyType);
						if (converter != null)
						{
							// Convert the object value.
							object convertedData = converter.ConvertFrom(objectValue);
							// Check that the converted data is of the same type as the property type
							if (convertedData.GetType() == propertyType)
							{
								// If it is, then set the converted data to the source object.
								property.SetValue(source, convertedData, null);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				// Exception during operations
				Debug.WriteLine(e.Message);
			}
		}

		return source;
	}

	/// <summary>
	/// Auto initialize all public properties of the source object with data from a data row.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The object to be auto initialized.</param>
	/// <param name="data">The data row containing the data for initializing.</param>
	/// <remarks>
	/// Contributed by Tri Tran, http://about.me/triqtran
	/// </remarks>
	public static void AutoInitialize<T>(this T source, DataRow data)
	{
		const BindingFlags flags =
				BindingFlags.Instance |
				BindingFlags.Public |
				BindingFlags.SetProperty;
		source.AutoInitialize(data, flags);
	}

	/// <summary>
	/// Auto initialize all properties specified by the binding flags of the source object with data from a data row.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The object to be auto initialized.</param>
	/// <param name="row">The data row containing the data for initializing.</param>
	/// <param name="flags">The binding flags used for property accessability. See <see cref="BindingFlags"/></param>
	/// <remarks>
	/// Contributed by Tri Tran, http://about.me/triqtran
	/// </remarks>
	public static void AutoInitialize<T>(this T source, DataRow row, BindingFlags flags)
	{
		if (row == null) return;

		// Get all the public - instace properties that contains a setter.
		PropertyInfo[] properties = source.GetType().GetProperties(flags);
		foreach (PropertyInfo property in properties)
		{
			// Get the column name from the column or use the property name.
			string columnName = property.Name;
			// Get the property type.
			Type propertyType = property.PropertyType;
			try
			{
				// Retreive the row value given the column name.
				object rowValue = row[columnName];
				// Determin that the row value is not null (DBNull)
				if (rowValue != Convert.DBNull)
				{
					// Get the converter for this property.
					TypeConverter converter = TypeDescriptor.GetConverter(propertyType);
					if (converter != null)
					{
						// Convert the row value to the property type.
						object data = converter.ConvertFrom(rowValue);
						// If the converted type matches the property type, then set the data to the source.
						if (data.GetType() == propertyType) property.SetValue(source, data, null);
					}
				}
			}
			catch (Exception e)
			{
				// Exception during operation
				// Most likely that the row does not contain the property name.
				Debug.WriteLine(e.Message);
			}
		}
	}

	/// <summary>
	/// Auto initialize all public properties of the source object with data from a data row.
	/// </summary>
	/// <typeparam name="T">
	/// </typeparam>
	/// <param name="source">
	/// The object to be auto initialized.
	/// </param>
	/// <param name="data">
	/// The data row containing the data for initializing.
	/// </param>
	/// <param name="columns">
	/// The columns.
	/// </param>
	/// <remarks>
	/// Contributed by Tri Tran, http://about.me/triqtran
	/// </remarks>
	public static void AutoInitialize<T>(this T source, DataRow data, params Expression<Func<T, object>>[] columns)
	{
		const BindingFlags flags =
				BindingFlags.Instance |
				BindingFlags.Public |
				BindingFlags.SetProperty;
		source.AutoInitialize(data, flags, columns);
	}

	/// <summary>
	/// Auto initialize all properties specified by the binding flags of the source object with data from a data row.
	/// </summary>
	/// <typeparam name="T">The type of the source.</typeparam>
	/// <param name="source">The object to be auto initialized.</param>
	/// <param name="row">The data row containing the data for initializing.</param>
	/// <param name="flags">The binding flags used for property accessability. See <see cref="BindingFlags"/></param>
	/// <param name="columns">An expression to specify the columns.</param>
	/// <remarks>
	/// Contributed by Tri Tran, http://about.me/triqtran
	/// </remarks>
	public static void AutoInitialize<T>
			(this T source, DataRow row, BindingFlags flags, params Expression<Func<T, object>>[] columns)
	{
		if (row == null || columns == null) return;
		// Get all the public - instace properties that contains a setter.
		Type sourceType = source.GetType();

		for (int i = 0; i < columns.Length; i++)
		{
			// Get the column name from the column or use the property name.
			string columnName = GetProperty(columns[i]).Name;

			// Get the property given the column name.
			PropertyInfo property = sourceType.GetProperty(columnName, flags);
			// If the property is found.
			if (property != null)
			{
				// Get the property type.
				Type propertyType = property.PropertyType;

				// Retreive the row value given the column name.
				object rowValue = row[columnName];
				// Determin that the row value is not null (DBNull)
				if (rowValue != Convert.DBNull)
				{
					// Get the converter for this property.
					TypeConverter converter = TypeDescriptor.GetConverter(propertyType);
					if (converter != null)
					{
						// Convert the row value to the property type.
						object data = converter.ConvertFrom(rowValue);
						// If the converted type matches the property type, then set the data to the source.
						if (data.GetType() == propertyType) property.SetValue(source, data, null);
					}
				}
			}
		}
	}

	/// <summary>
	/// Get the property info from the property expression.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="propertyExpression"></param>
	/// <returns></returns>
	/// <remarks>
	/// Contributed by Tri Tran, http://about.me/triqtran
	/// </remarks>
	public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> propertyExpression)
	{
		var lambda = propertyExpression as LambdaExpression;
		MemberExpression memberExpression;
		if (lambda.Body is UnaryExpression)
		{
			var unaryExpression = lambda.Body as UnaryExpression;
			memberExpression = (MemberExpression)unaryExpression.Operand;
		}
		else
		{
			memberExpression = (MemberExpression)lambda.Body;
		}

		return memberExpression.Member as PropertyInfo;
	}
}