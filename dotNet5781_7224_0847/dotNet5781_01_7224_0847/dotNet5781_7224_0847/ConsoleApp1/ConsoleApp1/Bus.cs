using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Bus
    {
        public string License_num = " ";                   //license number
        public DateTime Start_d = new DateTime(0, 0, 0);   //starting date
        public static long Km_all_buses = 0;               //total kilometrage of all the buses
        public DateTime last_care_d = new DateTime(0, 0, 0);
        public long Km;    //kilometrage of one bus
        public int Km_since_care;
        public int Km_since_fuel;
    }
}
