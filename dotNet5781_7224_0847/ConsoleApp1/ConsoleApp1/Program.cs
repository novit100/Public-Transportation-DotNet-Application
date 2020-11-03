using ConsoleApp1;
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

    enum Options {exit, add, choose, fuelOrCare, showKm };

    class Program
    {
        static Random r = new Random(DateTime.Now.Millisecond);

        List<Bus> buses = new List<Bus>();

        static void Main(string[] args)
        {
            Options op;

            do
            {
                Console.WriteLine(@"
to add a bus press 1
to choose a bus press 2
to fuel or take care of the bus press 3
to show the total kilometrage of the buses since the last care press 4
to exit press 0 
                    ");
                string tmp = Console.ReadLine();
                op=(Options)Convert.ToInt32(tmp);//convert to op


                switch (op)
                {
                    case Options.add:
                        add_bus();
                        break;
                    case Options.choose:
                        choose_bus();
                        break;
                    case Options.fuelOrCare:
                        fuelOrcare();
                        break;
                    case Options.showKm:
                        showKmAll();
                        break;
                    default: break;
                }
            }
            while (op != 0);
            Console.ReadKey();
        }

        static void add_bus()
        {
            Console.WriteLine("please enter the licence number:");
            Bus busAdded = new Bus();
            busAdded.License_num = getNum();//get num worry to restart license num
            foreach(Bus any in buses)
            {
                if (any.License_num == busAdded.License_num)
                    return;//bus already exists and no need to add it 
            }

            Console.WriteLine("please enter the start date of the bus:");
            busAdded.Start_d = getDate();//get date worries to restart date
            Console.WriteLine("want to add more details? Y/N");
            string ans = Console.ReadLine();
            if (ans == "N")
                return;
            Console.WriteLine("What is the kilometrage of the bus?");
            busAdded.Km = getNum();
            Console.WriteLine("What is the kilometrage of the bus since last care?");
            busAdded.Km_since_care = getNum();
            Console.WriteLine("What is the kilometrage of the bus since last fuel?");
            busAdded.Km_since_fuel = getNum();
           
            buses.Add(busAdded);
        }

       static int getNum()
        {
            int num;
            bool flag = true;
            do
            {
                if (!flag)               //illegal num- with letters
                    Console.WriteLine("ERROR, please enter again the licence number");
                string tmp = Console.ReadLine();
                flag = int.TryParse(tmp, out num);

            } while (!flag);
            return num;
        }

        static DateTime getDate()
        {
            DateTime date;
            bool flag = true;
            do
            {
                if (!flag)               //illegal date- with letters
                    Console.WriteLine("ERROR, please enter again the start date of the bus");
                string tmp = Console.ReadLine();
                flag = DateTime.TryParseExact(tmp, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date);

            } while (!flag);
            return date;
        }

        static bool checkLicenseNum(DateTime date, string licensenum)
        {

            if (licensenum.Count() == 7 && date.Year < 2018)
            {
                return true;
            }

            if (licensenum.Count() == 8 && date.Year > 2018)
            {
                return true;
            }
            return false;

        }


        static void choose_bus()
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
            if (buses[i].Km_since_care + current_ride_length >= 20000 || (DateTime.Now - buses[i].last_care_d).TotalDays >= 365))
)
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
        }

        static void fuelOrcare()
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

        static void showKmAll()
        {
            for (int i = 0; i < buses.Count(); i++)
            {
                Console.WriteLine("licence number: " + buses[i].License_num + "/t" + "kilometers since last treatment: " + buses[i].Km_since_care + "\n");
            }
        }
    }

}