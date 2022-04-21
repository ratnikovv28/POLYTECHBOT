namespace SPbPUBOT
{
    public class Building
    {
        private double _latitude;
        private double _longitude;
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
            }
        }
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
            }
        }

        public Building(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
