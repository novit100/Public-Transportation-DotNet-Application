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
    class Program
    {
        private static int numBusesAdded;
        private static List<Bus> buses = new List<Bus>();
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
            string licenceNumber;
            DateTime startDate;
            ///////////////////
            do
            {
                Console.WriteLine("please enter the licence number:");
                licenceNumber = Console.ReadLine();
                Console.WriteLine("please enter the start date of the bus:");
                string tmp = Console.ReadLine();
                bool flag = DateTime.TryParseExact(tmp, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out startDate);
              
                if (!checkLicenseNum(startDate, licenceNumber))
                    Console.WriteLine("an error has been caused");
            } while (!checkLicenseNum(startDate, licenceNumber));


            buses.Insert(numBusesAdded, new Bus() { License_num = licenceNumber, Start_d = startDate, last_care_d = startDate, Km = 0, Km_since_care = 0, Km_since_fuel=0});  ;
            //default value of last care date= start date. if the user restarts it later, it becomes the updated date.
            //same with km since care and fuel=0, till the user updates them.

            int op1;
            do
            {
                Console.WriteLine(@"
                    to  add the kilometrage of the bus press 1
                    to  add the kilometrage of the bus since care press 2
                    to  add the kilometrage of the bus since last fuel press 3
                    else press 0
                    ");

                string temp = Console.ReadLine();
                bool flag1 = int.TryParse(temp, out op1);
                switch (op1)
                {
                    case 1:
                        Console.WriteLine("What is the kilometrage of the bus?");
                        temp = Console.ReadLine();
                        int km;
                        flag1 = int.TryParse(temp, out km);
                        buses[numBusesAdded].Km = km;
                        break;
                    case 2:
                        Console.WriteLine("What is the kilometrage of the bus since last care?");
                        temp = Console.ReadLine();
                        int kmSinceCare;
                        flag1 = int.TryParse(temp, out kmSinceCare);
                        buses[numBusesAdded].Km_since_care = kmSinceCare;
                        break;
                    case 3:
                        Console.WriteLine("What is the kilometrage of the bus since last fuel?");
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
            Console.WriteLine("bus added successfully");
            numBusesAdded++;

        }

        private static void choose_bus()
        {
            Console.WriteLine("please enter the licence number:");
            string licenceNumber = Console.ReadLine();
            bool flag = false;
                                                                                        //bus.getLastTreatment()<=today.AddYears(-1))
            int current_ride_length = r.Next(20000);
            Console.WriteLine("the lentgh of the current ride is "+ current_ride_length+" km");
            foreach (Bus any in buses)
            {
                if (any.License_num == licenceNumber)
                {
                    flag = true;
                    if ((any.Km_since_care + current_ride_length) > 20000 || (DateTime.Now - any.last_care_d).TotalDays > 365)
                    {
                        Console.WriteLine("the bus cannot ride this length without a treatment");
                    }
                    if (any.Km_since_fuel + current_ride_length > 1200)
                    {
                        Console.WriteLine("the bus cannot ride this length without fueling first");
                    }
                    if(any.Km_since_fuel + current_ride_length <= 1200 && any.Km_since_care + current_ride_length <= 20000 && (DateTime.Now - any.last_care_d).TotalDays <= 365)
                    {
                        //if all parameters allow riding:
                        any.Km_since_care += current_ride_length;
                        any.Km_since_fuel += current_ride_length;
                        any.Km += current_ride_length;//update kilometrage
                        Console.WriteLine("have a nice ride:)");
                    }
                }
            } 
            if (flag == false)
            {
                Console.WriteLine("license number not found");
                return;
            }

        }

        private static void fuelOrcare()
        {
            Console.WriteLine("please enter the licence number:");
            string licenceNumber = Console.ReadLine();
            bool flag = false;
            foreach (Bus any in buses)
            {
                if (any.License_num == licenceNumber)
                {
                    flag = true;
                    Console.WriteLine("for treatment press 1, for fueling press 2, for both press 3");
                    string ans = Console.ReadLine();
                    if (ans == "1")
                    {
                        any.Km_since_care = 0;
                        any.last_care_d = DateTime.Now;
                        return;
                    }
                    if (ans == "2")
                    {
                        any.Km_since_fuel = 0;
                        return;
                    }
                    if (ans=="3")
                    {
                        any.Km_since_fuel = 0;
                        any.Km_since_care = 0;
                        any.last_care_d = DateTime.Now;
                        Console.WriteLine("have a nice ride:)");
                    }
                }
            }
            if (flag == false)
            {
                Console.WriteLine("license number not found");
            }

        }

        private static void showKmAll()
        {
            foreach(Bus any in buses)
            {
                Console.WriteLine("licence number:");
                if(any.License_num.Count()==7)
                {
                    string s1= any.License_num.Substring(0,2);
                    string s2 = any.License_num.Substring(2,3);
                    string s3 = any.License_num.Substring(5,2);
                    Console.WriteLine(s1+"-"+s2+"-"+s3);
                }
                if (any.License_num.Count() == 8)
                {
                    string s1 = any.License_num.Substring(0, 3);
                    string s2 = any.License_num.Substring(2, 2);
                    string s3 = any.License_num.Substring(5, 3);
                    Console.WriteLine(s1 + "-" + s2 + "-" + s3);
                }

                Console.WriteLine("kilometers since last treatment: ");  
                Console.WriteLine(any.Km_since_care);
            }
        }
        private static bool checkLicenseNum(DateTime date, string licensenum)
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


    }

}
