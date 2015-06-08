using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Xml;

[Flags]
public enum CharTypes
{
    Letters = 0x1,
    Digits = 0x2,
    XmlChar = 0x4,
    WhiteSpace = 0x8,


    LettersOrDigits = Letters | Digits

}

/// <summary>
/// 	Extension methods for the string data type
/// </summary>
public static partial class StringExtensions
{
	/// <summary>
	/// Returns the plural form of the specified word.
	/// </summary>
	/// <param name="singular">The singular string value</param>
	/// <param name="count">How many of the specified word there are. A count equal to 1 will not pluralize the specified word.</param>
	/// <param name="cultureInfo">Provide a culture info to pluralize (default to en-US)</param>
	/// <returns>A string that is the plural form of the input parameter.</returns>
	public static string ToPlural2(this string singular, int count = 0, CultureInfo cultureInfo = null)
	{
		return count == 1 ? singular : PluralizationService.CreateService(cultureInfo ?? new CultureInfo("en-US")).Pluralize(singular);
	}


    #region Extract
    
    /// <summary>
    /// Extracts characters that satisfy the given condition
    /// </summary>
    /// <param name="value"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static string Extract(this string value, Func<char, bool> condition)
    {
        if (value == null) throw new NullReferenceException();
        return new string(value.ToCharArray().Where(c => condition(c)).ToArray());
    }

    /// <summary>
    /// Extracts characters that exist within the given char array
    /// </summary>
    /// <param name="value"></param>
    /// <param name="chars"></param>
    /// <returns></returns>
    public static string Extract(this string value, params char[] chars)
    {
        return value.Extract(c => Array.IndexOf(chars, c) > -1);
    }

    /// <summary>
    /// Extracts characters that exist within the given char string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="chars"></param>
    /// <returns></returns>
    public static string Extract(this string value, string chars)
    {
        return value.Extract(chars.ToCharArray());
    }

    /// <summary>
    /// Extracts characters by character type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="charTypes"></param>
    /// <returns></returns>
    public static string Extract(this string value, CharTypes charTypes)
    {
        return value.Extract(CharFunc(charTypes));
    }

    /// <summary>
    /// Removes characters that satisfy the given condition
    /// </summary>
    /// <param name="value"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static string Remove(this string value, Func<char, bool> condition)
    {
        return value.Extract(v => !condition(v));
    }

    // conflicts with 3.5 versions
    //public static string Remove(this string value, params char[] chars)
    //{
    //    return value.Remove(c => Array.IndexOf(chars, c) > -1);
    //}

    //public static string Remove(this string value, string chars)
    //{
    //    return value.Remove(chars.ToCharArray());
    //}

    /// <summary>
    /// Removes characters by character type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="charTypes"></param>
    /// <returns></returns>
    public static string Remove(this string value, CharTypes charTypes)
    {
        return value.Remove(CharFunc(charTypes));
    }

    /// <summary>
    /// Builds a predicate from multiple character types
    /// </summary>
    /// <param name="charTypes"></param>
    /// <returns></returns>
    private static Func<char, bool> CharFunc(CharTypes charTypes)
    {
        var predicates = new Collection<Func<char, bool>>();

        // Letters or Digits
        if (charTypes.HasFlags(CharTypes.LettersOrDigits))
        {
            predicates.Add(char.IsLetterOrDigit);
        }
        else if (charTypes.HasFlags(CharTypes.Letters))
        {
            predicates.Add(char.IsLetter);
        }
        else if (charTypes.HasFlags(CharTypes.Digits))
        {
            predicates.Add(char.IsDigit);
        }

        // XML
        if (charTypes.HasFlags(CharTypes.XmlChar))
        {
            predicates.Add(XmlConvert.IsXmlChar);
        }

        // White space
        if (charTypes.HasFlags(CharTypes.WhiteSpace))
        {
            predicates.Add(char.IsWhiteSpace);
        }

        switch (predicates.Count)
        {
            case 1: return predicates[0];
            case 2: return c => predicates[0](c) || predicates[1](c);
            case 3: return c => predicates[0](c) || predicates[1](c) || predicates[2](c);
        }
        return null;

        // //unfortunately, this slows things way down - if anyone knows of a better
        // //way to combine predicates, please weigh in!
        //return c => predicates.Any(p => p(c));
    }

    #endregion

}