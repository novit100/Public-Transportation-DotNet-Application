using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// class BusLineCollections contains a collection of bus lines.
/// we declared part of the functions un this class
/// to be public so we would be able to approach them from 
/// the main.
/// </summary>

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
            if (newbusLine < 0)
                throw new BusException("invalid input for the busline key");
            int count = 0;//counts how many times the bus already appears in the collection
            foreach (BusLine item in 
                buses)//since BusLineCollections is enumerable, we can use it as a type (the list of buses itself)
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
            if (key1 == key2)
                throw new BusException("at least two bus stops in a path");
            /// <summary>
            ///  אם ישלנו את הקו הזה כבר פעם אחת וגם התחנה הראשונה שאני רוצים להוסיף שווה לתחנה הראשונה של הקו הקיים 
            ///אבל מה עם המצב שיש לנו את הקו הזה פעם אחת וגם המספר תחנה האחרון לא שווה למספר תחנה הראשון שאני רוצים להוסיף?!?
            ///אז מה שצריך לעשות זה להוסיף פשוט עוד תנאי איפ שאומר שאם המספר קו שווה למספר קו וגם התחנה הראשונה שאני רוצים להוסיף לא שווה לסופי אז תזרוק חריגה- אחרת תאפשר להכניס !  
            /// </summary>
            if (count==1)//the bus exist once
            {
                foreach (BusLine item in buses)
                {
                    if (item.busLine == newbusLine && item.FirstStation.BusStationKey == key1 )
                    {
                        throw new BusException("cannot add the bus since the same bus to the same direction already exists");
                    }
                    else if(item.busLine == newbusLine&& (item.LastStation.BusStationKey != key1||key2!= item.FirstStation.BusStationKey))
                        throw new BusException("cannot add the bus since the same bus reffers to another path");
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

        public void busesPassInStation(int key)
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

        public void returnSortedPathes(int key1, int key2)
        {
            List<BusLine> sortedCollection = new List<BusLine>();

            foreach (BusLine BusLineItem in buses)
            {
                int indkey1=-1;
                int indkey2=-1;
                for(int i=0; i<BusLineItem.Stations.Count; i++)//passes on all stat of a bus
                {
                    if (BusLineItem.Stations[i].BusStationKey == key1)
                        indkey1 = i;
                    if (BusLineItem.Stations[i].BusStationKey == key2)
                        indkey2 = i;
                }
                if(indkey1==-1||indkey2==-1)
                {
                    //the bus path doesnt contain the stations
                }
                if(indkey1>indkey2)
                {
                    //the bus contains the stations but not to the wanted direction
                }
                if(indkey1<indkey2)
                {
                    BusLine bus = BusLineItem.returnSubPath(key1, key2, indkey1, indkey2, BusLineItem);
                    sortedCollection.Add(bus);              
                }
            }

            if (sortedCollection.Count == 0)
            {
                throw new BusException("no buses pass from station " + key1 + " to station " + key2);
            }
            sortedCollection.Sort();//since we initialized icomparable interface in busLine and it compares two buses by the time, it sorts buses by the time

            foreach (BusLine item in sortedCollection)
            {
                Console.WriteLine(item.ToString()+"\n"+"travel time: "+item.time(key1,key2)+" minutes\n");
            }
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
