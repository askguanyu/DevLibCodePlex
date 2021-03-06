﻿//-----------------------------------------------------------------------
// <copyright file="ConfigPropertyGrid.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Configuration
{
    using System;
    using System.Drawing;
    using System.Security.Permissions;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Class ConfigPropertyGrid.
    /// </summary>
    [ToolboxBitmap(typeof(PropertyGrid))]
    [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
    public class ConfigPropertyGrid : PropertyGrid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigPropertyGrid" /> class.
        /// </summary>
        public ConfigPropertyGrid()
        {
            base.PropertyValueChanged += this.OnPropertyValueChanged;
            ConfigPropertyGridCollectionEditor.CollectionPropertyValueChanged += this.OnPropertyValueChanged;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public new event EventHandler PropertyValueChanged;

        /// <summary>
        /// Gets or sets the object for which the grid displays properties.
        /// </summary>
        public new object SelectedObject
        {
            [SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
            get
            {
                if (base.SelectedObject == null)
                {
                    return null;
                }

                object selectedObject = null;

                try
                {
                    Type selectedObjectType = base.SelectedObject.GetType();

                    if (selectedObjectType.IsGenericType && selectedObjectType.GetGenericTypeDefinition().IsAssignableFrom(typeof(InnerConfig<>)))
                    {
                        selectedObject = typeof(InnerConfig<>).MakeGenericType(selectedObjectType.GetGenericArguments()[0]).GetProperty("Items").GetValue(base.SelectedObject, null);
                    }
                    else
                    {
                        selectedObject = base.SelectedObject;
                    }
                }
                catch
                {
                }

                return selectedObject;
            }

            [SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
            set
            {
                if (value == null)
                {
                    base.SelectedObject = null;
                }
                else
                {
                    Type selectedObjectType = value.GetType();

                    if (selectedObjectType == typeof(string))
                    {
                        base.SelectedObject = value;
                    }
                    else if (selectedObjectType.GetInterface("IEnumerable") == null)
                    {
                        if (!ConfigPropertyGridCollectionEditor.CanConvert(selectedObjectType))
                        {
                            ConfigPropertyGridCollectionEditor.AppendCustomAttributes(selectedObjectType);
                        }

                        base.SelectedObject = value;
                    }
                    else
                    {
                        object innerConfig = Activator.CreateInstance(typeof(InnerConfig<>).MakeGenericType(selectedObjectType), value);
                        ConfigPropertyGridCollectionEditor.AppendCustomAttributes(selectedObjectType);
                        ConfigPropertyGridCollectionEditor.AppendCustomAttributes(innerConfig.GetType());
                        base.SelectedObject = innerConfig;
                    }
                }

                this.PropertySort = PropertySort.NoSort;
                this.ExpandAllGridItems();
                this.Refresh();
            }
        }

        /// <summary>
        /// Method OnPropertyValueChanged.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Instance of EventArgs.</param>
        private void OnPropertyValueChanged(object sender, EventArgs e)
        {
            this.RaiseEvent(this.PropertyValueChanged);
        }

        /// <summary>
        /// Method RaiseEvent.
        /// </summary>
        /// <param name="eventHandler">Instance of EventHandler.</param>
        private void RaiseEvent(EventHandler eventHandler)
        {
            // Copy a reference to the delegate field now into a temporary field for thread safety.
            EventHandler temp = Interlocked.CompareExchange(ref eventHandler, null, null);

            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Inner Class InnerConfig.
        /// </summary>
        /// <typeparam name="T">Type of configuration object.</typeparam>
        protected class InnerConfig<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InnerConfig{T}" /> class.
            /// </summary>
            /// <param name="configurationObject">Configuration object.</param>
            public InnerConfig(T configurationObject)
            {
                this.Items = configurationObject;
            }

            /// <summary>
            /// Gets or sets Items.
            /// </summary>
            public T Items
            {
                get;
                set;
            }
        }
    }
}
