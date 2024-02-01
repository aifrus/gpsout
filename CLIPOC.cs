using System;
using System.Threading;


namespace Aifrus.GPSout
{
    class CLIPOC
    {
        private static readonly SerialPort SerialPort = new SerialPort("COM1");
        static void Main(string[] args)
        {
            SerialPort.Open();
            using (var simConnectClient = new SimConnectClient())
            {
                simConnectClient.OnDataReceived += SimConnectClient_OnDataReceived;

                // Start a new thread for the SimConnect data request
                var simConnectThread = new Thread(() =>
                {
                    while (true)
                    {
                        simConnectClient.RequestData();
                        Thread.Sleep(1000); // Wait for 1 second before sending the next data request
                    }
                });

                simConnectThread.Start();

                // Wait for the Escape key to be pressed
                while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Escape)
                {
                    // Keep the main thread running
                }

                // Stop the SimConnect thread when the Escape key is pressed
                simConnectThread.Abort();
            }
            SerialPort.Close();
        }


        private static void SimConnectClient_OnDataReceived(double latitude, double longitude)
        {
            Console.WriteLine($"Data received: Latitude = {latitude}, Longitude = {longitude}");

            string formattedLatitude = FormatCoordinate(latitude);
            string formattedLongitude = FormatCoordinate(longitude);

            NMEAEncoder encoder = new NMEAEncoder
            {
                UTCTime = DateTime.UtcNow.ToString("HHmmss"),
                Latitude = formattedLatitude,
                Longitude = formattedLongitude,
                Altitude = "545.4", // You may want to replace this with actual data
                Course = "075.4", // You may want to replace this with actual data
                Speed = "022.4" // You may want to replace this with actual data
            };

            string gpggaSentence = encoder.EncodeGPGGA();
            string gpgllSentence = encoder.EncodeGPGLL();
            string gprmcSentence = encoder.EncodeGPRMC();
            string gpvtgSentence = encoder.EncodeGPVTG();

            SerialPort.WriteLine(gpggaSentence);
            SerialPort.WriteLine(gpgllSentence);
            SerialPort.WriteLine(gprmcSentence);
            SerialPort.WriteLine(gpvtgSentence);
        }

        private static string FormatCoordinate(double coordinate)
        {
            int degrees = (int)coordinate;
            double decimalMinutes = (coordinate - degrees) * 60;
            return $"{degrees.ToString("00")}{decimalMinutes.ToString("00.000")}";
        }
    }
}
