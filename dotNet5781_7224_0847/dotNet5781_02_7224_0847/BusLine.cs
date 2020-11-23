using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// the class busline presents an object of a bus path, it is presented by the code of the path, 
///the first and last bus station and the area of the path.
///it also contains different tipes of methodes that allowes us to do different actions on the current layout 
/// </summary>
namespace dotNet5781_02_7224_0847
{
    class BusLine:IComparable<BusLine>//number of bus line lets say:"kav mispar--76" 
    {
        public List<BusLineStation> Stations { get; set; } //the field of the bus line,it is performed by (many) single bus line stations
        
        private int busLine1;
        public int busLine //number of bus line lets say:"kav mispar--76" 
        {
            get; set;
        }

        ///////////////////////////////////////////////////////
        private BusLineStation firstStation;
        public BusLineStation FirstStation
        {
            get; set;
        }

        ///////////////////////////////////////////////////////
        private BusLineStation lastStation;
        public BusLineStation LastStation
        {
            get; set;
        }

        ///////////////////////////////////////////////////////
        private Areas area;
        public Areas Area
        {
            get; set;
        }
        /////////////////////////////////////////////////////// 
        public override string ToString()
        {
            String s = "";
            foreach (BusLineStation item in Stations)
            {
                s += item.BusStationKey+", ";
            }
            return "bus number: " + busLine + ", area: " + Area + "\n" + "bus stations: " +s;
        }
        //////////////////////////////////////////////////////
        public void AddBusStationToBusLine()
        {
            Console.WriteLine("please enter the LOCATION IN PATH of the bus station that you want to add: ");
            int location = ReceiveInt();

            //we assume that we dont allow changing the 1st or last bus station in a bus, 
            //since they are imporrtant to distinguish the bus
            if (location == 1||location==Stations.Count+1)
                throw new BusException("cannot change the first or last bus station");

            if(location<1 || location>Stations.Count)//location 1 means the 1st station (index 0)
            {
                throw new BusException("invalid location, cannot insert");
            }

            Console.WriteLine("please enter the CODE of the bus station that you want to add: ");
            int key = ReceiveInt();
            foreach (BusLineStation item in Stations)
            {
                if (item.BusStationKey == key)
                    throw new BusException("bus station already exists in the bus-line path");
               
            }
            BusLineStation stat;
            stat = new BusLineStation(key, false);
            Stations.Insert(location - 1, stat);
            

        }
        ///////////////////////////////////////////////////////
        public void deleteBusStationFromBusLine()
        {
            Console.WriteLine("please enter the key of the bus station that you intend to delete: ");
            int key = ReceiveInt();

            if(!exist(key))
                throw new BusException("ERROR! bus station does not exist in the bus-line path");

            int index = 0;
            foreach (BusLineStation item in Stations)
            {
                if (item.BusStationKey == key)
                    break;
                index++;
            }
            if(index==0||index==Stations.Count-1)
                throw new BusException("ERROR! not allowed to delete the first or the last stations! they ditinguish the bus!");
           
            Stations.Remove(Stations[index]); 
        }
        //////////////////////////////////////////////////////
        
        public bool exist(int key)
        {
            foreach(BusLineStation item in Stations)
            {
                if (item.BusStationKey == key)
                    return true;
            }
            return false;
        }

        public double distance()
        {
            Console.WriteLine("please enter the key of the station you want to check the distance from: ");
            int key1 = ReceiveInt();
            Console.WriteLine("please enter the key of the station you want to check the distance to: ");
            int key2 = ReceiveInt();

            if (key1 == key2)//same station
                return 0;

            int key1Index=-1;
            int key2Index=-1;

            for(int i=0; i<Stations.Count; i++)
            {
                if (Stations[i].BusStationKey == key1)
                    key1Index = i;
                if (Stations[i].BusStationKey == key2)
                    key2Index = i;
            }

            if (key1Index ==-1|| key2Index==-1)//one or two of the stations werent found
                throw new BusException("ERROR! one or two of the stations werent found");

            double dis = 0;

            for (int i = key1Index+1; i < key2Index; i++)
            {
                dis += Stations[i].Distance;
            }
            return dis;
        }

        public TimeSpan time(int key1, int key2)
        {
            if (key1 == key2)//same station
                return new TimeSpan(0,0,0);

            int key1Index = -1;
            int key2Index = -1;

            for (int i = 0; i < Stations.Count; i++)
            {
                if (Stations[i].BusStationKey == key1)
                    key1Index = i;
                if (Stations[i].BusStationKey == key2)
                    key2Index = i;
            }

            if (key1Index == -1 || key2Index == -1)//one or two of the stations werent found
                throw new BusException("ERROR! one or two of the stations werent found");

            TimeSpan t=new TimeSpan(0,0,0);

            for (int i = key1Index +1; i <= key2Index; i++)
            {
                t += Stations[i].Time;
            }
            return t;
        }

        public BusLine returnSubPath(int key1, int key2, int indexkey1, int indexkey2, BusLine myBus)
        {
            List<BusLineStation> stat = new List<BusLineStation>();
            BusLineStation first = new BusLineStation(key1, true);
            stat.Add(first);

            int f = 1;

            for (int i = indexkey1+1; i <= indexkey2; i++)
            {
                stat.Insert(f, myBus.Stations[i]);
                f++;
            }
            BusLine bus = new BusLine() { Stations = stat, busLine = myBus.busLine, FirstStation = first, LastStation = stat[stat.Count-1], Area = myBus.Area };
            return bus;
        }

        private int ReceiveInt()
        {
            String s = Console.ReadLine();
            int x;
            bool b = int.TryParse(s, out x);
            if (!b||x<0)
            {
                throw new BusException("invalid input");
            }
            return x;
        }

        public int CompareTo(BusLine other)//compare 2 buses by their time travel
        {
            return time(this.FirstStation.BusStationKey, this.LastStation.BusStationKey).CompareTo(other.time(other.FirstStation.BusStationKey,other.LastStation.BusStationKey));
        }


    }
}


