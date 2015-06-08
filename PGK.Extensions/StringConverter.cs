using System;
using System.Runtime.InteropServices;
using System.Security;

/// <summary>
/// 	Universal conversion and parsing methods for strings.
/// 	These methods are avaiblable throught the generic object.ConvertTo method:
/// 	Feel free to provide additional converns for string or any other object type.
/// </summary>
/// <example>
/// 	<code>
/// 		var value = "123";
/// 		var numeric = value.ConvertTo().ToInt32();
/// 	</code>
/// </example>
public static class StringConverter
{
    /// <summary>
    /// 	Converts a string to an Int32 value
    /// </summary>
    /// <param name = "value">The value.</param>
    /// <returns></returns>
    /// <example>
    /// 	<code>
    /// 		var value = "123";
    /// 		var numeric = value.ConvertTo().ToInt32();
    /// 	</code>
    /// </example>
    public static int ToInt32(this IConverter<string> value)
    {
        return ToInt32(value, 0, false);
    }

    /// <summary>
    /// 	Converts a string to an Int32 value
    /// </summary>
    /// <param name = "value">The value.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <param name = "ignoreException">if set to <c>true</c> any parsing exception will be ignored.</param>
    /// <returns></returns>
    /// <example>
    /// 	<code>
    /// 		var value = "123";
    /// 		var numeric = value.ConvertTo().ToInt32();
    /// 	</code>
    /// </example>
    public static int ToInt32(this IConverter<string> value, int defaultValue, bool ignoreException)
    {
        if (ignoreException)
        {
            try
            {
                return ToInt32(value, defaultValue, false);
            }
            catch
            { }
            return defaultValue;
        }

        return int.Parse(value.Value);
    }


    /// <summary>
    ///     Converts a regular string into SecureString
    /// </summary>
    /// <param name="u">String value.</param>
    /// <param name="makeReadOnly">Makes the text value of this secure string read-only.</param>
    /// <returns>Returns a SecureString containing the value of a transformed object. </returns>
    public static SecureString ToSecureString(this string u, bool makeReadOnly = true)
    {
        if (u.IsNull()) { return null;  }

        SecureString s = new SecureString();
        
        foreach (char c in u) { s.AppendChar(c); }

        if (makeReadOnly) { s.MakeReadOnly(); }

        return s;
    }

    // TODO: Evaluate if this method needs to be in this .cs file, otherwise create the new one and move the method (public static string ToString(this SecureString s))

    /// <summary>
    ///     Coverts the SecureString to a regular string.
    /// </summary>
    /// <param name="s">Object value.</param>
    /// <returns>Content of secured string.</returns>
    public static string ToUnsecureString(this SecureString s)
    {
        if (s.IsNull())
            return null;

        IntPtr unmanagedString = IntPtr.Zero;

        try
        {
            unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(s);
            return Marshal.PtrToStringUni(unmanagedString);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        }
    }
}
