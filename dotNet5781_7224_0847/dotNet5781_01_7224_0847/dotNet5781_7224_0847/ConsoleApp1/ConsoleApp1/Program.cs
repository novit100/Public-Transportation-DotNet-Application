using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace dotNet5781_01_7224_0847
{
    class Bus
    {
        public string License_num = " ";                   //license number
        public DateTime Start_d = new DateTime();   //starting date
        public static long Km_all_buses = 0;               //total kilometrage of all the buses
        public DateTime last_care_d = new DateTime();
        public long Km;    //kilometrage of one bus
        public int Km_since_care;
        public int Km_since_fuel;
    }

    class Program
    {
        private static int numBusesAdded;
        private static List<Bus> buses = new List<Bus>();
        /*     public static long Km_all_buses = 0;  */             //total kilometrage of all the buses
        private static Random r = new Random(DateTime.Now.Millisecond);
        //public enum options { exit, add_bus, choose_bus, fuelOrcare, showKmAll };
        static void Main(string[] args)
        {
            int op;

            do
            {
                Console.WriteLine(@"
                    to add a bus press 1
                    to choose a bus press 2
                    to fuel or take care of the bus press 3
                    to show the total kilometrage of the buses since the last care press 4
                    to exit press 0 
                    ");
                string temp = Console.ReadLine();

                bool flag = int.TryParse(temp, out op);


                switch (op)
                {
                    case 1:
                        add_bus();
                        break;
                    case 2:
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

        private static void add_bus()
        {
            Console.WriteLine("please enter the licence number:");
            string licenceNumber = Console.ReadLine();
            Console.WriteLine("please enter the start date of the bus:");
            string tmp = Console.ReadLine();
            DateTime startDate;
            bool flag = DateTime.TryParse(tmp, out startDate);
            buses.Insert(numBusesAdded, new Bus() { License_num = licenceNumber, Start_d = startDate });
            int op1;

            do
            {
                Console.WriteLine(@"
                    to add the kilometrage of the bus press 1
                    to the kilometrage of the bus since care press 2
                    to the kilometrage of the bus since last fuel press 3
                    else press 0 
                    ");

                string temp = Console.ReadLine();
                bool flag1 = int.TryParse(temp, out op1);
                switch (op1)
                {
                    case 1:
                        Console.WriteLine("What is the kilometrage of the bus?\n");
                        temp = Console.ReadLine();
                        int km;
                        flag1 = int.TryParse(temp, out km);
                        buses[numBusesAdded].Km = km;
                        break;
                    case 2:
                        Console.WriteLine("What is the kilometrage of the bus since last care?\n");
                        temp = Console.ReadLine();
                        int kmSinceCare;
                        flag1 = int.TryParse(temp, out kmSinceCare);
                        buses[numBusesAdded].Km_since_care = kmSinceCare;
                        //Km_all_buses += kmSinceCare;
                        break;
                    case 3:
                        Console.WriteLine("What is the kilometrage of the bus since last fuel?\n");
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
            Console.WriteLine("bus added successfully\n");
            numBusesAdded++;

        }

        private static void choose_bus()
        {
            Console.WriteLine("please enter the licence number:");
            string licenceNumber = Console.ReadLine();
            bool flag = false;
            int i;
            for (i = 0; i < buses.Count; i++)
            {
                if (buses[i].License_num == licenceNumber)
                    flag = true;
            }
            if (flag == false)
            {
                Console.WriteLine("license number not found\n");
                return;
            }
            int current_ride_length = r.Next();
            if (
                buses[i].Km_since_care + current_ride_length >= 20000 || (DateTime.Now - buses[i].last_care_d).TotalDays >= 365)
            {
                Console.WriteLine("the bus cannot ride this length without a treatment\n");
                return;
            }
            if (buses[i].Km_since_fuel + current_ride_length > 1200)
            {
                Console.WriteLine("the bus cannot ride this length without fueling first\n");
                return;
            }
            //if all parameters allow riding:
            buses[i].Km_since_care += current_ride_length;
            buses[i].Km_since_fuel += current_ride_length;
            buses[i].Km += current_ride_length;//update kilometrage
            //Km_all_buses += current_ride_length; 

        }

        private static void fuelOrcare()
        {
            Console.WriteLine("please enter the licence number:");
            string licenceNumber = Console.ReadLine();
            bool flag = false;
            int i;
            for (i = 0; i < buses.Count; i++)
            {
                if (buses[i].License_num == licenceNumber)
                    flag = true;
            }
            if (flag == false)
            {
                Console.WriteLine("license number not found\n");
                return;
            }
            Console.WriteLine("for treatment press 1, for fueling press 2\n");
            string ans = Console.ReadLine();
            if (ans == "1")
            {
                //Km_all_buses -= buses[i].Km_since_care;
                buses[i].Km_since_care = 0;
                buses[i].last_care_d = DateTime.Now;
            }
            if (ans == "2")
                buses[i].Km_since_fuel = 0;
            return;

        }

        private static void showKmAll()
        {
            for (int i = 0; i < buses.Count(); i++)
            {
                Console.WriteLine("licence number: " + buses[i].License_num + "/t" + "kilometers since last treatment: " + buses[i].Km_since_care + "\n");
            }
        }
    }

}