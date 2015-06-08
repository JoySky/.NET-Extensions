using System;
using System.Text;

namespace PGK.Extensions
{
    /// <summary>
    /// Extensions for StringBuilder
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// AppendLine version with format string parameters.
        /// </summary>
        public static void AppendLine(this StringBuilder builder, string value, params Object[] parameters)
        {
            builder.AppendLine(string.Format(value, parameters));
        }

        /// <summary>
        /// Appends the value of the object's System.Object.ToString() method followed by the default line terminator to the end of the current
        /// System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="this"></param>
        /// <param name="condition">The conditional expression to evaluate.</param>
        /// <param name="value"></param>
        public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, object value)
        {
            if (condition) sb.AppendLine(value.ToString());
            return sb;
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains zero or more format items, followed by the default
        /// line terminator to the end of the current System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="this"></param>
        /// <param name="condition">The conditional expression to evaluate.</param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, string format, params object[] args)
        {
            if (condition) sb.AppendFormat(format, args).AppendLine();
            return sb;
        }

        /// <summary>
        /// Appends the value of the object's System.Object.ToString() method to the end of the current
        /// System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="this"></param>
        /// <param name="condition"></param>
        /// <param name="value"></param>
        public static StringBuilder AppendIf(this StringBuilder sb, bool condition, object value)
        {
            if (condition) sb.Append(value.ToString());
            return sb;
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains zero or more format items, 
        /// to the end of the current System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="this"></param>
        /// <param name="condition"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static StringBuilder AppendFormatIf(this StringBuilder sb, bool condition, string format, params object[] args)
        {
            if (condition) sb.AppendFormat(format, args);
            return sb;
        }
    }
}
