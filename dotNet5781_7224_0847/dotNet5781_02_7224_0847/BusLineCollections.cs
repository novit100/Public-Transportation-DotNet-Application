using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7224_0847
{
    class BusLineCollections: IEnumerable
    {
        List<BusLine> buses = new List<BusLine>();

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < buses.Count; i++)
                yield return buses[i];
        }

        public void addNewBusToCollection()
        {
            Console.WriteLine("please enter the bus line you want to add to the collection: ");
            int newbusLine = ReceiveInt();

            int count = 0;//counts how many times the bus already appears in the collection
            foreach (BusLine item in buses)//since BusLineCollections is enumerable, we can use it as a type (the list of buses itself)
            {
                if (item.busLine == newbusLine)
                {
                    count++;
                }
            }

            Console.WriteLine("Enter the key of the first station");
            int key1 = ReceiveInt();
            Console.WriteLine("Enter the key of the last station");
            int key2 = ReceiveInt();

            if (count==1)//the bus exist once
            {
                foreach (BusLine item in buses)
                {
                    if (item.busLine == newbusLine && item.FirstStation.BusStationKey==key1)
                    {
                        throw new BusException("cannot add the bus since the same bus to the same direction already exists");
                    }
                }
            }

            if(count==2)
                throw new BusException("cannot add the bus since we already have the back and forth path of it");
          
            //if reached here- add the bus.
            Console.WriteLine(@"
choose the bus area:
press 0- gereral area
press 1- North
press 2- South
press 3- Center
press 4- Jerusalem
                ");

            string str = Console.ReadLine();
            Areas choose = (Areas)Enum.Parse(typeof(Areas), str);

            List<BusLineStation> stat = new List<BusLineStation>();
            BusLineStation first = new BusLineStation(key1, true);
            BusLineStation last = new BusLineStation(key2, false);
            stat.Add(first);
            stat.Add(last);
            BusLine bus = new BusLine() { Stations = stat, busLine = newbusLine, FirstStation = first, LastStation = last, Area = choose };

            buses.Add(bus);//add the new bus to the collection
        }

        public void deleteBusFromCollection()
        {
            Console.WriteLine("please enter the bus line you want to delete from the collection: ");
            int delbusLine = ReceiveInt();

            foreach  (BusLine item in buses)
            {
                if (item.busLine == delbusLine)
                {
                    buses.Remove(item);//it will remove the bus line to both directions
                }
            }
        }

        public BusLine this[int index]//indexer
        {
            get
            {
                if (index <= buses.Count - 1)
                    return buses[index];
                else
                    throw new BusException("no bus exists in this index in the buses collection");
            }
            set
            {
                buses[index] = value;
            }
        }

        private void busesPassInStation(int key)
        {
            bool ifSomeonePasses = false;
            List<BusLine> list = new List<BusLine>();
            foreach (BusLine BusLineItem in buses)//for each bus line in the collection
            {
                bool flag = false;
                foreach (BusLineStation stationItem in BusLineItem.Stations)//for each station of a specific bus line
                {
                    if (stationItem.BusStationKey==key)
                    {
                        flag = true;
                        ifSomeonePasses = true;
                    }
                }
                if(flag)//if one of the bus stations is "key", the bus passes in that station
                {
                    Console.WriteLine("bus line "+BusLineItem.busLine+" passes in the station\n");
                }
            }
            if (!ifSomeonePasses)//if no bus passes the station
                throw new BusException("no bus passes the station");
        }

        public void addStationToBus()
        {
            Console.WriteLine("please enter the bus line you want to add a station to: ");
            int bus = ReceiveInt();

            foreach (BusLine BusLineItem in buses)//for each bus line in the collection
            {
                if(BusLineItem.busLine==bus)
                {
                    BusLineItem.AddBusStationToBusLine();
                }
            }
        }

        public void delStationFromBus()
        {
            Console.WriteLine("please enter the bus line you want to delete a station from: ");
            int bus = ReceiveInt();
            foreach (BusLine BusLineItem in buses)//for each bus line in the collection
            {
                if (BusLineItem.busLine == bus)
                {
                    BusLineItem.deleteBusStationFromBusLine();
                }
            }
        }

        private void sortAllBussesByTravelTime()
        {
            //List<BusLine> sortedCollection = new List<BusLine>();
            //sortedCollection= buses.Sort();//since we initialized icomparable interface in busLine and it compares two buses by the time, it sorts buses by the time

            buses.Sort();//since we initialized icomparable interface in busLine and it compares two buses by the time, it sorts buses by the time
        }

        private int ReceiveInt()
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
    }
}
