using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_0847_7224
{   class C {/* public int ca=8; public readonly int x = 8;*/ }
    struct S
    {
     public int cb;
       
    }
    partial class Program
    {
        static void Main(string[] args)
    {
            //List<int> list = new List<int>() { 1, 2, 3, 4 };
            //var result1 = list.Where()
        C co;
  //      Console.WriteLine(co);
             S so=new S();
             Console.WriteLine(so.cb);


            co = null;
           
            Console.WriteLine("----------------");
            Console.WriteLine(so);
      //  so = null;
            ////////////////////////////////



            Console.WriteLine($"please enter the number of amudot you want in the matrix");
            int[][] intArray = new int[4][];
            string y = Console.ReadLine();
           int x;
            for (int i = 0; i < intArray.Length; i++)
            {
              bool b=int.TryParse(y,out x);
                Console.WriteLine(x);
                intArray[i] = new int[x];
                for (int j = 0; j < intArray[i].Length; j++) intArray[i][j] = (i + 1) * (j + 1);
            }
            

for (int i = 0; i < intArray.Length; i++)
            {
                for (int j = 0; j < intArray[i].Length; j++) Console.Write("{0,-3}", intArray[i][j]);
                Console.WriteLine();
            }
            //////////////////////
            int[][] intosArray = new int[4][]
              {
    new int[4]{ 1, 2, 3, 4 },
    new int[4]{ 5, 6, 7, 8 },
    new int[4]{ 9, 10, 11, 12 },
    new int[4]{ 13, 14, 15, 16 }
            };
            Console.WriteLine("intos");
Console.WriteLine(intosArray[0][3]); Console.WriteLine(intosArray[3][0]);
           



            ///////////////////////////////



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
