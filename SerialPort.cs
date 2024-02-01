using System;

namespace Aifrus.GPSout
{
    /// <summary>
    /// Provides methods for interacting with a serial port.
    /// </summary>
    internal class SerialPort
    {
        private readonly System.IO.Ports.SerialPort _port;

        /// <summary>
        /// Initializes a new instance of the SerialPort class with the specified port name.
        /// </summary>
        /// <param name="portName">The name of the serial port to connect to.</param>
        public SerialPort(string portName)
        {
            _port = new System.IO.Ports.SerialPort(portName);
        }

        /// <summary>
        /// Opens the serial port.
        /// </summary>
        public void Open()
        {
            ExecuteWithExceptionHandling(() =>
            {
                _port.Open();
                Console.WriteLine("Port opened");
            }, "Error opening port: ");
        }

        /// <summary>
        /// Closes the serial port.
        /// </summary>
        public void Close()
        {
            try
            {
                _port.Close();
                Console.WriteLine("Port closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing port: {ex.Message}");
            }
            finally
            {
                _port.Dispose();
            }
        }

        /// <summary>
        /// Writes the specified string to the serial port.
        /// </summary>
        /// <param name="data">The string to write to the serial port.</param>
        public void WriteLine(string data)
        {
            ExecuteWithExceptionHandling(() =>
            {
                _port.WriteLine(data);
                Console.WriteLine(data);
            }, "Error writing to port: ");
        }

        /// <summary>
        /// Executes the specified action and handles any exceptions that occur.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="errorMessage">The error message to display if an exception occurs.</param>
        private void ExecuteWithExceptionHandling(Action action, string errorMessage)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine(errorMessage + ex.Message);
            }
        }
    }
}
