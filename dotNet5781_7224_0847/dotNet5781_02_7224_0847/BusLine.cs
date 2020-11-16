using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7224_0847
{/// <summary>
/// the class busline presents an object of a bus path, it is presented by the code of the path, 
///the first and last bus station and the area of the path.
///it also contains different tipes of methodes that allowes us to do different actions on the current layout 
/// </summary>
    class BusLine:IComparable<BusLine>
    {
        public List<BusLineStation> stations { get; set; } //the field of the bus line,it is performed by (many) single bus line stations

        ///////////////////////////////////////////////////////
        private int busLineKey;

        ///////////////////////////////////////////////////////
        private BusLineStation FirstStation;

        ///////////////////////////////////////////////////////
        private BusLineStation LastStation;

        ///////////////////////////////////////////////////////
        public enum Ereas
        {
            General, North, South, Center, Jerusalem
        }

        ///////////////////////////////////////////////////////
        private Ereas area;

        ///////////////////////////////////////////////////////

        public override string ToString()
        {
            String s = "";
            foreach (BusLineStation item in BusLine)
            {
                s += item.BusStationKey+" , ";
            }
            return "bus number: " + busLineKey + ", area: " + area + "\n" + "bus station numbers: " +s;


        }
        ///////////////////////////////////////////////////////
        private void deleteBusStationFromBusLine()
        {
            Console.WriteLine("please enter the bus station number that you intend to delete: ");
            int stationCode = ReceiveStationCode();
            for (int i = 0; i < stations.Count; i++)
            {
               if (stations[i].BusStationKey == stationCode)
                {
                    stations.Remove(stations[i]);
             }
            }
        }
        //////////////////////////////////////////////////////
        private void AddBusStationFromBusLine()
        {
            Console.WriteLine("please enter the bus station number that you intend to add a station after it : ");
            int stationCode = ReceiveStationCode();
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stationCode)
                {
                    BusLineStation bus = new BusLineStation();
                    bus.Distance = 2.5;

                    stations.Add(stations[i + 1]);
                }
            }
        }
        //////////////////////////////////////////////////////
        private int ReceiveStationCode()
        {
            String s = Console.ReadLine();
            int x;
            bool b = int.TryParse(s, out x);
            if (!b)
            {
                throw new BusException("invalid input");
            }
            return x;
        }

        public int CompareTo(BusLine other)
        {
            double sum = 0.0;
            double sum1 = 0.0;
            foreach (BusLineStation item in BusLine)
            {
                sum += item.Time;
            }
            foreach (BusLineStation item in other)
            {
                sum1 += item.Time;
            }
            return sum.CompareTo(sum1);
        }
    }
}
