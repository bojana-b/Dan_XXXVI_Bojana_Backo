using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Dan_XXXVI_Bojana_Backo
{
    public class Matrix
    {
        readonly object listLock = new object();
        static List<int> list = new List<int>();
        Random random = new Random();

        // Function for creating the matrix of range 100x100
        public void EmptyToFullMatrix()
        {
            int[,] matrix = new int[100, 100];
            // Lock the critical section of code
            lock (listLock)
            {
                while (list.Count != 10000)
                {
                    // Wait while list count is different from max 10000
                    Monitor.Wait(listLock);
                }
                Console.WriteLine(Thread.CurrentThread.Name + " is filling the matrix.");
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] = list[i + j];
                    }
                }
            }
        }
        // Function for genetaring random numbers
        public void GenerateNumbers()
        {
            // Lock the critical section of code
            lock (listLock)
            {
                for (int i = 0; i < 10000; i++)
                {
                    int number = random.Next(10, 100);
                    list.Add(number);
                }
                // When the 10000th random number is generated Puls to
                // the EmptyToFullMatrix() function to fill the matrix
                Monitor.Pulse(listLock);
                Console.WriteLine(Thread.CurrentThread.Name + " generated numbers.");
            }
        }

        public void WriteOddNumbersToFile()
        {
            List<int> oddNumbers = new List<int>();
            foreach (var item in list)
            {
                if (item % 2 != 0)
                {
                    oddNumbers.Add(item);
                }
            }
            try
            {
                File.Delete(Program.fileOddNumbers);
                using (StreamWriter sw = File.CreateText(Program.fileOddNumbers))
                {
                    foreach (var item in oddNumbers)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Close();
                    Console.WriteLine(Thread.CurrentThread.Name + " has finished writing to the file!");
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
        }

        public void ReadOddNumbersFromFile()
        {
            try
            {
                using (StreamReader sr = File.OpenText(Program.fileOddNumbers))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.Write(line);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
        }
    }
}
