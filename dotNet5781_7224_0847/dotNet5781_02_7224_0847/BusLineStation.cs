using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7224_0847
{
    class BusLineStation
    {
        private static Random r = new Random();
        public BusStation busStation
        {
            get { return busStation; }////////////////
            private set { busStation = value; }//////
        }
        private double distance1;
        public double distance
        {
            get { return distance1; }
            set
            {
                double lat = r.NextDouble() * (33.3 - 31) + 31;
                double lon = r.NextDouble() * (33.3 - 31) + 31;
                distance1 = Math.Sqrt(Math.Pow(lat - busStation.latitude, 2) - Math.Pow(lon - busStation.longitude, 2));
            }
        }

        private TimeSpan time1;
        public TimeSpan time
        {
            get { return time1; }

        }

        public override string ToString()
        {
            return " " + busStation.key + " ";
        }
    }
}
