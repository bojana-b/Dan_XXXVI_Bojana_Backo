using System;
using System.Threading;

namespace Dan_XXXVI_Bojana_Backo
{
    class Program
    {
        public static string fileOddNumbers = @"..\..\FileByThread.txt";

        static void Main(string[] args)
        {
            Matrix matrix = new Matrix();
            Thread t1 = new Thread(() => matrix.EmptyToFullMatrix());
            t1.Name = "First Thread";
            t1.Start();
            Thread t2 = new Thread(() => matrix.GenerateNumbers());
            t2.Name = "Second Thread";
            t2.Start();
            t1.Join();
            t2.Join();

            Thread t3 = new Thread(() => matrix.WriteOddNumbersToFile());
            t3.Name = "Third Thread";
            Thread t4 = new Thread(() => matrix.ReadOddNumbersFromFile());
            t4.Name = "Fourth Thread";
            t3.Start();
            t3.Join();
            t4.Start();
            t4.Join();
            Console.ReadLine();
        }

        
    }
}
