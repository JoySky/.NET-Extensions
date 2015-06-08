using System;
using System.IO;
using System.IO.Compression;
using System.Net;

/// <summary>
///   Extension methods for HttpListenerContext
/// </summary>
public static class HttpListenerContextExtensions {
    /// <summary>
    ///   Prepares a response-stream using compression-, caching- and buffering-settings
    /// </summary>
    /// <param name = "context">The context</param>
    /// <param name = "allowZip">set to true in case you want to honor the compression-http-headers</param>
    /// <param name = "buffered">set to true in case you want a BufferedStream</param>
    /// <param name = "allowCache">set to false in case you want to set the no-cache-http-headers</param>
    /// <returns>the stream to write you stuff to</returns>
    /// <remarks>
    ///   Contributed by blaumeister, http://www.codeplex.com/site/users/view/blaumeiser
    /// </remarks>
    public static Stream GetResponseStream(this HttpListenerContext context, bool allowZip = true, bool buffered = true, bool allowCache = true) {
        var gzip = (context.Request.Headers["Accept-Encoding"] ?? String.Empty).Contains("gzip");
        var deflate = (context.Request.Headers["Accept-Encoding"] ?? String.Empty).Contains("deflate");

        if (!allowCache) {
            context.Response.AddHeader("Date", DateTime.UtcNow.ToString("R"));
            context.Response.AddHeader("Expires", DateTime.UtcNow.AddHours(-1).ToString("R"));
            context.Response.AddHeader("Cache-Control", "no-cache");
            context.Response.AddHeader("Pragma", "no-cache");
        }

        var stream = context.Response.OutputStream;

        if (allowZip) {
            context.Response.AddHeader("Vary", "Accept-Encoding");
            if (gzip) {
                stream = new GZipStream(stream, CompressionMode.Compress);
                context.Response.AddHeader("Content-Encoding", "gzip");
            }
            else if (deflate) {
                stream = new DeflateStream(stream, CompressionMode.Compress);
                context.Response.AddHeader("Content-Encoding", "deflate");
            }
        }

        if (buffered) stream = new BufferedStream(stream);

        return stream;
    }
}