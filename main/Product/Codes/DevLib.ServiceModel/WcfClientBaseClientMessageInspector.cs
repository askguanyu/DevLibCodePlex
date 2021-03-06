﻿//-----------------------------------------------------------------------
// <copyright file="WcfClientBaseClientMessageInspector.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.ServiceModel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Threading;

    /// <summary>
    /// WcfClientBase ClientMessageInspector.
    /// </summary>
    [Serializable]
    public class WcfClientBaseClientMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// Field _oneWayActions.
        /// </summary>
        private readonly HashSet<string> _oneWayActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfClientBaseClientMessageInspector"/> class.
        /// This new instance will simulate ReceivingReply event for OneWay action.
        /// </summary>
        /// <param name="serviceEndpoint">The service endpoint.</param>
        public WcfClientBaseClientMessageInspector(ServiceEndpoint serviceEndpoint)
        {
            this._oneWayActions = new HashSet<string>();

            foreach (var operation in serviceEndpoint.Contract.Operations)
            {
                if (operation.IsOneWay)
                {
                    this._oneWayActions.Add(operation.Messages[0].Action);
                }
            }
        }

        /// <summary>
        /// Occurs before send request.
        /// </summary>
        public event EventHandler<WcfClientBaseEventArgs> SendingRequest;

        /// <summary>
        /// Occurs after receive reply.
        /// </summary>
        public event EventHandler<WcfClientBaseEventArgs> ReceivingReply;

        /// <summary>
        /// Enables inspection or modification of a message after a reply message is received but prior to passing it back to the client application.
        /// </summary>
        /// <param name="reply">The message to be transformed into types and handed back to the client application.</param>
        /// <param name="correlationState">Correlation state data.</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            Guid messageId = Guid.Empty;

            try
            {
                messageId = (Guid)correlationState;
            }
            catch
            {
            }

            string message = null;

            if (reply != null)
            {
                message = reply.ToString();
            }
            else
            {
                message = string.Empty;
            }

            Debug.WriteLine("DevLib.ServiceModel.WcfClientBaseClientMessageInspector.AfterReceiveReply: " + messageId.ToString());
            Debug.WriteLine(message);

            this.RaiseEvent(this.ReceivingReply, reply, message, messageId, false);
        }

        /// <summary>
        /// Enables inspection or modification of a message before a request message is sent to a service.
        /// </summary>
        /// <param name="request">The message to be sent to the service.</param>
        /// <param name="channel">The WCF client object channel.</param>
        /// <returns>The object that is returned as the correlationState argument of the <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)" /> method. This is null if no correlation state is used.The best practice is to make this a <see cref="T:System.Guid" /> to ensure that no two correlationState objects are the same.</returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            Guid messageId = Guid.NewGuid();

            string message = null;

            bool isOneWay = false;

            if (request != null)
            {
                message = request.ToString();

                isOneWay = this._oneWayActions.Contains(request.Headers.Action);
            }
            else
            {
                message = string.Empty;
            }

            Debug.WriteLine("DevLib.ServiceModel.WcfClientBaseClientMessageInspector.BeforeSendRequest: " + messageId.ToString());
            Debug.WriteLine(message);

            this.RaiseEvent(this.SendingRequest, request, message, messageId, isOneWay);

            if (isOneWay)
            {
                Debug.WriteLine("DevLib.ServiceModel.WcfClientBaseClientMessageInspector.BeforeSendRequest(simulate reply for OneWay): " + messageId.ToString());

                this.RaiseEvent(this.ReceivingReply, request, message, messageId, isOneWay);
            }

            return messageId;
        }

        /// <summary>
        /// Method RaiseEvent.
        /// </summary>
        /// <param name="eventHandler">Instance of EventHandler.</param>
        /// <param name="channelMessage">The channel message.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="isOneWay">Whether the message is one way.</param>
        private void RaiseEvent(EventHandler<WcfClientBaseEventArgs> eventHandler, Message channelMessage, string message, Guid messageId, bool isOneWay)
        {
            // Copy a reference to the delegate field now into a temporary field for thread safety
            EventHandler<WcfClientBaseEventArgs> temp = Interlocked.CompareExchange(ref eventHandler, null, null);

            if (temp != null)
            {
                temp(null, new WcfClientBaseEventArgs(null, null, null, channelMessage, message, messageId, isOneWay, null, null));
            }
        }
    }
}
