using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_7224_0847
{
    class Bus
    {
        public string License_num = " ";                   //license number
        public DateTime Start_d =new DateTime ( 0,0,0);   //starting date
        public DateTime last_care_d = new DateTime(0, 0, 0);
        public long Km;    //kilometrage of one bus
        public int Km_since_care; 
        public int Km_since_fuel;


       
    }
}
