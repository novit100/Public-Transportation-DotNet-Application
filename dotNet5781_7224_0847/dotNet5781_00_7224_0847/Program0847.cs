using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_0847_7224
{class c { }
    struct s { }
    partial class Program
    {
        static void Main(string[] args)
        { c co;
            //Console.WriteLine(co);
            co = null;
            s so;
            Console.WriteLine("----------------");
            Console.WriteLine(so);
            //so = null;

            Welcome7224();
            Welcome0847();
           
            Console.ReadKey();
        }

        private static void Welcome0847()
        {
            CreateFile("file1.txt");
            CreateFile("file1.txt", "c:\\");
            CreateFile("file1.txt", "\\", 512);
            CreateFile("file1.txt", size: 128);  //מעניין!

            int x;
            int c;
            bool b = int.TryParse("abc", out x);
            bool b1 = int.TryParse("258", out c);
            int y = int.Parse("1234");
            Console.WriteLine(x);// prints 0
            Console.WriteLine(y);//prints 1234
            Console.WriteLine(c);//prints 258
            bool b3 = int.TryParse("131313", out x);
            Console.WriteLine(x);//prints 131313
            Console.WriteLine("Enter your name: ");
            String n = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", n);
            int myCharUnicode = System.Console.Read();//returns the asciiz vlue of the first character
            Console.WriteLine(myCharUnicode);
            string author = "JKR";
            string book = "HP";
            int price = 20;
      
            Console.WriteLine($"{author}{book,20}");
            Console.WriteLine($"book {book} price is {(price<50?"50":"100")} $");
            Console.ReadKey();

        }
        static partial void Welcome7224();
        public static void CreateFile
    (string name, string path = "\\", int size = 1024)
        {
            // do something  
            Console.WriteLine("name:{0,-5} | path:{1,-3} | size:{2,-5}", name, path, size);
        }

    }
}
