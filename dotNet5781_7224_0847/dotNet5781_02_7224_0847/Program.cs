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
            int op;
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

                bool flag = int.TryParse(temp, out op);

                BusLineCollections coll = new BusLineCollections();
                switch (op)
                {
                    case 1:
                       coll.addNewBusToCollection();
                        break;
                    case 2:
                        
                        break;
                    case 3:
                        
                        break;
                    case 4:
                        
                        break;
                    default: break;
                }
            }
            while (op != 0);
            Console.ReadKey();
        }

    }
}

