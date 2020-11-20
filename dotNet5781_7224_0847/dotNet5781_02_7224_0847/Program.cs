using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dotNet5781_02_7224_0847
{
    class Program
    {
        static void Main(string[] args)
        {
            Options op;
            do
            {
                Console.WriteLine(@"
                    to add a new bus line press 1
                    to sdd a new bus stop press 2
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

                BusLineCollections coll = new BusLineCollections();
                switch (op)
                {
                    case Options.AddBus:
                        try
                        {
                            coll.addNewBusToCollection();
                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case Options.AddStop:
                        try
                        {
                            coll.addStationToBus();
                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                        break;
                    case Options.DeleteBusLine:
                        try
                        {
                            coll.deleteBusFromCollection();
                        }
                        catch (BusException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case Options.DeleteBusStation:


                        break;
                    case Options.SearchBuses:

                        break;
                    case Options.SearchPath:

                        break;

                    case Options.PrintBusLines:
                        break;

                    case Options.PrintAll:
                        break;


                    default: break;
                }
            }
            while (op != Options.exit);
            Console.ReadKey();
        }

    }
}
