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
  public  class Bus
    {
        private string licenseNumber;
        public string LicenseNumber //license number
        {
            get { return licenseNumber; }
            set 
            {
                int tmp;
                bool flag = int.TryParse(value, out tmp);
                if (flag)//numbers only
                {
                    if (tmp >= 1000000 && tmp <= 9999999)//7 digits
                        licenseNumber = value.Substring(0, 2) + "-" + value.Substring(2, 3) + "-" + value.Substring(5, 2);
                    if (tmp >= 10000000 && tmp <= 99999999)//8 digits
                        licenseNumber = value.Substring(0, 3) + "-" + value.Substring(3, 2) + "-" + value.Substring(5, 3);
                }
                else//allready includes "-" between the numbers
                { 
                    if(value.Length==9)
                        licenseNumber = value.Substring(0, 2) + "-" + value.Substring(3, 3) + "-" + value.Substring(7, 2);
                    else if(value.Length==10)
                        licenseNumber = value.Substring(0, 3) + "-" + value.Substring(4, 2) + "-" + value.Substring(7, 3);
                    else { }//dont init yet since the value isnt valid
                }
            } 
        }         
        public DateTime Start_d { get; set; }           //starting activity day
        public DateTime last_care_d { get; set; }
        public long Km { get; set; }                    //kilometrage of one bus
        public int Km_since_care { get; set; }
        public int Km_since_fuel { get; set; }
       public Status status { get; set; }


    }
}
