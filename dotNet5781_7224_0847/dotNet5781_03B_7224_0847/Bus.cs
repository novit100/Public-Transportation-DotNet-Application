using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03B_7224_0847
{
    public enum Status
    {
     TRY_ME  ,DRIVING  ,FUELING  ,IN_CARE  //STATUS OF BUS
    }
    public class Bus
    {
        public int LicenseNumber { get; set; }         //license number
        public DateTime Start_d { get; set; }           //starting activity day
        public DateTime last_care_d { get; set; }
        public long Km { get; set; }                    //kilometrage of one bus
        public int Km_since_care { get; set; }
        public int Km_since_fuel { get; set; }
        public Status status { get; set; }
        //    private static void fuelOrcare()
        //    {
        //        Console.WriteLine("please enter the licence number:");
        //        string licenceNumber = Console.ReadLine();
        //        bool flag = false;
        //        foreach (Bus any in buses)
        //        {
        //            if (any.License_num == licenceNumber)
        //            {
        //                flag = true;
        //                Console.WriteLine("for treatment press 1, for fueling press 2, for both press 3");
        //                string ans = Console.ReadLine();
        //                if (ans == "1")
        //                {
        //                    any.Km_since_care = 0;
        //                    any.last_care_d = DateTime.Now;
        //                    return;
        //                }
        //                if (ans == "2")
        //                {
        //                    any.Km_since_fuel = 0;
        //                    return;
        //                }
        //                if (ans == "3")
        //                {
        //                    any.Km_since_fuel = 0;
        //                    any.Km_since_care = 0;
        //                    any.last_care_d = DateTime.Now;
        //                    Console.WriteLine("have a nice ride:)");
        //                }
        //            }
        //        }
        //        if (flag == false)
        //        {
        //            Console.WriteLine("license number not found");
        //        }

        //    }

        //    private static void showKmAll()
        //    {
        //        foreach (Bus any in buses)
        //        {
        //            Console.WriteLine("licence number:");
        //            if (any.License_num.Count() == 7)
        //            {
        //                string s1 = any.License_num.Substring(0, 2);
        //                string s2 = any.License_num.Substring(2, 3);
        //                string s3 = any.License_num.Substring(5, 2);
        //                Console.WriteLine(s1 + "-" + s2 + "-" + s3);
        //            }
        //            if (any.License_num.Count() == 8)
        //            {
        //                string s1 = any.License_num.Substring(0, 3);
        //                string s2 = any.License_num.Substring(2, 2);
        //                string s3 = any.License_num.Substring(5, 3);
        //                Console.WriteLine(s1 + "-" + s2 + "-" + s3);
        //            }

        //            Console.WriteLine("kilometers since last treatment: ");
        //            Console.WriteLine(any.Km_since_care);
        //        }
        //    }
        //    private static bool checkLicenseNum(DateTime date, string licensenum)
        //    {

        //        if (licensenum.Count() == 7 && date.Year < 2018)
        //        {
        //            return true;
        //        }

        //        if (licensenum.Count() == 8 && date.Year > 2018)
        //        {
        //            return true;
        //        }
        //        return false;

        //    }

    }
}
