using System;

namespace PGK.Extensions
{
    /// <summary>
    /// TimeSpan extensions
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        ///     Multiply a <c>System.TimeSpan</c> by a <paramref name="factor"/>
        /// </summary>
        /// <param name="source">The given <c>System.TimeSpan</c> to be multiplied</param>
        /// <param name="factor">The multiplier factor</param>
        /// <returns>The multiplication of the <paramref name="source"/> by <paramref name="factor"/></returns>
        public static TimeSpan MultiplyBy(this TimeSpan source, int factor)
        {
            TimeSpan result = TimeSpan.FromTicks(source.Ticks*factor);
            return result;
        }
        
        /// <summary>
        ///     Multiply a <c>System.TimeSpan</c> by a <paramref name="factor"/>
        /// </summary>
        /// <param name="source">The given <c>System.TimeSpan</c> to be multiplied</param>
        /// <param name="factor">The multiplier factor</param>
        /// <returns>The multiplication of the <paramref name="source"/> by <paramref name="factor"/></returns>
        public static TimeSpan MultiplyBy(this TimeSpan source, double factor)
        {
            TimeSpan result = TimeSpan.FromTicks((long)(source.Ticks*factor));
            return result;
        }
    }
}