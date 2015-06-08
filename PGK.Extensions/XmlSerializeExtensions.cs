using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PGK.Extensions
{
    /// <summary>
    ///     Extension methods for the XmlSerialize
    /// </summary>
    public static class XmlSerializeExtensions
    {
        /// <summary>
        ///     Check that can Xml Serialize this instance or not.
        /// </summary>
        /// <typeparam name="T">Type of object to Xml Serialize.</typeparam>
        /// <param name="instance">An instance of object to Xml serialize.</param>
        /// <returns>If Xml serialize was possible, returns true, otherwise returns false.</returns>
        public static bool CanXmlSerialize<T>(this T instance) where T : class, new()
        {
            try
            {
                var stream = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, instance);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Check that can Xml Deserialize this file or not.
        /// </summary>
        /// <typeparam name="T">Type of object which serialized in file.</typeparam>
        /// <param name="filename">Name of file.</param>
        /// <returns>IF file can be deserialize returns true, otherwise return false.</returns>
        public static bool CanXmlDeserialize<T>(string filename) where T : class, new()
        {
            return CanXmlDeserialize<T>(new FileStream(filename, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        ///     Check that can Xml Deserialize this file or not.
        /// </summary>
        /// <typeparam name="T">Type of object which serialized in Stream.</typeparam>
        /// <param name="stream">Stream to deserialize.</param>
        /// <returns>IF Stream can be deserialize returns true, otherwise return false.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        public static bool CanXmlDeserialize<T>(this Stream stream) where T : class, new()
        {
            try
            {
                var reader = XmlReader.Create(stream);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return serializer.CanDeserialize(reader);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Xml Serialize an instance of specific type to stream.
        /// </summary>
        /// <typeparam name="T">Type of instance to Xml serialize.</typeparam>
        /// <param name="instance">Instance to Xml serialize.</param>
        /// <param name="stream">Stream to save Xml serialize in.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void XmlSerialize<T>(this T instance, Stream stream) where T : class, new()
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, instance);
            // return to begin of file.
            stream.Position = 0;
        }

        /// <summary>
        ///     Xml Serialize an instance of specific type to file.
        /// </summary>
        /// <typeparam name="T">Type of instance to Xml serialize.</typeparam>
        /// <param name="instance">Instance to Xml serialize.</param>
        /// <param name="filename">File name to create and store Xml serialized data.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void XmlSerialize<T>(this T instance, string filename) where T : class, new()
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(stream, instance);
            }
        }

        /// <summary>
        ///     Xml Deserialize this stream as object of <typeparamref name="T"/> from stream.
        /// </summary>
        /// <typeparam name="T">Type of instance to Xml Deserialize.</typeparam>
        /// <param name="stream">Stream to get Xml serialized data.</param>
        /// <returns>Returns an Xml Deserialized object.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static T XmlDeserialize<T>(this Stream stream) where T : class, new()
        {
            stream.Position = 0;
            var serializer = new XmlSerializer(typeof(T));            
            return serializer.Deserialize(stream) as T;            
        }
    }
}
