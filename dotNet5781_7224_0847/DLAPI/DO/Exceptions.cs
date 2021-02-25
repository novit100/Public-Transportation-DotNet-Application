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
        public LineException(int busNumber) : base() => BUSNUMBER = busNumber;//quick initialization and call to father("base") ctor
        public LineException(int busNumber, string message) :
            base(message) => BUSNUMBER = busNumber;
        public LineException(int busNumber, string message, Exception innerException) :
            base(message, innerException) => BUSNUMBER = busNumber;
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
        public LineStationException(int code, int busNumber) : base() { CODE = code; BUSNUMBER = busNumber; } //quick initialization and call to father("base") ctor
        public LineStationException(int code, int busNumber, string message) :
                base(message)
        { CODE = code; BUSNUMBER = busNumber; }
        public LineStationException(int code, int busNumber, string message, Exception innerException) :
            base(message, innerException)
        { CODE = code; BUSNUMBER = busNumber; }
        //public override string ToString() => base.ToString() + $", error in station that its code is: {CODE}";
        public override string ToString()
        {
            return Message + "\n";
        }
    }
    [Serializable]
    public class AppUserException : Exception
    {
        public string NAME;
        public AppUserException(string name) : base() => NAME = name;//quick initialization and call to father("base") ctor
        public AppUserException(string name, string message) :
            base(message) => NAME = name;
        public AppUserException(string name, string message, Exception innerException) :
            base(message, innerException) => NAME = name;
        //public override string ToString() => base.ToString() + $", error in line: {BUSNUMBER}";
        public override string ToString()
        {
            return Message + "\n";
        }
    }

    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }
}
