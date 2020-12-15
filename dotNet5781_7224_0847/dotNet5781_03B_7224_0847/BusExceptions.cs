using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace dotNet5781_03B_7224_0847
{/// <summary>
/// here we keep our exceptions,
/// it is an estetic and common way to write the code.
/// we overrided the func to string so we can use the messages 
/// that we've chosen.
/// </summary>
    class BusException :Exception
    {

        public BusException() : base() { }
        public BusException(string message) : base(message) { }
        public BusException(string message, Exception inner) : base(message, inner) { }
        protected BusException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
        

        override public string ToString()
        { return Message; }

    }
}
