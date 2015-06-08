using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///   Extension methods for all kinds of exceptions.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    ///   Gets the original exception which is most inner exception.
    /// </summary>
    /// <param name = "exception">The exeption</param>
    /// <returns>The original exception</returns>
    /// <remarks>
    ///   Contributed by Kenneth Scott
    /// </remarks>
    [Obsolete("Use GetBaseException instead")]
    public static Exception GetOriginalException(this Exception exception)
    {
        if (exception.InnerException == null) return exception;

        return exception.InnerException.GetOriginalException();
    }

    ///<summary>
    /// Gets all the error messages
    ///</summary>
    ///<param name="exception">The exception</param>
    ///<returns>IEnumerable of message</returns>
    /// <remarks>
    /// 	Contributed by Michael T, http://about.me/MichaelTran
    /// </remarks>
    /// <note>
    /// The most inner exception message is first in the list, and the most outer exception message is last in the list
    /// </note>
    public static IEnumerable<string> Messages(this Exception exception)
    {
        return exception != null ?
                new List<string>(exception.InnerException.Messages()) { exception.Message } : Enumerable.Empty<string>();
    }

    ///<summary>
    /// Gets all the errors
    ///</summary>
    ///<param name="exception">The exception</param>
    ///<returns>IEnumerable of message</returns>
    /// <remarks>
    /// 	Contributed by Michael T, http://about.me/MichaelTran
    /// </remarks>
    /// <note>
    /// The most inner exception is first in the list, and the most outer exception is last in the list
    /// </note>
    public static IEnumerable<Exception> Exceptions(this Exception exception)
    {
        return exception != null ?
                new List<Exception>(exception.InnerException.Exceptions()) { exception } : Enumerable.Empty<Exception>();
    }

}