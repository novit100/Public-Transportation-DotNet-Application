using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace dotNet5781_02_7224_0847
{
    [Serializable]
    public class MyExeption: Exception
    {
        public MyExeption() : base() { }
        public MyExeption(string message) : base(message) { }
        public MyExeption(string message, Exception inner) : base(message, inner) { }
        protected MyExeption(SerializationInfo info, StreamingContext context)
     : base(info, context) { }

        override public string ToString()
        { return  Message; }


    }
}
