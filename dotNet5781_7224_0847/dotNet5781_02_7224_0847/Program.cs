﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// this project is programmed to be a platform which we will be able to actualize a system 
/// of buses.
/// we assumed in our project that the user does not change the first or last 
/// bus stop, because they are what define the essence of the specific bus line. 
/// we assume that 2 stations with the same key can appear in 2 buses with different areas,
/// since there are interurbun buses, that statrt from the same city but travel to different areas.
/// 
/// </summary>
namespace dotNet5781_02_7224_0847
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "                                          " +
                "                                                 Esti's ans Nov's second bus project";
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.White;

            BusLineCollections coll = new BusLineCollections();

            //initialize(coll);

            int num = -1;
            Options op;

            do
            {
                Console.WriteLine(@"
                    to add a new bus line press 1
                    to add a new bus stop press 2
                    to to delete a bus line press 3
                    to to delete a bus station from a bus line path press 4
                    to search for buses that get to a certain station press 5
                    to search for a direct path between two bus stations press 6
                    for printing all of the bus lines press 7
                    for printing the whole list of stations and the bus line that approach them press 8
                    to exit press 0
                    ");

                string str = Console.ReadLine();
                int.TryParse(str, out num);
                while (!(Enum.TryParse(str, true, out op)) || num < 0 || num > 8)
                {
                    Console.WriteLine("ERROR! enter your choice again");
                    str = Console.ReadLine();
                    int.TryParse(str, out num);
                }

                switch (op)
                {
                    case Options.AddBus:
                        try//1
                        {
                            coll.addNewBusToCollection();
                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case Options.AddStop:
                        try//2
                        {
                            coll.addStationToBus();
                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                        break;
                    case Options.DeleteBusLine:
                        try//3
                        {
                            coll.deleteBusFromCollection();
                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case Options.DeleteBusStation:
                        try//4
                        {
                            coll.delStationFromBus();
                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case Options.SearchBuses:
                        try //5
                        {
                            Console.WriteLine("please enter the key of the bus station you want to search buses that get to");
                            int key = ReceiveInt();
                            coll.busesPassInStation(key);
                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case Options.SearchPath://6
                        try
                        {
                            Console.WriteLine("please enter the code of the first and last bus station of the path you want ");
                            int k1 = ReceiveInt();
                            int k2 = ReceiveInt();
                            coll.returnSortedPathes(k1, k2);

                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        catch
                        { throw new BusException("unknown error"); }
                        break;

                    case Options.PrintBusLines://7
                        try
                        {
                            foreach (BusLine item in coll)
                            {
                                Console.WriteLine(item.ToString()+'\n'); 
                            }
                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;

                    case Options.PrintAllStations://8
                        try
                        {
                            List<BusLineStation> allstat = new List<BusLineStation>();//contains all the stat but only once

                            foreach (BusLine bus in coll)
                            {
                                foreach (BusLineStation stat in bus.Stations)
                                {
                                    bool ifExistInAllStat = false;
                                    foreach (BusLineStation s in allstat)
                                    {
                                        if (s.BusStationKey == stat.BusStationKey)
                                            ifExistInAllStat = true;
                                    }
                                    if(!ifExistInAllStat)//add only stations that dont exist allready in allstat
                                        allstat.Add(stat);
                                }
                            }
                            foreach (BusLineStation item in allstat)
                            {
                                Console.WriteLine("bus station " + item.BusStationKey+":");
                                coll.busesPassInStation(item.BusStationKey);
                                Console.WriteLine("\n");
                            }
                        }
                        catch(BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                        

                    default: break;
                        
                }
                
            }
            while (op != Options.exit);
            Console.ReadKey();
        }

        public static int ReceiveInt()
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

    }
}
