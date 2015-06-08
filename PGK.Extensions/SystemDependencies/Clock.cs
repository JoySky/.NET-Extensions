using System;

namespace PGK.Extensions.SystemDependencies
{
    internal static class Clock
    {
        /// <summary>
        /// Set a substitute (and fix) value for Now.  See <see cref="SubstituteForSystemDate"/>
        /// for usage example.
        /// </summary>
        public static DateTime? SubstituteForNow;

        public static DateTime Now { 
            get { return (SubstituteForNow ?? DateTime.Now); }
        }
    }
}
