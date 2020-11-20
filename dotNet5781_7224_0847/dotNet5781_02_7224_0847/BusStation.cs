using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7224_0847
{
    class BusStation
    {
        public BusStation(int key)
        {
            if (num > 999999 || num < 0)
                throw new BusException("invalid number of digits for the key was insertd");
            BusStationKey = key;

            Latitude = r.NextDouble() * (33.3 - 31) + 31;

            Longitude = r.NextDouble() * (35.5 - 34.3) + 34.3;
        }
        private static Random r = new Random();

        private static int num = 0;
        ////////////////////////////////////////
        public int BusStationKey//the code of the bus syation
        {
            get; private set;
        }
        ////////////////////////////////////////
        public double Latitude//field "rochav"
        {
        get; private set;
        }

        ////////////////////////////////////////
        public double Longitude//field "orech"
        {
            get; private set;
        }
        ////////////////////////////////////////

        public override string ToString()
        {
            return "Bus Station Code: " + BusStationKey + ", " + Latitude + "°N " + Longitude + "°E" + "\n";
        }


    }
}
















//private BusStation() /////ctor   ///////////////// 
//{
//    BusStationKey =1;
//}