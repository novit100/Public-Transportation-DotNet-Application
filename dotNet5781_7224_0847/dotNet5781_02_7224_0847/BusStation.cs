using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7224_0847
{
    class BusStation
    {
        private static Random r = new Random();
        private int BusStationKey;//field
        public int key
        {
            get
            {
                return BusStationKey;
            }

            set
            {
                if (value >= 100000 && value <= 999999)
                {
                    BusStationKey = value;
                }
                else//less or more than 6 digits
                {
                    throw new MyExeption("ERROR! bus station must have a 6 digits key\n");
                }

            }
        }
        private double latitude1;//field
        public double latitude
        {
            get { return latitude1; }
            set 
            {
                latitude1= r.NextDouble() * (33.3 - 31) + 31;
            }
        }
        private double longitude1;//field
        public double longitude
        {
            get { return longitude1; }
            set
            {
                longitude1 = r.NextDouble() * (33.3 - 31) + 31;
            }
        }
        public string address { get; private set; }

        public override string ToString()
        {
            return "Bus Station Code: "+ BusStationKey + ", "+ latitude+ "°N "+longitude+ "°E"+"\n";
        }
        

    }
}
