using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// BusLineStation inherites from busstation and adds a few more features 
/// so it will be able to fit in a path.
/// </summary>
namespace dotNet5781_02_7224_0847
{
    class BusLineStation:BusStation
    {  
        private static Random r = new Random();

        public BusLineStation(int key, bool ifFirst):base(key)
        {
            if (ifFirst)//if its the 1st station in the bus-line path
                Distance = 0;
            else
            {
                double lat = r.NextDouble() * (33.3 - 31) + 31;
                double lon = r.NextDouble() * (35.5 - 34.3) + 34.3;
                Distance = Math.Sqrt(Math.Pow(lat - Latitude, 2) - Math.Pow(lon - Longitude, 2));
            }
            TimeInMin = Distance * 1.2 *10;//we assume that it takes 1.2 min per km
        }

        public double Distance
        {
            get; private set;
        }

        public double TimeInMin
        {
            get;  private set;
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