using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_0847_7224
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome7224();
            Welcome0847();

            Console.ReadKey();
        }

        private static void Welcome0847()
        {
            Console.WriteLine("Enter your name: ");
            String n = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", n);
        }
        static partial void Welcome7224();
    }
}
