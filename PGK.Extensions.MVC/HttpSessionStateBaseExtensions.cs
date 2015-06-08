using System.Web;

/// <summary>
///   Extensions classes for the ASP.NET Session State class
/// </summary>
public static class HttpSessionStateBaseExtensions
{
	/// <summary>
	///   Returns a typed value from the ASP.NET MVC session state or the provided default value
	/// </summary>
	/// <typeparam name="TValue">The generic type to be returned</typeparam>
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
	/// <remarks>
	/// 	Contributed by Michael T, http://about.me/MichaelTran
	/// </remarks>
	public static TValue Get<TValue>(this HttpSessionStateBase state, string key, TValue defaultValue = default(TValue))
	{
		var value = state[key];
		return (TValue)(value ?? defaultValue);
	}

	/// <summary>
	///   Ensures a specific key to be either already in the ASP.NET MVC session state or to be newly created
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
	/// <remarks>
	/// 	Contributed by Michael T, http://about.me/MichaelTran
	/// </remarks>
	public static T Ensure<T>(this HttpSessionStateBase state, string key) where T : class, new()
	{
		var value = state.Get<T>(key);
		if (value == null)
		{
			value = new T();
			state.Set(key, value);
		}

		return value;
	}

	/// <summary>
	///   Sets the specified value into the ASP.NET MVC session state.
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
	/// <remarks>
	/// 	Contributed by Michael T, http://about.me/MichaelTran
	/// </remarks>
	public static void Set(this HttpSessionStateBase state, string key, object value)
	{
		state[key] = value;
	}
}