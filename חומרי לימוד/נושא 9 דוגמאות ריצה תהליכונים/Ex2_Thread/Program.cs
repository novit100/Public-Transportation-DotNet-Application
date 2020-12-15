using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex2_Thread
{
    class Program
    {
        private static void run(object obj)
        {
            while (true)
            {
                Console.Write(obj);
                Thread.Sleep(1000);
            }
        }

        static void Main(string[] args)
        {
            Thread t1 = new Thread(run);
            t1.Start("A");

            Thread t2 = new Thread(run);
            t2.Start(100);

            Thread.Sleep(6000);

            t2.Abort();

            Thread.Sleep(6000);

            t1.Abort();

            Console.ReadKey();
        }
    }
}

//A100A100A100A100A100A100AAAAAAA
