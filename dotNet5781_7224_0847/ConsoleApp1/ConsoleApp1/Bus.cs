using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Bus
    {
        public int License_num   //license number
        {
            get { return License_num; }
            set
            {
                if (value < 1000000 || value > 99999999)//more than 8 digits or less than 7 digit (the 1st digit cannot be '0' because it won't print)
                    License_num = 1111111;
                else
                    License_num = value;//the parameter we got
            }
        }

        public DateTime Start_d { get; set; }              //starting activity day
        public DateTime last_care_d { get; set; }
        public long Km { get; set; }     //kilometrage of one bus
        public int Km_since_care { get; set; }
        public int Km_since_fuel { get; set; }
    }
}
