using System.Web.SessionState;

/// <summary>
///   Extensions classes for the ASP.NET Session State class
/// </summary>
public static class HttpSessionStateExtensions {
    /// <summary>
    ///   Returns a typed value from the ASP.NET session state
    /// </summary>
    /// <typeparam name = "T">The generic type to be returned</typeparam>
    /// <param name = "state">The session state.</param>
    /// <param name = "key">The session state key.</param>
    /// <returns>The session state value.</returns>
    /// <example>
    ///   <code>
    ///     public List&lt;string&gt; StringValues {
    ///     get { return this.Session.Ensure&lt;List&lt;string&gt;&gt;("StringValues"); }
    ///     set { this.ViewState.Set("StringValues", value); }
    ///     }
    ///   </code>
    /// </example>
    public static T Get<T>(this HttpSessionState state, string key) {
        return state.Get(key, default(T));
    }

    /// <summary>
    ///   Returns a typed value from the ASP.NET session state or the provided default value
    /// </summary>
    /// <typeparam name = "T">The generic type to be returned</typeparam>
    /// <param name = "state">The session state.</param>
    /// <param name = "key">The session state key.</param>
    /// <param name = "defaultValue">The default value to be returned.</param>
    /// <returns>The session state value.</returns>
    /// <example>
    ///   <code>
    ///     public List&lt;string&gt; StringValues {
    ///     get { return this.Session.Ensure&lt;List&lt;string&gt;&gt;("StringValues"); }
    ///     set { this.ViewState.Set("StringValues", value); }
    ///     }
    ///   </code>
    /// </example>
    public static T Get<T>(this HttpSessionState state, string key, T defaultValue) {
        var value = state[key];
        return (T) (value ?? defaultValue);
    }

    /// <summary>
    ///   Ensures a specific key to be either already in the ASP.NET session state or to be newly created
    /// </summary>
    /// <typeparam name = "T">The generic type to be returned</typeparam>
    /// <param name = "state">The session state.</param>
    /// <param name = "key">The session state key.</param>
    /// <returns>The session state value.</returns>
    /// <example>
    ///   <code>
    ///     public List&lt;string&gt; StringValues {
    ///     get { return this.Session.Ensure&lt;List&lt;string&gt;&gt;("StringValues"); }
    ///     set { this.ViewState.Set("StringValues", value); }
    ///     }
    ///   </code>
    /// </example>
    public static T Ensure<T>(this HttpSessionState state, string key) where T : class, new() {
        var value = state.Get<T>(key);
        if (value == null) {
            value = new T();
            state.Set(key, value);
        }

        return value;
    }

    /// <summary>
    ///   Sets the specified value into the ASP.NET session state.
    /// </summary>
    /// <param name = "state">The session state.</param>
    /// <param name = "key">The session state key.</param>
    /// <param name = "value">The new session state value.</param>
    /// <example>
    ///   <code>
    ///     public List&lt;string&gt; StringValues {
    ///     get { return this.Session.Ensure&lt;List&lt;string&gt;&gt;("StringValues"); }
    ///     set { this.ViewState.Set("StringValues", value); }
    ///     }
    ///   </code>
    /// </example>
    public static void Set(this HttpSessionState state, string key, object value) {
        state[key] = value;
    }
}