using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_7224_0847
{
    class Bus
    {
        public string License_num { get; set; }         //license number
        public DateTime Start_d { get; set; }           //starting activity day
        public DateTime last_care_d { get; set; }
        public long Km { get; set; }                    //kilometrage of one bus
        public int Km_since_care { get; set; }
        public int Km_since_fuel { get; set; }
    }
}
