using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

/// <summary>
/// </summary>
public class UriQueryString {
    /// <summary>
    /// </summary>
    private readonly Dictionary<string, string> values = new Dictionary<string, string>();

    /// <summary>
    ///   Returns a <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
    /// </summary>
    /// <returns>
    ///   A <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
    /// </returns>
    public override string ToString() {
        return this.ToString((string) null);
    }

    /// <summary>
    ///   Returns a <see cref = "System.String" /> that represents this instance.
    /// </summary>
    /// <param name = "baseUrl">The base URL.</param>
    /// <returns>
    ///   A <see cref = "System.String" /> that represents this instance.
    /// </returns>
    public virtual string ToString(Uri baseUrl) {
        return this.ToString(baseUrl.ToString());
    }

    /// <summary>
    ///   Returns a <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
    /// </summary>
    /// <param name = "baseUrl">The base URL.</param>
    /// <returns>
    ///   A <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
    /// </returns>
    public virtual string ToString(string baseUrl) {
        var sb = new StringBuilder();

        foreach (var value in this.values) {
            if (sb.Length > 0) sb.Append("&");
            sb.AppendFormat("{0}={1}", value.Key, value.Value);
        }

        if (baseUrl.IsNotEmpty()) {
            return string.Concat(baseUrl, "?", sb.ToString());
        }

        return sb.ToString();
    }

    /// <summary>
    ///   Adds the specified key.
    /// </summary>
    /// <param name = "key">The key.</param>
    /// <param name = "value">The value.</param>
    public void Add(string key, string value) {
        values.Add(key, HttpUtility.UrlEncode(value));
    }
}