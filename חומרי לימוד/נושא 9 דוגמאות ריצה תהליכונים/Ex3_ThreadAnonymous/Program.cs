using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex3_ThreadAnonymous
{
    class Program
    {
        static bool bContinue = true;

        static void Main(string[] args)
        {
            //lambda expression for smaller thread methods
            new Thread(() =>
            {
                while (bContinue)
                {
                    Console.WriteLine("I'm running on another thread!");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("ByeBye Anonymouse Thread");

            }).Start();

            Console.WriteLine("Waiting the anonymous thread to Stop...");
            Thread.Sleep(6000);
            bContinue = false;

            Console.WriteLine("ByeBye Main Thread");
            Console.ReadKey();

        }
    }
}


/*
I'm running on another thread!
Waiting the anonymous thread to Stop...
I'm running on another thread!
I'm running on another thread!
I'm running on another thread!
I'm running on another thread!
I'm running on another thread!
ByeBye Main Thread
ByeBye Anonymouse Thread
*/
