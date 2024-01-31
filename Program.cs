using System;
using Microsoft.FlightSimulator.SimConnect;

namespace SimConnectApp
{
    class Program
    {
        private static SimConnect SimConnect = null;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Attempting to connect to SimConnect...");
                SimConnect = new SimConnect("Managed Data Request", IntPtr.Zero, 0, null, 0);
                SimConnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                SimConnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);
                Console.WriteLine("SimConnect initialized. Waiting for connection...");

                // Loop until SimConnect is null, which happens when the OnRecvQuit event is triggered.
                while (SimConnect != null)
                {
                    System.Threading.Thread.Sleep(1000); // Sleep for 1 second.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to connect to SimConnect: " + ex.Message);
            }
        }

        static void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Console.WriteLine("Connected to SimConnect.");
        }

        static void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Console.WriteLine("SimConnect connection closed.");
            SimConnect = null;
        }
    }
}