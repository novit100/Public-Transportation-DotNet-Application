﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Station
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        IEnumerable<Line> linesInStation { set; get; }
        //IEnumerable<LinePassesInStation> linesInStation { set; get; }
    }
}
