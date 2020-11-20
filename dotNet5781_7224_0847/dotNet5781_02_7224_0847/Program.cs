using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// this project is programmed to be a platform witch we will be able to actualize a system 
/// of buses.
/// we assumed in our project that the user does not change the first or last 
/// bus stop, because they are what define the essence of the specific bus line.
/// also we assume that the time is a function of distance. 
///there were 
/// </summary>
namespace dotNet5781_02_7224_0847
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Esti's ans Nov's second bus project";
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.White;
            BusLineCollections coll = new BusLineCollections();
            Options op;
            do
            {
                Console.WriteLine(@"
                    to add a new bus line press 1
                    to add a new bus stop press 2
                    to to delete a bus line press 3
                    to to delete a bus station from a bus line path press 4
                    to search buses that approach a certain station press 5
                    to search for a direct path between two bus stations press 6
                    for printing all of the bus lines press 7
                    for printing the whole list of stations and the bus line that approach them press 8
                    to exit press 0
                    ");
                string temp = Console.ReadLine();

                op = (Options)Enum.Parse(typeof(Options), temp);

               
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
                        catch
                        {
                            Console.WriteLine("error");
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
                            Console.WriteLine("please enter the key of the bus station you want to serch");
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
                        break;

                    case Options.PrintBusLines://7
                        try
                        {
                            foreach (BusLine item in coll)
                            {
                                Console.WriteLine(coll.ToString()+'\n'); 

                            }
                        }
                        catch (BusException ex)
                        {

                            Console.WriteLine(ex.ToString());
                        }
                        break;

                    case Options.PrintAll://8
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
            if (!b)
            {
                throw new BusException("invalid input");
            }
            return x;
        }
    }
}
