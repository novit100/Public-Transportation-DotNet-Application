using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Line
    {
        public int LineId { get; set; }
        public int BusNumber { get; set; }
        IEnumerable<LineStation> lineStations { set; get; }
    }
}
