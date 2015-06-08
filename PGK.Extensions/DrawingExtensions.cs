using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace PGK.Extensions
{
    /// <summary>
    /// Extension methods for the System.Drawing class
    /// </summary>
    public static class DrawingExtensions
    {
        /// <summary>
        /// Split an Icon (that contains multiple icons) into an array of Icon each rapresenting a single icons.
        /// </summary>
        /// <param name="icon">Instance value.</param>
        /// <returns>An array of <see cref="System.Drawing.Icon"/> objects.</returns>
        public static Icon[] SplitIcon(this Icon icon)
        {
            if (icon == null)
            {
                throw new ArgumentNullException("Can't split the icon. Icon is null.");
            }

            // Get multiple .ico file image.
            byte[] srcBuf = null;
            using (MemoryStream stream = new MemoryStream())
            {
                icon.Save(stream);
                srcBuf = stream.ToArray();
            }

            List<Icon> splitIcons = new List<Icon>();
            {
                const int sICONDIR = 6;            // sizeof(ICONDIR) 
                const int sICONDIRENTRY = 16;      // sizeof(ICONDIRENTRY)

                int count = BitConverter.ToInt16(srcBuf, 4); // ICONDIR.idCount

                for (int i = 0; i < count; i++)
                {
                    using (MemoryStream destStream = new MemoryStream())
                    using (BinaryWriter writer = new BinaryWriter(destStream))
                    {
                        // Copy ICONDIR and ICONDIRENTRY.
                        writer.Write(srcBuf, 0, sICONDIR - 2);
                        writer.Write((short)1);    // ICONDIR.idCount == 1;

                        writer.Write(srcBuf, sICONDIR + sICONDIRENTRY * i, sICONDIRENTRY - 4);
                        writer.Write(sICONDIR + sICONDIRENTRY);    // ICONDIRENTRY.dwImageOffset = sizeof(ICONDIR) + sizeof(ICONDIRENTRY)

                        // Copy picture and mask data.
                        int imgSize = BitConverter.ToInt32(srcBuf, sICONDIR + sICONDIRENTRY * i + 8);       // ICONDIRENTRY.dwBytesInRes
                        int imgOffset = BitConverter.ToInt32(srcBuf, sICONDIR + sICONDIRENTRY * i + 12);    // ICONDIRENTRY.dwImageOffset
                        writer.Write(srcBuf, imgOffset, imgSize);

                        // Create new icon.
                        destStream.Seek(0, SeekOrigin.Begin);
                        splitIcons.Add(new Icon(destStream));
                    }
                }
            }

            return splitIcons.ToArray();
        }

        /// <summary>
        /// Serializes the image in an byte array
        /// </summary>
        /// <param name="image">Instance value.</param>
        /// <param name="format">Specifies the format of the image.</param>
        /// <returns>The image serialized as byte array.</returns>
        public static byte[] ToBytes(this Image image, ImageFormat format)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (format == null)
                throw new ArgumentNullException("format");

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, format);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Gets the bounds of the image in pixels
        /// </summary>
        /// <param name="image">Instance value.</param>
        /// <returns>A rectangle that has the same hight and width as given image.</returns>
        public static Rectangle GetBounds(this Image image)
        {
            return new Rectangle(0, 0, image.Width, image.Height);
        }

        /// <summary>
        /// Gets the rectangle that sorrounds the given point by a specified distance.
        /// </summary>
        /// <param name="p">Instance value.</param>
        /// <param name="distance">Distance that will be used to surround the point.</param>
        /// <returns>Rectangle that sorrounds the given point by a specified distance.</returns>
        public static Rectangle Surround(this Point p, int distance)
        {
            return new Rectangle(p.X - distance, p.Y - distance, distance * 2, distance * 2);
        }

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
            using (Graphics g = Graphics.FromImage(scaledBitmap))
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
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }
            else if (height.Equals(0))
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else if (((float)width) / bitmap.Size.Width * bitmap.Size.Height <= height)
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }

            return bitmap.ScaleToSize((int)proportionalWidth, (int)proportionalHeight);
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
            using (Graphics g = Graphics.FromImage(scaledBitmap))
            {
                g.Clear(backgroundColor);

                Bitmap proportionalBitmap = bitmap.ScaleProportional(width, height);

                var imagePosition = new Point((int)((width - proportionalBitmap.Width) / 2m),
                                              (int)((height - proportionalBitmap.Height) / 2m));
                g.DrawImage(proportionalBitmap, imagePosition);
            }

            return scaledBitmap;
        }

        /// <summary>
        /// Gets the Image as a Byte[]
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="format">ImageFormat</param>
        /// <returns>A Byte[] of the Image</returns>
        /// <remarks></remarks>
        public static byte[] GetImageInBytes(this Image img, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                if (format != null)
                {
                    img.Save(ms, format);
                    return ms.ToArray();
                }
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets the Image in Base64 format for storage or transfer
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="format">ImageFormat</param>
        /// <returns>Base64 String of the Image</returns>
        /// <remarks></remarks>
        public static string GetImageInBase64(this Image img, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                if (format != null)
                {
                    img.Save(ms, format);
                    return Convert.ToBase64String(ms.ToArray());
                }
                img.Save(ms, img.RawFormat);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// Scales the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="height">The height as int.</param>
        /// <param name="width">The width as int.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Image ScaleImage(this Image img, int height, int width)
        {
            if (img == null || height <= 0 || width <= 0)
            {
                return null;
            }
            int newWidth = (img.Width * height) / (img.Height);
            int newHeight = (img.Height * width) / (img.Width);
            int x, y;

            var bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            // use this when debugging.
            //g.FillRectangle(Brushes.Aqua, 0, 0, bmp.Width - 1, bmp.Height - 1);
            if (newWidth > width)
            {
                // use new height
                x = (bmp.Width - width) / 2;
                y = (bmp.Height - newHeight) / 2;
                g.DrawImage(img, x, y, width, newHeight);
            }
            else
            {
                // use new width
                x = (bmp.Width / 2) - (newWidth / 2);
                y = (bmp.Height / 2) - (height / 2);
                g.DrawImage(img, x, y, newWidth, height);
            }
            // use this when debugging.
            //g.DrawRectangle(new Pen(Color.Red, 1), 0, 0, bmp.Width - 1, bmp.Height - 1);
            return bmp;
        }

        /// <summary>
        /// Resizes the image to fit new size.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="newSize">The new size.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Image ResizeAndFit(this Image image, Size newSize)
        {
            bool sourceIsLandscape = image.Width > image.Height;
            bool targetIsLandscape = newSize.Width > newSize.Height;

            double ratioWidth = newSize.Width / (double)image.Width;
            double ratioHeight = newSize.Height / (double)image.Height;

            double ratio;

            if (ratioWidth > ratioHeight && sourceIsLandscape == targetIsLandscape)
                ratio = ratioWidth;
            else
                ratio = ratioHeight;

            var targetWidth = (int)(image.Width * ratio);
            var targetHeight = (int)(image.Height * ratio);

            var bitmap = new Bitmap(newSize.Width, newSize.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            double offsetX = ((double)(newSize.Width - targetWidth)) / 2;
            double offsetY = ((double)(newSize.Height - targetHeight)) / 2;

            graphics.DrawImage(image, (int)offsetX, (int)offsetY, targetWidth, targetHeight);
            graphics.Dispose();

            return bitmap;
        }

        /// <summary>
        /// Converts to image.
        /// </summary>
        /// <param name="byteArrayIn">The byte array in.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Image ConvertToImage(this byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
