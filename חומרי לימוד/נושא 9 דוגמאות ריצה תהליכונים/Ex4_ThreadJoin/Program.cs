using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex4_ThreadJoin
{
    class Program
    {
        static void Main()
        {
            var stopwatch = Stopwatch.StartNew();

            Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds);

            // Create an array of Thread references.
            Thread[] array = new Thread[4];

            for (int i = 0; i < array.Length; i++)
            {
                // Start the thread with a ThreadStart.
                array[i] = new Thread(new ThreadStart(Try));
                array[i].Start();
            }

            Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds);
            
            // Join all the threads.
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine("Waiting for thread " + i + " to finish...");
                array[i].Join();
                Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds);
            }
            Console.WriteLine("DONE: {0}", stopwatch.ElapsedMilliseconds);
        }

        static void Try()
        {
            // This method takes ten seconds to terminate.
            Thread.Sleep(10000);
        }
    }
}

/*
Time: 0
Time: 8
Waiting for thread 0 to finish...
Time: 10007
Waiting for thread 1 to finish...
Time: 10007
Waiting for thread 2 to finish...
Time: 10009
Waiting for thread 3 to finish...
Time: 10010
DONE: 10010
*/
