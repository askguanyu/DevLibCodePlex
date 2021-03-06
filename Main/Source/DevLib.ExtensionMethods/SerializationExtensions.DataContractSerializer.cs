﻿//-----------------------------------------------------------------------
// <copyright file="SerializationExtensions.DataContractSerializer.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.ExtensionMethods
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Serialization Extensions.
    /// </summary>
    public static partial class SerializationExtensions
    {
        /// <summary>
        /// Serializes DataContract object to Xml string.
        /// </summary>
        /// <param name="source">The DataContract object to serialize.</param>
        /// <param name="indent">Whether to write individual elements on new lines and indent.</param>
        /// <param name="omitXmlDeclaration">Whether to write an Xml declaration.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array that may be present in the object graph.</param>
        /// <returns>An Xml encoded string representation of the source DataContract object.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Reviewed.")]
        public static string SerializeDataContractXmlString(this object source, bool indent = false, bool omitXmlDeclaration = true, Type[] knownTypes = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            DataContractSerializer dataContractSerializer = (knownTypes == null || knownTypes.Length == 0) ? new DataContractSerializer(source.GetType()) : new DataContractSerializer(source.GetType(), knownTypes);

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader streamReader = new StreamReader(memoryStream))
            using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings() { OmitXmlDeclaration = omitXmlDeclaration, Indent = indent, Encoding = new UTF8Encoding(false), CloseOutput = true }))
            {
                dataContractSerializer.WriteObject(xmlWriter, source);
                xmlWriter.Flush();
                memoryStream.Position = 0;
                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Serializes DataContract object to Xml string, write to file.
        /// </summary>
        /// <param name="source">The DataContract object to serialize.</param>
        /// <param name="filename">File name.</param>
        /// <param name="overwrite">Whether overwrite exists file.</param>
        /// <param name="indent">Whether to write individual elements on new lines and indent.</param>
        /// <param name="omitXmlDeclaration">Whether to write an Xml declaration.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array that may be present in the object graph.</param>
        /// <returns>File full path.</returns>
        public static string WriteDataContractXml(this object source, string filename, bool overwrite = false, bool indent = true, bool omitXmlDeclaration = true, Type[] knownTypes = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            string fullPath = Path.GetFullPath(filename);
            string fullDirectoryPath = Path.GetDirectoryName(fullPath);

            if (!overwrite && File.Exists(filename))
            {
                throw new ArgumentException("The specified file already exists.", fullPath);
            }

            if (!Directory.Exists(fullDirectoryPath))
            {
                try
                {
                    Directory.CreateDirectory(fullDirectoryPath);
                }
                catch
                {
                    throw;
                }
            }

            DataContractSerializer dataContractSerializer = (knownTypes == null || knownTypes.Length == 0) ? new DataContractSerializer(source.GetType()) : new DataContractSerializer(source.GetType(), knownTypes);

            using (XmlWriter xmlWriter = XmlWriter.Create(fullPath, new XmlWriterSettings() { OmitXmlDeclaration = omitXmlDeclaration, Indent = indent, Encoding = new UTF8Encoding(false), CloseOutput = true }))
            {
                dataContractSerializer.WriteObject(xmlWriter, source);
                xmlWriter.Flush();
                return fullPath;
            }
        }

        /// <summary>
        /// Deserializes DataContract Xml string to object.
        /// </summary>
        /// <param name="source">The DataContract Xml string to deserialize.</param>
        /// <param name="type">Type of DataContract object.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array that may be present in the object graph.</param>
        /// <returns>Instance of DataContract object.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed.")]
        public static object DeserializeDataContractXmlString(this string source, Type type, Type[] knownTypes = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            DataContractSerializer dataContractSerializer = (knownTypes == null || knownTypes.Length == 0) ? new DataContractSerializer(type) : new DataContractSerializer(type, knownTypes);

            using (XmlReader xmlReader = XmlReader.Create(new StringReader(source), new XmlReaderSettings { CheckCharacters = false, IgnoreComments = true, IgnoreWhitespace = true, IgnoreProcessingInstructions = true, CloseInput = true }))
            {
                return dataContractSerializer.ReadObject(xmlReader);
            }
        }

        /// <summary>
        /// Deserializes DataContract Xml string to object.
        /// </summary>
        /// <param name="source">The DataContract Xml string to deserialize.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array of object types to serialize.</param>
        /// <returns>Instance of DataContract object.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed.")]
        public static object DeserializeDataContractXmlString(this string source, Type[] knownTypes)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            if (knownTypes == null || knownTypes.Length < 1)
            {
                throw new ArgumentException("knownTypes is null or empty.", "knownTypes");
            }

            Type sourceType = null;

            using (StringReader stringReader = new StringReader(source))
            {
                string rootNodeName = XElement.Load(stringReader).Name.LocalName;
                sourceType = knownTypes.FirstOrDefault(p => p.Name == rootNodeName);

                if (sourceType == null)
                {
                    throw new InvalidOperationException();
                }
            }

            DataContractSerializer dataContractSerializer = new DataContractSerializer(sourceType, knownTypes);

            using (XmlReader xmlReader = XmlReader.Create(new StringReader(source), new XmlReaderSettings { CheckCharacters = false, IgnoreComments = true, IgnoreWhitespace = true, IgnoreProcessingInstructions = true, CloseInput = true }))
            {
                return dataContractSerializer.ReadObject(xmlReader);
            }
        }

        /// <summary>
        /// Deserializes DataContract Xml string to object.
        /// </summary>
        /// <typeparam name="T">Type of the returns object.</typeparam>
        /// <param name="source">The DataContract Xml string to deserialize.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array that may be present in the object graph.</param>
        /// <returns>Instance of T.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed.")]
        public static T DeserializeDataContractXmlString<T>(this string source, Type[] knownTypes = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            DataContractSerializer dataContractSerializer = (knownTypes == null || knownTypes.Length == 0) ? new DataContractSerializer(typeof(T)) : new DataContractSerializer(typeof(T), knownTypes);

            using (XmlReader xmlReader = XmlReader.Create(new StringReader(source), new XmlReaderSettings { CheckCharacters = false, IgnoreComments = true, IgnoreWhitespace = true, IgnoreProcessingInstructions = true, CloseInput = true }))
            {
                return (T)dataContractSerializer.ReadObject(xmlReader);
            }
        }

        /// <summary>
        /// Deserializes DataContract Xml string to object, read from file.
        /// </summary>
        /// <param name="source">File name.</param>
        /// <param name="type">Type of DataContract object.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array that may be present in the object graph.</param>
        /// <returns>Instance of DataContract object.</returns>
        public static object ReadDataContractXml(this string source, Type type, Type[] knownTypes = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            string fullPath = Path.GetFullPath(source);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("The specified file does not exist.", fullPath);
            }

            DataContractSerializer dataContractSerializer = (knownTypes == null || knownTypes.Length == 0) ? new DataContractSerializer(type) : new DataContractSerializer(type, knownTypes);

            using (FileStream fileStream = File.OpenRead(fullPath))
            {
                return dataContractSerializer.ReadObject(fileStream);
            }
        }

        /// <summary>
        /// Deserializes DataContract Xml string to object, read from file.
        /// </summary>
        /// <param name="source">File name.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array of object types to serialize.</param>
        /// <returns>Instance of DataContract object.</returns>
        public static object ReadDataContractXml(this string source, Type[] knownTypes)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            if (knownTypes == null || knownTypes.Length < 1)
            {
                throw new ArgumentException("knownTypes is null or empty.", "knownTypes");
            }

            string fullPath = Path.GetFullPath(source);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("The specified file does not exist.", fullPath);
            }

            Type sourceType = null;
            string rootNodeName = XElement.Load(fullPath).Name.LocalName;
            sourceType = knownTypes.FirstOrDefault(p => p.Name == rootNodeName);

            if (sourceType == null)
            {
                throw new InvalidOperationException();
            }

            DataContractSerializer dataContractSerializer = new DataContractSerializer(sourceType, knownTypes);

            using (FileStream fileStream = File.OpenRead(fullPath))
            {
                return dataContractSerializer.ReadObject(fileStream);
            }
        }

        /// <summary>
        /// Deserializes DataContract Xml string to object, read from file.
        /// </summary>
        /// <typeparam name="T">Type of the returns object.</typeparam>
        /// <param name="source">File name.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array that may be present in the object graph.</param>
        /// <returns>Instance of T.</returns>
        public static T ReadDataContractXml<T>(this string source, Type[] knownTypes = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            string fullPath = Path.GetFullPath(source);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("The specified file does not exist.", fullPath);
            }

            DataContractSerializer dataContractSerializer = (knownTypes == null || knownTypes.Length == 0) ? new DataContractSerializer(typeof(T)) : new DataContractSerializer(typeof(T), knownTypes);

            using (FileStream fileStream = File.OpenRead(fullPath))
            {
                return (T)dataContractSerializer.ReadObject(fileStream);
            }
        }

        /// <summary>
        /// Serializes DataContract object to bytes.
        /// </summary>
        /// <param name="source">The DataContract object to serialize.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array that may be present in the object graph.</param>
        /// <returns>Bytes representation of the source DataContract object.</returns>
        public static byte[] SerializeDataContractXmlBinary(this object source, Type[] knownTypes = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            DataContractSerializer dataContractSerializer = (knownTypes == null || knownTypes.Length == 0) ? new DataContractSerializer(source.GetType()) : new DataContractSerializer(source.GetType(), knownTypes);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                dataContractSerializer.WriteObject(memoryStream, source);
                memoryStream.Position = 0;
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Deserializes DataContract bytes to object.
        /// </summary>
        /// <param name="source">The bytes to deserialize.</param>
        /// <param name="type">Type of DataContract object.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array that may be present in the object graph.</param>
        /// <returns>Instance of DataContract object.</returns>
        public static object DeserializeDataContractXmlBinary(this byte[] source, Type type, Type[] knownTypes = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            DataContractSerializer dataContractSerializer = (knownTypes == null || knownTypes.Length == 0) ? new DataContractSerializer(type) : new DataContractSerializer(type, knownTypes);

            using (MemoryStream memoryStream = new MemoryStream(source))
            {
                memoryStream.Position = 0;
                return dataContractSerializer.ReadObject(memoryStream);
            }
        }

        /// <summary>
        /// Deserializes DataContract bytes to object.
        /// </summary>
        /// <param name="source">The bytes to deserialize.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array of object types to serialize.</param>
        /// <returns>Instance of DataContract object.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Reviewed.")]
        public static object DeserializeDataContractXmlBinary(this byte[] source, Type[] knownTypes = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (knownTypes == null || knownTypes.Length < 1)
            {
                throw new ArgumentException("knownTypes is null or empty.", "knownTypes");
            }

            Type sourceType = null;

            using (MemoryStream memoryStream = new MemoryStream(source))
            using (XmlReader xmlReader = XmlReader.Create(memoryStream))
            {
                string rootNodeName = XElement.Load(xmlReader).Name.LocalName;
                sourceType = knownTypes.FirstOrDefault(p => p.Name == rootNodeName);

                if (sourceType == null)
                {
                    throw new InvalidOperationException();
                }

                memoryStream.Position = 0;
                DataContractSerializer dataContractSerializer = new DataContractSerializer(sourceType, knownTypes);
                return dataContractSerializer.ReadObject(memoryStream);
            }
        }

        /// <summary>
        /// Deserializes DataContract bytes to object.
        /// </summary>
        /// <typeparam name="T">Type of the returns object.</typeparam>
        /// <param name="source">The bytes to deserialize.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array that may be present in the object graph.</param>
        /// <returns>Instance of DataContract object.</returns>
        public static T DeserializeDataContractXmlBinary<T>(this byte[] source, Type[] knownTypes = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            DataContractSerializer dataContractSerializer = (knownTypes == null || knownTypes.Length == 0) ? new DataContractSerializer(typeof(T)) : new DataContractSerializer(typeof(T), knownTypes);

            using (MemoryStream memoryStream = new MemoryStream(source))
            {
                memoryStream.Position = 0;
                return (T)dataContractSerializer.ReadObject(memoryStream);
            }
        }
    }
}
