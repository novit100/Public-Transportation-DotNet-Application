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
        private BusStation busStation = new BusStation();

       public double distance1;
        public double distance
        {
            get { return distance1; }
            set
            {
                //double lat = r.NextDouble() * (33.3 - 31) + 31;
                //double lon = r.NextDouble() * (33.3 - 31) + 31;
                //distance1 = Math.Sqrt(Math.Pow(lat - busStation.latitude, 2) - Math.Pow(lon - busStation.longitude, 2));
                
                distance1 = r.NextDouble()*(2000-500)+500;//distance btween stations between 500-2000 m
            }
        }

        private TimeSpan time1=new TimeSpan(0, 0, seconds:(int)(distance1 * 0.05));//we assume that it takes 0.05 seconds per meter.

        public override string ToString()
        {
            return " " + busStation.key + " ";
        }
    }
}
