using System.Drawing;
using System.Drawing.Drawing2D;

/// <summary>
/// 	Extension methods for the System.Drawing.Bitmap class
/// </summary>
public static class BitmapExtensions
{
	/// <summary>
	/// 	Scales the bitmap to the passed target size without respecting the aspect.
	/// </summary>
	/// <param name = "bitmap">The source bitmap.</param>
	/// <param name = "size">The target size.</param>
	/// <returns>The scaled bitmap</returns>
	/// <example>
	/// 	<code>
	/// 		var bitmap = new Bitmap("image.png");
	/// 		var thumbnail = bitmap.ScaleToSize(100, 100);
	/// 	</code>
	/// </example>
	public static Bitmap ScaleToSize(this Bitmap bitmap, Size size)
	{
		return bitmap.ScaleToSize(size.Width, size.Height);
	}

	/// <summary>
	/// 	Scales the bitmap to the passed target size without respecting the aspect.
	/// </summary>
	/// <param name = "bitmap">The source bitmap.</param>
	/// <param name = "width">The target width.</param>
	/// <param name = "height">The target height.</param>
	/// <returns>The scaled bitmap</returns>
	/// <example>
	/// 	<code>
	/// 		var bitmap = new Bitmap("image.png");
	/// 		var thumbnail = bitmap.ScaleToSize(100, 100);
	/// 	</code>
	/// </example>
	public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
	{
		var scaledBitmap = new Bitmap(width, height);
		using (var g = Graphics.FromImage(scaledBitmap))
		{
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(bitmap, 0, 0, width, height);
		}
		return scaledBitmap;
	}

	/// <summary>
	/// 	Scales the bitmap to the passed target size by respecting the aspect.
	/// </summary>
	/// <param name = "bitmap">The source bitmap.</param>
	/// <param name = "size">The target size.</param>
	/// <returns>The scaled bitmap</returns>
	/// <example>
	/// 	<code>
	/// 		var bitmap = new Bitmap("image.png");
	/// 		var thumbnail = bitmap.ScaleProportional(100, 100);
	/// 	</code>
	/// </example>
	/// <remarks>
	/// 	Please keep in mind that the returned bitmaps size might not match the desired size due to original bitmaps aspect.
	/// </remarks>
	public static Bitmap ScaleProportional(this Bitmap bitmap, Size size)
	{
		return bitmap.ScaleProportional(size.Width, size.Height);
	}

	/// <summary>
	/// 	Scales the bitmap to the passed target size by respecting the aspect.
	/// </summary>
	/// <param name = "bitmap">The source bitmap.</param>
	/// <param name = "width">The target width.</param>
	/// <param name = "height">The target height.</param>
	/// <returns>The scaled bitmap</returns>
	/// <example>
	/// 	<code>
	/// 		var bitmap = new Bitmap("image.png");
	/// 		var thumbnail = bitmap.ScaleProportional(100, 100);
	/// 	</code>
	/// </example>
	/// <remarks>
	/// 	Please keep in mind that the returned bitmaps size might not match the desired size due to original bitmaps aspect.
	/// </remarks>
	public static Bitmap ScaleProportional(this Bitmap bitmap, int width, int height)
	{
		float proportionalWidth, proportionalHeight;

		if (width.Equals(0))
		{
			proportionalWidth = ((float) height)/bitmap.Size.Height*bitmap.Width;
			proportionalHeight = height;
		}
		else if (height.Equals(0))
		{
			proportionalWidth = width;
			proportionalHeight = ((float) width)/bitmap.Size.Width*bitmap.Height;
		}
		else if (((float) width)/bitmap.Size.Width*bitmap.Size.Height <= height)
		{
			proportionalWidth = width;
			proportionalHeight = ((float) width)/bitmap.Size.Width*bitmap.Height;
		}
		else
		{
			proportionalWidth = ((float) height)/bitmap.Size.Height*bitmap.Width;
			proportionalHeight = height;
		}

		return bitmap.ScaleToSize((int) proportionalWidth, (int) proportionalHeight);
	}

	/// <summary>
	/// 	Scales the bitmap to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
	/// </summary>
	/// <param name = "bitmap">The source bitmap.</param>
	/// <param name = "size">The target size.</param>
	/// <returns>The scaled bitmap</returns>
	/// <example>
	/// 	<code>
	/// 		var bitmap = new Bitmap("image.png");
	/// 		var thumbnail = bitmap.ScaleToSizeProportional(100, 100);
	/// 	</code>
	/// </example>
	public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, Size size)
	{
		return bitmap.ScaleToSizeProportional(Color.White, size);
	}

	/// <summary>
	/// 	Scales the bitmap to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
	/// </summary>
	/// <param name = "bitmap">The source bitmap.</param>
	/// <param name = "backgroundColor">The color of the background.</param>
	/// <param name = "size">The target size.</param>
	/// <returns>The scaled bitmap</returns>
	/// <example>
	/// 	<code>
	/// 		var bitmap = new Bitmap("image.png");
	/// 		var thumbnail = bitmap.ScaleToSizeProportional(100, 100);
	/// 	</code>
	/// </example>
	public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, Color backgroundColor, Size size)
	{
		return bitmap.ScaleToSizeProportional(backgroundColor, size.Width, size.Height);
	}

	/// <summary>
	/// 	Scales the bitmap to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
	/// </summary>
	/// <param name = "bitmap">The source bitmap.</param>
	/// <param name = "width">The target width.</param>
	/// <param name = "height">The target height.</param>
	/// <returns>The scaled bitmap</returns>
	/// <example>
	/// 	<code>
	/// 		var bitmap = new Bitmap("image.png");
	/// 		var thumbnail = bitmap.ScaleToSizeProportional(100, 100);
	/// 	</code>
	/// </example>
	public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, int width, int height)
	{
		return bitmap.ScaleToSizeProportional(Color.White, width, height);
	}

	/// <summary>
	/// 	Scales the bitmap to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
	/// </summary>
	/// <param name = "bitmap">The source bitmap.</param>
	/// <param name = "backgroundColor">The color of the background.</param>
	/// <param name = "width">The target width.</param>
	/// <param name = "height">The target height.</param>
	/// <returns>The scaled bitmap</returns>
	/// <example>
	/// 	<code>
	/// 		var bitmap = new Bitmap("image.png");
	/// 		var thumbnail = bitmap.ScaleToSizeProportional(100, 100);
	/// 	</code>
	/// </example>
	public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, Color backgroundColor, int width, int height)
	{
		var scaledBitmap = new Bitmap(width, height);
		using (var g = Graphics.FromImage(scaledBitmap))
		{
			g.Clear(backgroundColor);

			var proportionalBitmap = bitmap.ScaleProportional(width, height);

			var imagePosition = new Point((int) ((width - proportionalBitmap.Width)/2m), (int) ((height - proportionalBitmap.Height)/2m));
			g.DrawImage(proportionalBitmap, imagePosition);
		}

		return scaledBitmap;
	}
}
