﻿//-----------------------------------------------------------------------
// <copyright file="SyncSerialPort.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Utilities
{
    using System;
    using System.IO.Ports;
    using System.Threading;

    /// <summary>
    ///
    /// </summary>
    public enum SerialStatus
    {
        INIT,

        OperateOK,

        NotFoundPort,

        NotExistPort,

        SerialPortNull,

        OpenException,

        SendException,

        SendDataEmpty,

        ReadTimeout,

        ReadException
    }

    /// <summary>
    /// Represents a sync serial port resource
    /// </summary>
    public class SyncSerialPort : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        private static string[] _portNames = SerialPort.GetPortNames();

        /// <summary>
        ///
        /// </summary>
        private SerialPort _serialPort = null;

        /// <summary>
        ///
        /// </summary>
        private SerialStatus _currentStatus = SerialStatus.INIT;

        /// <summary>
        ///
        /// </summary>
        private byte[] _receiveBuffer = null;

        /// <summary>
        ///
        /// </summary>
        private int _readTimeout = 1000;

        /// <summary>
        ///
        /// </summary>
        private AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        /// <summary>
        ///
        /// </summary>
        private object _syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the SyncSerialPort class using
        /// the specified port name, baud rate, parity bit, data bits, and stop bit.
        /// </summary>
        /// <param name="portName">The port to use (for example, COM1)</param>
        /// <param name="baudRate">The baud rate</param>
        /// <param name="partity">One of the System.IO.Ports.SerialPort.Parity values</param>
        /// <param name="dataBits">The data bits value</param>
        /// <param name="stopBits">One of the System.IO.Ports.SerialPort.StopBits values</param>
        public SyncSerialPort(string portName, int baudRate, Parity partity, int dataBits, StopBits stopBits)
        {
            if (_portNames == null || _portNames.Length == 0)
            {
                _currentStatus = SerialStatus.NotExistPort;
            }
            else
            {
                bool isExistPort = false;
                for (int i = 0; i < _portNames.Length; i++)
                {
                    if (_portNames[i].Equals(portName))
                    {
                        isExistPort = true;
                        break;
                    }
                }

                if (isExistPort)
                {
                    _serialPort = new SerialPort(portName, baudRate, partity, dataBits, stopBits);
                    _currentStatus = SerialStatus.OperateOK;
                }
                else
                {
                    _currentStatus = SerialStatus.NotFoundPort;
                }
            }
        }

        /// <summary>
        /// Gets an array of serial port names for the current computer
        /// </summary>
        public static string[] PortNames
        {
            get { return _portNames; }
        }

        /// <summary>
        /// Current status
        /// </summary>
        public SerialStatus CurrentStatus
        {
            get { return _currentStatus; }
        }

        /// <summary>
        /// Gets a value indicating the open or closed status of SyncSerialPort
        /// </summary>
        public bool IsOpen
        {
            get { return _serialPort.IsOpen; }
        }

        /// <summary>
        /// Opens a new serial port connection
        /// </summary>
        public bool Open()
        {
            if (_serialPort != null)
            {
                try
                {
                    if (!_serialPort.IsOpen)
                    {
                        _serialPort.Open();
                    }

                    return true;
                }
                catch
                {
                    _currentStatus = SerialStatus.OpenException;
                    return false;
                }
            }
            else
            {
                _currentStatus = SerialStatus.SerialPortNull;
                return false;
            }
        }

        /// <summary>
        /// Closes the port connection
        /// </summary>
        public bool Close()
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }

                return true;
            }
            else
            {
                _currentStatus = SerialStatus.SerialPortNull;
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            if (_serialPort != null)
            {
                _serialPort.Dispose();
            }
        }

        /// <summary>
        /// Sync send a specified number of bytes to the serial port using data from a buffer
        /// </summary>
        /// <param name="sendData">The byte array that contains the data to write to the port</param>
        /// <param name="receivedData">The byte array to write the received data</param>
        /// <param name="timeout">The number of milliseconds before a time-out occurs when a read operation does not finish</param>
        public bool Send(byte[] sendData, out byte[] receivedData, int timeout)
        {
            lock (_syncRoot)
            {
                _autoResetEvent.Reset();

                if (sendData == null || sendData.Length == 0)
                {
                    receivedData = new byte[0];
                    _currentStatus = SerialStatus.SendDataEmpty;
                    return false;
                }

                if (_serialPort == null)
                {
                    receivedData = new byte[0];
                    _currentStatus = SerialStatus.SerialPortNull;
                    return false;
                }

                _readTimeout = timeout;

                try
                {
                    if (!Open())
                    {
                        receivedData = new byte[0];
                        return false;
                    }

                    _serialPort.Write(sendData, 0, sendData.Length);
                    Thread threadReceive = new Thread(new ParameterizedThreadStart(SyncReceiveData));
                    threadReceive.IsBackground = true;

                    //threadReceive.Name = "ReadSerialPortData";
                    threadReceive.Start(_serialPort);
                    _autoResetEvent.WaitOne();

                    if (threadReceive.IsAlive)
                    {
                        threadReceive.Abort();
                    }

                    if (_currentStatus == SerialStatus.OperateOK)
                    {
                        receivedData = _receiveBuffer;
                        return true;
                    }
                    else
                    {
                        receivedData = new byte[0];
                        return false;
                    }
                }
                catch
                {
                    receivedData = new byte[0];
                    _currentStatus = SerialStatus.SendException;
                    return false;
                }
            }
        }

        /// <summary>
        /// Sync receive data
        /// </summary>
        private void SyncReceiveData(object serialPortobj)
        {
            SerialPort serialPort = serialPortobj as SerialPort;
            System.Threading.Thread.Sleep(0);
            serialPort.ReadTimeout = _readTimeout;

            try
            {
                byte firstByte = Convert.ToByte(serialPort.ReadByte());
                int bytesRead = serialPort.BytesToRead;
                _receiveBuffer = new byte[bytesRead + 1];
                _receiveBuffer[0] = firstByte;
                for (int i = 1; i <= bytesRead; i++)
                {
                    _receiveBuffer[i] = Convert.ToByte(serialPort.ReadByte());
                }

                _currentStatus = SerialStatus.OperateOK;
            }
            catch
            {
                _currentStatus = SerialStatus.ReadTimeout;
            }
            finally
            {
                _autoResetEvent.Set();
                Close();
            }
        }
    }
}
