using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DO;

namespace BO
{
    [Serializable]
    public class StationException : Exception
    {
        public int CODE;
        public StationException(string message) : base(message) { }
        public StationException(string message, Exception innerException) : 
            base(message, innerException) => CODE = ((DO.StationException)innerException).CODE;
        //public override string ToString() => base.ToString() + $", error in station that its code is: {CODE}";
        public override string ToString()
        {
            return Message + "\n";
        }
    }

    [Serializable]
    public class LineException : Exception
    {
        public int BUSNUMBER;
        public LineException(string message) : base(message) { }
        public LineException(string message, Exception innerException) :
            base(message, innerException) => BUSNUMBER = ((DO.LineException)innerException).BUSNUMBER;
        //public override string ToString() => base.ToString() + $", error in line: {BUSNUMBER}";
        public override string ToString()
        {
            return Message + "\n";
        }
    }

    [Serializable]
    public class LineStationException : Exception
    {
        public int CODE;
        public int BUSNUMBER;
        public LineStationException(string message) : base(message) { }
        public LineStationException(string message, Exception innerException) :
            base(message, innerException)
        {
            CODE = ((DO.LineStationException)innerException).CODE;
            BUSNUMBER = ((DO.LineStationException)innerException).BUSNUMBER;
        }
        //public override string ToString() => base.ToString() + $", error in line: {BUSNUMBER}";
        public override string ToString()
        {
            return Message + "\n";
        }
    }

    [Serializable]
    public class AppUserException: Exception
    {
        public string NAME;
        public AppUserException(string message) : base(message) { }
        public AppUserException(string message, Exception innerException) :
            base(message, innerException) => NAME = ((DO.AppUserException)innerException).NAME;
   
        public override string ToString()
        {
            return Message + "\n";
        }
    }
}
