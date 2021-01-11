using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class StationException : Exception
    {
        public int CODE;
        public StationException(int code) : base() => CODE = code;//quick initialization and call to father("base") ctor
        public StationException(int code, string message) : 
            base(message) => CODE = code;
        public StationException(int code, string message, Exception innerException) : 
            base(message, innerException) => CODE = code;
        public override string ToString() => base.ToString() + $", error in station that its code is: {CODE}";
    }

    [Serializable]
    public class LineException : Exception
    {
        public int BUSNUMBER;
        public LineException(int busNumber) : base() => BUSNUMBER = busNumber;//quick initialization and call to father("base") ctor
        public LineException(int busNumber, string message) :
            base(message) => BUSNUMBER = busNumber;
        public LineException(int busNumber, string message, Exception innerException) :
            base(message, innerException) => BUSNUMBER = busNumber;
        public override string ToString() => base.ToString() + $", error in line: {BUSNUMBER}";
    }
}
