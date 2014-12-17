﻿//-----------------------------------------------------------------------
// <copyright file="WcfServiceHostEventArgs.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.ServiceModel
{
    using System;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;

    /// <summary>
    /// WcfServiceHost EventArgs.
    /// </summary>
    [Serializable]
    public class WcfServiceHostEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WcfServiceHostEventArgs" /> class.
        /// </summary>
        /// <param name="name">String of Wcf Service Name.</param>
        /// <param name="state">Instance of WcfServiceHostStateEnum.</param>
        /// <param name="endpoints">The endpoints.</param>
        /// <param name="channelMessage">The unit of communication between endpoints in a distributed environment.</param>
        /// <param name="message">The Wcf service message.</param>
        public WcfServiceHostEventArgs(string name, WcfServiceHostState state, ServiceEndpointCollection endpoints = null, Message channelMessage = null, string message = "")
        {
            this.Name = name;
            this.Endpoints = endpoints;
            this.State = state;
            this.ChannelMessage = channelMessage;
            this.Message = message;
        }

        /// <summary>
        /// Gets Wcf service name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the collection of endpoints from the service description.
        /// </summary>
        public ServiceEndpointCollection Endpoints
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets Wcf service state.
        /// </summary>
        public WcfServiceHostState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the unit of communication between endpoints in a distributed environment.
        /// </summary>
        public Message ChannelMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets Wcf service message.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }
    }
}
