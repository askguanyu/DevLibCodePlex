﻿//-----------------------------------------------------------------------
// <copyright file="AddInConstants.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.AddIn
{
    /// <summary>
    ///
    /// </summary>
    internal static class AddInConstants
    {
        /// <summary>
        ///
        /// </summary>
        internal const string DefaultFriendlyName = "AddInDomain";

        /// <summary>
        ///
        /// </summary>
        internal const string KeyIpcPortName = "portName";

        /// <summary>
        ///
        /// </summary>
        internal const string KeyIpcChannelName = "name";

        /// <summary>
        ///
        /// </summary>
        internal const string IpcUrlStringFormat = "ipc://{0}/{1}";

        /// <summary>
        ///
        /// </summary>
        internal const string AddInUnknownPlatformException = "Unknown platform target specified.";

        /// <summary>
        ///
        /// </summary>
        internal const string ExceptionStringFormat = "[Exception:\r\n{0} failure with exception.\r\nSource:\r\n{1}\r\nMessage:\r\n{2}\r\nStackTrace:\r\n{3}\r\nRaw:\r\n{4}\r\n]";

        /// <summary>
        ///
        /// </summary>
        internal const string WarningStringFormat = "[Warning:\r\n{0} failure with exception.\r\nSource:\r\n{1}\r\nMessage:\r\n{2}\r\nStackTrace:\r\n{3}\r\nRaw:\r\n{4}\r\n]";

        /// <summary>
        ///
        /// </summary>
        internal const string ProcessOuputStringFormat = "[{0}] {1}";

        /// <summary>
        ///
        /// </summary>
        internal const string ProcessStartTimeoutException = "Waiting for remote process to start timeout.";

        /// <summary>
        ///
        /// </summary>
        internal const string ProcessStartExceptionStringFormat = "Failed to start process from: {0}";

        /// <summary>
        ///
        /// </summary>
        internal const string AssemblyResolverException = "Could not load type for assembly resolver.";

        /// <summary>
        ///
        /// </summary>
        internal const string EventHandleNotExist = "Event handle did not exist for remote process.";

        /// <summary>
        ///
        /// </summary>
        internal const string EventHandleAlreadyExistedException = "Event handle already existed for remote process.";

        /// <summary>
        ///
        /// </summary>
        internal const string DeleteFileExceptionStringFormat = "Failed to delete AddInDomain file '{0}'";
    }
}
