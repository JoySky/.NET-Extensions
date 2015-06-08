using System.Web.UI;

/// <summary>
///   Extensions classes for the ASP.NET ViewState StateBag class
/// </summary>
public static class StateBagExtensions {
    /// <summary>
    ///   Returns a typed value from the ASP.NET ViewState
    /// </summary>
    /// <typeparam name = "T">The generic type to be returned</typeparam>
    /// <param name = "state">The ViewState.</param>
    /// <param name = "key">The ViewState key.</param>
    /// <returns>The ViewState value.</returns>
    /// <example>
    ///   <code>
    ///     public string Text {
    ///     get { return this.ViewState.Get&lt;string&gt;("Text", "DefaultText"); }
    ///     set { this.ViewState.Set("Text", value); }
    ///     }
    ///   </code>
    /// </example>
    public static T Get<T>(this StateBag state, string key) {
        return state.Get(key, default(T));
    }

    /// <summary>
    ///   Returns a typed value from the ASP.NET ViewState or the provided default value
    /// </summary>
    /// <typeparam name = "T">The generic type to be returned</typeparam>
    /// <param name = "state">The ViewState.</param>
    /// <param name = "key">The ViewState key.</param>
    /// <param name = "defaultValue">The default value to be returned.</param>
    /// <returns>The ViewState value.</returns>
    /// <example>
    ///   <code>
    ///     public string Text {
    ///     get { return this.ViewState.Get&lt;string&gt;("Text", "DefaultText"); }
    ///     set { this.ViewState.Set("Text", value); }
    ///     }
    ///   </code>
    /// </example>
    public static T Get<T>(this StateBag state, string key, T defaultValue) {
        var value = state[key];
        return (T) (value ?? defaultValue);
    }

    /// <summary>
    ///   Ensures a specific key to be either already in the ASP.NET ViewState or to be newly created
    /// </summary>
    /// <typeparam name = "T">The generic type to be returned</typeparam>
    /// <param name = "state">The ViewState.</param>
    /// <param name = "key">The ViewState key.</param>
    /// <returns>The ViewState value.</returns>
    /// <example>
    ///   <code>
    ///     public string Text {
    ///     get { return this.ViewState.Get&lt;string&gt;("Text", "DefaultText"); }
    ///     set { this.ViewState.Set("Text", value); }
    ///     }
    ///   </code>
    /// </example>
    public static T Ensure<T>(this StateBag state, string key) where T : class, new() {
        var value = state.Get<T>(key);
        if (value == null) {
            value = new T();
            state.Set(key, value);
        }

        return value;
    }

    /// <summary>
    ///   Sets the specified value into the ASP.NET ViewState.
    /// </summary>
    /// <param name = "state">The ViewState.</param>
    /// <param name = "key">The ViewState key.</param>
    /// <param name = "value">The new ViewState value.</param>
    /// <example>
    ///   <code>
    ///     public string Text {
    ///     get { return this.ViewState.Get&lt;string&gt;("Text", "DefaultText"); }
    ///     set { this.ViewState.Set("Text", value); }
    ///     }
    ///   </code>
    /// </example>
    public static void Set(this StateBag state, string key, object value) {
        state[key] = value;
    }
}