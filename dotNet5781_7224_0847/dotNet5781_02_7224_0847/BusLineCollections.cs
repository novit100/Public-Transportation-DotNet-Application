﻿using System;
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
            foreach (BusLine item in buses)//since BusLineCollections is enumerable, we can use it as a type (the list of buses itself)
            {
                if (item.busLine == newbusLine)
                {
                    count++;
                }
            }

            Console.WriteLine("Enter the key of the first station");
            int key1;
            bool f1 = int.TryParse(Console.ReadLine(), out key1);
            if (!f1||key1<0)
                throw new BusException("invalid input for the station key");

            Console.WriteLine("Enter the key of the last station");
            int key2;
            bool f2 = int.TryParse(Console.ReadLine(), out key2);
            if (!f2||key1<0)
                throw new BusException("invalid input for the station key");

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

        private void deleteBusFromCollection ()
        {

        }

        public BusLine this[int index]//indexer
        {
            get
            {
                return buses[index];
            }
            set
            {
                buses[index] = value;
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
