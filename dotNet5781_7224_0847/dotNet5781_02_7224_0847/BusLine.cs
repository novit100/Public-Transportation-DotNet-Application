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
            return "bus number: " + busLineKey + ", area: " + area + "\n" + stations.ToString();


        }
        ///////////////////////////////////////////////////////
        private void deleteBusStationFromBusLine()
        {
            Console.WriteLine("please enter the bus station number that you intend to delete: ");
            int stationCode = ReceiveStationCode();
            for (int i = 0; i < stations.Count; i++)
            {
                //if (stations[i].BusStationKey == stationCode)
                //{
                //    stations.Remove(stations[i]);
                //}
            }
        }
        //////////////////////////////////////////////////////
        private void AddBusStationFromBusLine()
        {

        }
        //////////////////////////////////////////////////////
        private int ReceiveStationCode()
        {
            String s = Console.ReadLine();
            int x;
            bool b = int.TryParse(s, out x);
            int y = int.Parse(s);
            return y;
        }

        public int CompareTo(BusLine other)
        {
            return (stations.time).CompareTo(other.stations.time);
        }
    }
}
