﻿//-----------------------------------------------------------------------
// <copyright file="Logger.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    /// <summary>
    /// Class Logger. Provides logging functions.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Reviewed.")]
    public sealed class Logger
    {
        /// <summary>
        /// Field _logFile.
        /// </summary>
        private readonly string _logFile;

        /// <summary>
        /// Field _loggerSetup.
        /// </summary>
        private readonly LoggerSetup _loggerSetup;

        /// <summary>
        /// Field _fileAppender.
        /// </summary>
        private readonly MutexMultiProcessFileAppender _fileAppender;

        /// <summary>
        /// Synchronizes access to <see cref="_queue" />.
        /// </summary>
        private readonly object _queueSyncRoot = new object();

        /// <summary>
        /// The <see cref="Queue{T}" /> that contains the data items.
        /// </summary>
        private readonly Queue<string> _queue;

        /// <summary>
        /// Allows the consumer thread to block when no items are available in the <see cref="_queue" />.
        /// </summary>
        private readonly EventWaitHandle _queueWaitHandle;

        /// <summary>
        /// Field _consumerThread.
        /// </summary>
        private readonly Thread _consumerThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="logFile">Log file.</param>
        /// <param name="loggerSetup">Log setup.</param>
        internal Logger(string logFile, LoggerSetup loggerSetup)
        {
            this._logFile = logFile;

            this._loggerSetup = loggerSetup;

            if (this._loggerSetup.WriteToFile)
            {
                try
                {
                    this._fileAppender = new MutexMultiProcessFileAppender(this._logFile, this._loggerSetup);

                    this._queueWaitHandle = new AutoResetEvent(false);

                    this._queue = new Queue<string>();

                    this._consumerThread = new Thread(this.ConsumerThread);

                    this._consumerThread.IsBackground = true;

                    this._consumerThread.Start();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Logger" /> class.
        /// </summary>
        ~Logger()
        {
            if (this._queueWaitHandle != null)
            {
                this._queueWaitHandle.Close();
            }

            if (this._fileAppender != null)
            {
                this._fileAppender.Dispose();
            }

            if (this._consumerThread != null)
            {
                try
                {
                    this._consumerThread.Abort();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Writes the diagnostic message at the specified level.
        /// </summary>
        /// <param name="logLevel">Log level.</param>
        /// <param name="objs">Diagnostic messages or objects to log.</param>
        public void Log(LogLevel logLevel, params object[] objs)
        {
            this.Log(1, logLevel, objs);
        }

        /// <summary>
        /// Writes the diagnostic message at the specified level.
        /// </summary>
        /// <param name="skipFrames">The number of frames up the stack to skip.</param>
        /// <param name="logLevel">Log level.</param>
        /// <param name="objs">Diagnostic messages or objects to log.</param>
        public void Log(int skipFrames, LogLevel logLevel, params object[] objs)
        {
            try
            {
                if ((this._loggerSetup.WriteToConsole && Environment.UserInteractive) || this._fileAppender != null)
                {
                    string message = LogLayout.Render(skipFrames, logLevel, this._loggerSetup.UseBracket, objs);

                    if (this._loggerSetup.WriteToConsole && Environment.UserInteractive)
                    {
                        ColoredConsoleAppender.Write(logLevel, message);
                    }

                    if (this._fileAppender != null && logLevel >= this._loggerSetup.Level)
                    {
                        lock (this._queueSyncRoot)
                        {
                            this._queue.Enqueue(message);

                            this._queueWaitHandle.Set();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Log(e);
            }
        }

        /// <summary>
        /// The consumer thread.
        /// </summary>
        private void ConsumerThread()
        {
            while (true)
            {
                string nextItem = null;

                bool itemExists;

                lock (this._queueSyncRoot)
                {
                    itemExists = this._queue.Count > 0;

                    if (itemExists)
                    {
                        nextItem = this._queue.Dequeue();
                    }
                }

                if (itemExists)
                {
                    try
                    {
                        this._fileAppender.Write(nextItem);
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.Log(e);
                    }
                }
                else
                {
                    this._queueWaitHandle.WaitOne();
                }
            }
        }
    }
}
