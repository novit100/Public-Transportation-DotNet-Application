using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_7224_0847
{
    class Program
    {
        private static int numBusesAdded;
        private static int op;
        private static int op1;
        private static List <Bus> buses;
        //public enum options { exit, add_bus, choose_bus, fuelOrcare, showKmAll };
        static void Main(string[] args)
        {

            do
            {
                string temp = Console.ReadLine();
                int op;
                bool flag = int.TryParse(temp, out op);
                Console.WriteLine(@"
                    to add a bus press 1
                    to choose a bus press 2
                    to fuel or take care of the bus press 3
                    to show the total kilometrage of the buses since the last care press 4
                    to exit press 0 
                    ");

                switch (op)
                {
                    case 0:
                        add_bus();
                        break;
                    case 1:
                        choose_bus();
                        break;
                    case 3:
                        fuelOrcare();
                        break;
                    case 4:
                        showKmAll();
                        break;
                    default: break;


                }


            }
            while (op != 0);

Console.ReadKey();

        }

        private static void add_bus() {
            Console.WriteLine("please enter the licence number:");
            string licenceNumber = Console.ReadLine();
            Console.WriteLine("please enter the start date of the bus:");
            string tmp = Console.ReadLine();
            DateTime startDate;
            bool flag = DateTime.TryParse(tmp, out startDate);
            buses.Insert(numBusesAdded, new Bus () { License_num = licenceNumber, Start_d = startDate });

            do
            {
                Console.WriteLine(@"
                    to add the kilometrage of the bus press 1
                    to the kilometrage of the bus since care press 2
                    to the kilometrage of the bus since last fuel  press 3
                    else press 0 
                    ");
                
                string temp = Console.ReadLine();
                int op1;
                bool flag1 = int.TryParse(temp, out op1);
                switch (op1)
                {
                    case 0:
                        break;
                    case 1:
                        temp = Console.ReadLine();
                        int km;
                        flag1 = int.TryParse(temp, out km);
                        buses[numBusesAdded].Km = km;
                        break;
                    case 2:
                        temp = Console.ReadLine();
                        int kmSinceCare;
                        flag1 = int.TryParse(temp, out kmSinceCare);
                        buses[numBusesAdded].Km_since_care = kmSinceCare;
                        break;
                    case 3:
                        temp = Console.ReadLine();
                        int kmSinceFuel;
                        flag1 = int.TryParse(temp, out kmSinceFuel);
                        buses[numBusesAdded].Km_since_fuel = kmSinceFuel;
                        break;
                    default:
                        break;



                }


            }
            while (op1 != 0);
            numBusesAdded++;
            return; }



    


    }
        
    
}
