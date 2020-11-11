using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7224_0847
{
    class BusLine
    {
        private int busLineKey;
        private BusLineStation FirstStation;
        private BusLineStation LastStation;

        enum Ereas
        {
            General, North, South, Center, Jerusalem
        }

        private Ereas area;
        List<BusLineStation> stations;
        public override string ToString()
        {
            Console.WriteLine("bus number: " + busLineKey + ", area: " + area + "\n");
            int count = 1;
            return foreach (BusLineStation item in stations) { item};

        }

    }
}
