using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 	Extension methods for the TextReader class and its sub classes (StreamReader, StringReader)
/// </summary>
public static class TextReaderExtensions
{
	/// <summary>
	/// 	The method provides an iterator through all lines of the text reader.
	/// </summary>
	/// <param name = "reader">The text reader.</param>
	/// <returns>The iterator</returns>
	/// <example>
	/// 	<code>
	/// 		using(var reader = fileInfo.OpenText()) {
	/// 		foreach(var line in reader.IterateLines()) {
	/// 		// ...
	/// 		}
	/// 		}
	/// 	</code>
	/// </example>
	/// <remarks>
	/// 	Contributed by OlivierJ
	/// </remarks>
	public static IEnumerable<string> IterateLines(this TextReader reader)
	{
		string line = null;
		while ((line = reader.ReadLine()) != null)
			yield return line;
	}

	/// <summary>
	/// 	The method executes the passed delegate /lambda expression) for all lines of the text reader.
	/// </summary>
	/// <param name = "reader">The text reader.</param>
	/// <param name = "action">The action.</param>
	/// <example>
	/// 	<code>
	/// 		using(var reader = fileInfo.OpenText()) {
	/// 		reader.IterateLines(l => Console.WriteLine(l));
	/// 		}
	/// 	</code>
	/// </example>
	/// <remarks>
	/// 	Contributed by OlivierJ
	/// </remarks>
	public static void IterateLines(this TextReader reader, Action<string> action)
	{
		foreach (var line in reader.IterateLines())
			action(line);
	}
}
