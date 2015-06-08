using System.Text;

/// <summary>
/// 	Extension methods for string[]
/// </summary>
public static class StringArrayExtensions
{
	/// <summary>
	/// 	Returns a combined value of strings from a string array
	/// </summary>
	/// <param name = "values">The values.</param>
	/// <param name = "prefix">The prefix.</param>
	/// <param name = "suffix">The suffix.</param>
	/// <param name = "quotation">The quotation (or null).</param>
	/// <param name = "separator">The separator.</param>
	/// <returns>
	/// 	A <see cref = "System.String" /> that represents this instance.
	/// </returns>
	/// <remarks>
	/// 	Contributed by blaumeister, http://www.codeplex.com/site/users/view/blaumeiser
	/// </remarks>
	public static string ToString(this string[] values, string prefix = "(", string suffix = ")", string quotation = "\"", string separator = ",")
	{
		var sb = new StringBuilder();
		sb.Append(prefix);

		for (var i = 0; i < values.Length; i++)
		{
			if (i > 0)
				sb.Append(separator);
			if (quotation != null)
				sb.Append(quotation);
			sb.Append(values[i]);
			if (quotation != null)
				sb.Append(quotation);
		}

		sb.Append(suffix);
		return sb.ToString();
	}
}
