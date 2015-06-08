using System;
using System.IO;
using System.Text;

/// <summary>
/// 	Extension methods any kind of streams
/// </summary>
public static class StreamExtensions
{
	/// <summary>
	/// 	Opens a StreamReader using the default encoding.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <returns>The stream reader</returns>
	public static StreamReader GetReader(this Stream stream)
	{
		return stream.GetReader(null);
	}

	/// <summary>
	/// 	Opens a StreamReader using the specified encoding.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <param name = "encoding">The encoding.</param>
	/// <returns>The stream reader</returns>
	public static StreamReader GetReader(this Stream stream, Encoding encoding)
	{
		if (stream.CanRead == false)
			throw new InvalidOperationException("Stream does not support reading.");

		encoding = (encoding ?? Encoding.Default);
		return new StreamReader(stream, encoding);
	}

	/// <summary>
	/// 	Opens a StreamWriter using the default encoding.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <returns>The stream writer</returns>
	public static StreamWriter GetWriter(this Stream stream)
	{
		return stream.GetWriter(null);
	}

	/// <summary>
	/// 	Opens a StreamWriter using the specified encoding.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <param name = "encoding">The encoding.</param>
	/// <returns>The stream writer</returns>
	public static StreamWriter GetWriter(this Stream stream, Encoding encoding)
	{
		if (stream.CanWrite == false)
			throw new InvalidOperationException("Stream does not support writing.");

		encoding = (encoding ?? Encoding.Default);
		return new StreamWriter(stream, encoding);
	}

	/// <summary>
	/// 	Reads all text from the stream using the default encoding.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <returns>The result string.</returns>
	public static string ReadToEnd(this Stream stream)
	{
		return stream.ReadToEnd(null);
	}

	/// <summary>
	/// 	Reads all text from the stream using a specified encoding.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <param name = "encoding">The encoding.</param>
	/// <returns>The result string.</returns>
	public static string ReadToEnd(this Stream stream, Encoding encoding)
	{
		using (var reader = stream.GetReader(encoding))
			return reader.ReadToEnd();
	}

	/// <summary>
	/// 	Sets the stream cursor to the beginning of the stream.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <returns>The stream</returns>
	public static Stream SeekToBegin(this Stream stream)
	{
		if (stream.CanSeek == false)
			throw new InvalidOperationException("Stream does not support seeking.");

		stream.Seek(0, SeekOrigin.Begin);
		return stream;
	}

	/// <summary>
	/// 	Sets the stream cursor to the end of the stream.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <returns>The stream</returns>
	public static Stream SeekToEnd(this Stream stream)
	{
		if (stream.CanSeek == false)
			throw new InvalidOperationException("Stream does not support seeking.");

		stream.Seek(0, SeekOrigin.End);
		return stream;
	}

	/// <summary>
	/// 	Copies one stream into a another one.
	/// </summary>
	/// <param name = "stream">The source stream.</param>
	/// <param name = "targetStream">The target stream.</param>
	/// <returns>The source stream.</returns>
	public static Stream CopyTo(this Stream stream, Stream targetStream)
	{
		return stream.CopyTo(targetStream, 4096);
	}

	/// <summary>
	/// 	Copies one stream into a another one.
	/// </summary>
	/// <param name = "stream">The source stream.</param>
	/// <param name = "targetStream">The target stream.</param>
	/// <param name = "bufferSize">The buffer size used to read / write.</param>
	/// <returns>The source stream.</returns>
	public static Stream CopyTo(this Stream stream, Stream targetStream, int bufferSize)
	{
		if (stream.CanRead == false)
			throw new InvalidOperationException("Source stream does not support reading.");
		if (targetStream.CanWrite == false)
			throw new InvalidOperationException("Target stream does not support writing.");

		var buffer = new byte[bufferSize];
		int bytesRead;

		while ((bytesRead = stream.Read(buffer, 0, bufferSize)) > 0)
			targetStream.Write(buffer, 0, bytesRead);
		return stream;
	}

	/// <summary>
	/// 	Copies any stream into a local MemoryStream
	/// </summary>
	/// <param name = "stream">The source stream.</param>
	/// <returns>The copied memory stream.</returns>
	public static MemoryStream CopyToMemory(this Stream stream)
	{
		var memoryStream = new MemoryStream((int) stream.Length);
		stream.CopyTo(memoryStream);
		return memoryStream;
	}

	/// <summary>
	/// 	Reads the entire stream and returns a byte array.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <returns>The byte array</returns>
	/// <remarks>
	/// 	Thanks to EsbenCarlsen  for providing an update to this method.
	/// </remarks>
	public static byte[] ReadAllBytes(this Stream stream)
	{
		using (var memoryStream = stream.CopyToMemory())
			return memoryStream.ToArray();
	}

	/// <summary>
	/// 	Reads a fixed number of bytes.
	/// </summary>
	/// <param name = "stream">The stream to read from</param>
	/// <param name = "bufsize">The number of bytes to read.</param>
	/// <returns>the read byte[]</returns>
	public static byte[] ReadFixedBuffersize(this Stream stream, int bufsize)
	{
		var buf = new byte[bufsize];
		int offset = 0, cnt;
		do
		{
			cnt = stream.Read(buf, offset, bufsize - offset);
			if (cnt == 0)
				return null;
			offset += cnt;
		} while (offset < bufsize);

		return buf;
	}

	/// <summary>
	/// 	Writes all passed bytes to the specified stream.
	/// </summary>
	/// <param name = "stream">The stream.</param>
	/// <param name = "bytes">The byte array / buffer.</param>
	public static void Write(this Stream stream, byte[] bytes)
	{
		stream.Write(bytes, 0, bytes.Length);
	}
}
