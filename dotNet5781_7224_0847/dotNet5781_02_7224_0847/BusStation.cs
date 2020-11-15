using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7224_0847
{
    class BusStation
    {

        private static Random r = new Random();

        private static int num = 0;
        ////////////////////////////////////////
        public int BusStationKey  //the code of the bus syation 
        {
            get
            {
                return BusStationKey;
            }

            private set
            {
                if (num > 999999)
                    throw new BusException("more than 6 digits key was insertd- overflow!");//
                BusStationKey = num++;//forwards the codes 

            }
        }
        ////////////////////////////////////////
        private double latitude;//field "rochav"
        public double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = r.NextDouble() * (33.3 - 31) + 31;
            }
        }
        ////////////////////////////////////////
        private double longitude;//field "orech"
        public double Londitude
        {
            get { return longitude; }
            set
            {
                longitude = r.NextDouble() * (35.5 - 34.3) + 34.3;
            }
        }
        ////////////////////////////////////////
        //public string address { get; private set; }

        public override string ToString()
        {
            return "Bus Station Code: " + BusStationKey + ", " + Latitude + "°N " + longitude + "°E" + "\n";
        }


    }
}
















//private BusStation() /////ctor   ///////////////// 
//{
//    BusStationKey =1;
//}