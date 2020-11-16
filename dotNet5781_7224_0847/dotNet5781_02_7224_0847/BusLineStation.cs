using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7224_0847
{
    class BusLineStation:BusStation
    {   private double distance;
        private static Random r = new Random();

        ///////////////////////////////////////////////////////
        public BusLineStation()
        {

        }
        public double Distance
        {
            get { return distance; }
            set
            {
                double lat = r.NextDouble() * (33.3 - 31) + 31;
                double lon = r.NextDouble() * (35.5 - 34.3) + 34.3;
                distance = Math.Sqrt(Math.Pow(lat - Latitude, 2) - Math.Pow(lon - Londitude, 2));
            }
        }
        ///////////////////////////////////////////////////////
        private double time;

        public double Time
        {
            get { return time; }
            set { time = r.NextDouble()* (60-1)+1; }
        }



        ///////////////////////////////////////////////////////
        public override string ToString()
        {
            string s1 = base.ToString(); //callind the tostring of BusStation
            string s2 = "distance from last station: " + distance +" km"+ " travel time from last station " + time+" minutes";
            return " " + s1+ "\n" +s2 + "\n";
        }
    }
}














//DateTime d = new DateTime();
//d.Min.

//private TimeSpan time;
//public TimeSpan Time
//{
//    get { return time; }
//    set
//    {
//        DateTime d1 = DateTime.Now;
//        DateTime d2 = new DateTime(1,1,(int)distance+2017);//we used the distance so the num will turn out random each time\
//        time = d1 - d2;
//    }
//}
//private BusStation busStation = new BusStation();