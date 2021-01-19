using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Line
    {
        public int LineId { get; set; }//running number. we need it also, because in different areas (and enen in the same area),
        //there can be 2 buses with the same busNumber. thats why we need the "LineId" also.
        public int BusNumber { get; set; }
        public Areas Area { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
    }

}
