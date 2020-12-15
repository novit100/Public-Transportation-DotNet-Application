using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex1_Thread
{
    class Program
    {
        static bool bContinue = true;

        private static void Second()
        {
            while (bContinue)
            {
                Console.WriteLine(Thread.CurrentThread.Name);
                Thread.Sleep(1000);
            }
            Console.WriteLine("Bye Bye from " + Thread.CurrentThread.Name);
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main Thread";

            Thread t1 = new Thread(Second);
            t1.Name = "Second Thread";
            t1.Start();

            Console.WriteLine("***** Main Thread continue ******");
            Console.WriteLine("Stop the Second Thread? (Y/N)");
            string answer = Console.ReadLine();
            while (answer != "Y")
            {
                Thread.Sleep(2000);
                Console.WriteLine("Stop the Second Thread? (Y/N)");
                answer = Console.ReadLine();
            }

            bContinue = false;
            Console.WriteLine("Bye Bye from " + Thread.CurrentThread.Name);
            Console.ReadKey();
        }
    }
}

/*
 
***** Main Thread continue ******
Stop the Second Thread? (Y/N)
Second Thread
Second Thread
Second Thread
Second Thread
N
Second Thread
Second Thread
Second Thread
Stop the Second Thread? (Y/N)
Second Thread
Second Thread
Second Thread
N
Second Thread
Second Thread
Stop the Second Thread? (Y/N)
Second Thread
Second Thread
Second Thread
Y
Second Thread

Bye Bye from Second Thread
Bye Bye from Main Thread


*/
