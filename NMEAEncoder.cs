namespace Aifrus.GPSout
{
    internal class NMEAEncoder
    {
        public string UTCTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Altitude { get; set; }
        public string Course { get; set; }
        public string Speed { get; set; }

        public string EncodeGPGGA()
        {
            string data = $"$GPGGA,{UTCTime},{Latitude},N,{Longitude},E,1,08,0.9,{Altitude},M,0.0,M,,";
            string checksum = CalculateChecksum(data);
            return data + "*" + checksum;
        }

        public string EncodeGPGLL()
        {
            string data = $"$GPGLL,{Latitude},N,{Longitude},E,{UTCTime},A";
            string checksum = CalculateChecksum(data);
            return data + "*" + checksum;
        }

        public string EncodeGPRMC()
        {
            string data = $"$GPRMC,{UTCTime},A,{Latitude},N,{Longitude},E,{Speed},{Course},{UTCTime},,,A";
            string checksum = CalculateChecksum(data);
            return data + "*" + checksum;
        }

        public string EncodeGPVTG()
        {
            string data = $"$GPVTG,{Course},T,,M,{Speed},N,,K,A";
            string checksum = CalculateChecksum(data);
            return data + "*" + checksum;
        }

        private string CalculateChecksum(string sentence)
        {
            int checksum = 0;
            for (int i = 1; i < sentence.Length; i++)
            {
                checksum ^= sentence[i];
            }
            return checksum.ToString("X2");
        }
    }
}
