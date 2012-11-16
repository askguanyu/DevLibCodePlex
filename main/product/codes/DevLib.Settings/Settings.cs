﻿//-----------------------------------------------------------------------
// <copyright file="Settings.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Settings
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a configuration file that is applicable to a particular application. This class cannot be inherited.
    /// </summary>
    public sealed class Settings
    {
        /// <summary>
        ///
        /// </summary>
        private Dictionary<string, object> _settingsItemDictionary;

        /// <summary>
        ///
        /// </summary>
        private XmlReaderSettings _xmlReaderSettings;

        /// <summary>
        ///
        /// </summary>
        private XmlWriterSettings _xmlWriterSettings;

        /// <summary>
        ///
        /// </summary>
        private XmlSerializerNamespaces _xmlNamespaces;

        /// <summary>
        ///
        /// </summary>
        /// <param name="configFile"></param>
        internal Settings(string configFile)
        {
            this.ConfigFile = configFile;
            this.Init();
            this.Reload();
        }

        /// <summary>
        /// Gets current configuration file.
        /// </summary>
        public string ConfigFile
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets
        /// </summary>
        public int Count
        {
            get
            {
                lock (((ICollection)this._settingsItemDictionary).SyncRoot)
                {
                    return this._settingsItemDictionary.Count;
                }
            }
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public object this[string key]
        {
            get
            {
                return this.GetValue(key);
            }

            set
            {
                this.SetValue(key, value);
            }
        }

        /// <summary>
        ///Gets
        /// </summary>
        public string[] Keys
        {
            get
            {
                lock (((ICollection)this._settingsItemDictionary).SyncRoot)
                {
                    string[] result = new string[this._settingsItemDictionary.Count];
                    this._settingsItemDictionary.Keys.CopyTo(result, 0);
                    return result;
                }
            }
        }

        /// <summary>
        ///Gets
        /// </summary>
        public object[] Values
        {
            get
            {
                lock (((ICollection)this._settingsItemDictionary).SyncRoot)
                {
                    object[] result = new object[this._settingsItemDictionary.Count];
                    this._settingsItemDictionary.Values.CopyTo(result, 0);
                    return result;
                }
            }
        }

        /// <summary>
        /// Writes the configuration settings to the current XML configuration file.
        /// </summary>
        public void Save()
        {
            if (string.IsNullOrEmpty(this.ConfigFile))
            {
                throw new ArgumentNullException("Settings.ConfigFile", "Didn't specify a configuration file.");
            }

            try
            {
                this.WriteXmlFile(this.ConfigFile);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Writes the configuration settings to the specified XML configuration file. Keep using current Settings instance.
        /// </summary>
        /// <param name="fileName">The path and file name to save the configuration file to.</param>
        public void SaveAs(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName", "Didn't specify a configuration file.");
            }

            try
            {
                this.WriteXmlFile(fileName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets value for specified key.
        /// </summary>
        /// <param name="key">A string specifying the key.</param>
        /// <param name="value">An object specifying the value.</param>
        public void SetValue(string key, object value)
        {
            this.CheckNullKey(key);

            lock (((ICollection)this._settingsItemDictionary).SyncRoot)
            {
                this._settingsItemDictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets value of specified key.
        /// </summary>
        /// <param name="key">A string specifying the key.</param>
        /// <param name="defaultValue">If <paramref name="key"/> does not exist, return default value.</param>
        /// <returns>A configuration object, or <paramref name="defaultValue"/> if <paramref name="key"/> does not exist in the collection.</returns>
        public object GetValue(string key)
        {
            this.CheckNullKey(key);

            lock (((ICollection)this._settingsItemDictionary).SyncRoot)
            {
                if (this._settingsItemDictionary.ContainsKey(key))
                {
                    try
                    {
                        return this._settingsItemDictionary[key];
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    throw new KeyNotFoundException(string.Format(SettingsConstants.KeyNotFoundExceptionStringFormat, key));
                }
            }
        }

        /// <summary>
        /// Gets value of specified key.
        /// </summary>
        /// <param name="key">A string specifying the key.</param>
        /// <param name="defaultValue">If <paramref name="key"/> does not exist, return default value.</param>
        /// <returns>A configuration object, or <paramref name="defaultValue"/> if <paramref name="key"/> does not exist in the collection.</returns>
        public T GetValue<T>(string key)
        {
            this.CheckNullKey(key);

            lock (((ICollection)this._settingsItemDictionary).SyncRoot)
            {
                if (this._settingsItemDictionary.ContainsKey(key))
                {
                    try
                    {
                        return (T)this._settingsItemDictionary[key];
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    throw new KeyNotFoundException(string.Format(SettingsConstants.KeyNotFoundExceptionStringFormat, key));
                }
            }
        }

        /// <summary>
        /// Gets value of specified key.
        /// </summary>
        /// <param name="key">A string specifying the key.</param>
        /// <param name="defaultValue">If <paramref name="key"/> does not exist, return default value.</param>
        /// <returns>A configuration object, or <paramref name="defaultValue"/> if <paramref name="key"/> does not exist in the collection.</returns>
        public T GetValue<T>(string key, T defaultValue)
        {
            this.CheckNullKey(key);

            lock (((ICollection)this._settingsItemDictionary).SyncRoot)
            {
                if (this._settingsItemDictionary.ContainsKey(key))
                {
                    try
                    {
                        return (T)this._settingsItemDictionary[key];
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// Removes a setting by key.
        /// </summary>
        /// <param name="key">A string specifying the key.</param>
        public void Remove(string key)
        {
            this.CheckNullKey(key);

            lock (((ICollection)this._settingsItemDictionary).SyncRoot)
            {
                this._settingsItemDictionary.Remove(key);
            }
        }

        /// <summary>
        /// Clears the settings.
        /// </summary>
        public void Clear()
        {
            lock (((ICollection)this._settingsItemDictionary).SyncRoot)
            {
                this._settingsItemDictionary.Clear();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            this.CheckNullKey(key);

            return this._settingsItemDictionary.ContainsKey(key);
        }

        /// <summary>
        ///
        /// </summary>
        public void Reload()
        {
            if (string.IsNullOrEmpty(this.ConfigFile) || !File.Exists(this.ConfigFile))
            {
                return;
            }

            using (XmlReader xmlReader = XmlReader.Create(this.ConfigFile, this._xmlReaderSettings))
            {
                if (xmlReader.IsEmptyElement || !xmlReader.Read())
                {
                    return;
                }

                xmlReader.ReadStartElement("settings");

                lock (((ICollection)this._settingsItemDictionary).SyncRoot)
                {
                    this._settingsItemDictionary.Clear();

                    while (xmlReader.NodeType != XmlNodeType.EndElement)
                    {
                        string key = xmlReader.GetAttribute("key");

                        xmlReader.ReadStartElement("item");

                        string valueTypeName = xmlReader.GetAttribute("type");
                        XmlSerializer valueSerializer = new XmlSerializer(Type.GetType(valueTypeName, false, true));

                        xmlReader.ReadStartElement("value");
                        object value = valueSerializer.Deserialize(xmlReader);
                        xmlReader.ReadEndElement();

                        this._settingsItemDictionary.Add(key, value);

                        xmlReader.ReadEndElement();
                        xmlReader.MoveToContent();
                    }
                }

                xmlReader.ReadEndElement();
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void Init()
        {
            this._settingsItemDictionary = new Dictionary<string, object>();

            this._xmlWriterSettings = new XmlWriterSettings();
            this._xmlWriterSettings.ConformanceLevel = ConformanceLevel.Auto;
            this._xmlWriterSettings.OmitXmlDeclaration = true;
            this._xmlWriterSettings.Indent = true;

            this._xmlReaderSettings = new XmlReaderSettings();
            this._xmlReaderSettings.IgnoreComments = true;
            this._xmlReaderSettings.IgnoreProcessingInstructions = true;
            this._xmlReaderSettings.IgnoreWhitespace = true;
            this._xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;

            this._xmlNamespaces = new XmlSerializerNamespaces();
            this._xmlNamespaces.Add(String.Empty, String.Empty);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        private void CheckNullKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        private void WriteXmlFile(string fileName)
        {
            using (XmlWriter writer = XmlWriter.Create(fileName, this._xmlWriterSettings))
            {
                writer.WriteStartElement("settings");

                lock (((ICollection)this._settingsItemDictionary).SyncRoot)
                {
                    foreach (KeyValuePair<string, object> item in this._settingsItemDictionary)
                    {
                        writer.WriteStartElement("item");

                        writer.WriteStartAttribute("key");
                        writer.WriteValue(item.Key);
                        writer.WriteEndAttribute();

                        Type valueType = item.Value.GetType();
                        XmlSerializer valueSerializer = new XmlSerializer(valueType);

                        writer.WriteStartElement("value");
                        writer.WriteStartAttribute("type");
                        writer.WriteValue(valueType.AssemblyQualifiedName);
                        writer.WriteEndAttribute();
                        valueSerializer.Serialize(writer, item.Value, this._xmlNamespaces);
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
            }
        }
    }
}
