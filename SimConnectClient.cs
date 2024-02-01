using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Runtime.InteropServices;

namespace Aifrus.GPSout
{
    internal class SimConnectClient : IDisposable
    {
        private SimConnect simConnect = null;
        private const int WM_USER_SIMCONNECT = 0x0402;

        enum DEFINITIONS
        {
            Struct1,
        }
#pragma warning disable 0649
        struct Struct1
        {
            public double Latitude;
            public double Longitude;
        }
#pragma warning restore 0649


        public event Action<double, double> OnDataReceived;
        public SimConnectClient()
        {
            try
            {
                simConnect = new SimConnect("Managed Data Request", IntPtr.Zero, WM_USER_SIMCONNECT, null, 0);
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.RegisterDataDefineStruct<Struct1>(DEFINITIONS.Struct1);
                simConnect.OnRecvSimobjectDataBytype += SimConnect_OnRecvSimobjectDataBytype;
                Console.WriteLine("Connected to Flight Simulator");
            }
            catch (COMException ex)
            {
                Console.WriteLine("Unable to connect to Flight Simulator " + ex.Message);
            }
        }

        private void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            Console.WriteLine("OnRecvSimobjectDataBytype event triggered");

            if (data.dwRequestID == (uint)DEFINITIONS.Struct1)
            {
                Struct1 s1 = (Struct1)data.dwData[0];
                OnDataReceived?.Invoke(s1.Latitude, s1.Longitude);
            }
        }

        public void RequestData()
        {
            simConnect.RequestDataOnSimObjectType(DEFINITIONS.Struct1, DEFINITIONS.Struct1, 1000000, SIMCONNECT_SIMOBJECT_TYPE.USER);
        }


        public void Dispose()
        {
            simConnect?.Dispose();
        }
    }
}
