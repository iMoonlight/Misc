using System;
using System.Collections.Generic;
using System.Linq;

namespace SortArray
{
    static class SortArray
    {
        static bool inDev = false;

        static void Main(string[] args)
        {
            string input = Console.ReadLine();

            int[] raw = input.Split(' ').Select(s => int.Parse(s)).ToArray();
            int quanity = raw[0];

            int[] temp = raw.Skip(1).ToArray();
            List<int> sorted = new List<int>();

            sorted.AddRange(temp.Take(quanity));
            sorted.Sort();

            Console.Write(quanity + " ");
            
            for(int i=0; i<quanity; i++)
            {
                Console.Write(sorted[i]);

                if (i < quanity - 1) Console.Write(" ");
            }

            //Just for debug
            if (inDev)
            {
                Console.WriteLine("------- DEBUG INFO -------");

                foreach (int s in sorted)
                {
                    Console.Write(s + " ");
                }
				
                Console.WriteLine();

                Console.ReadKey();
            }
        }
    }
}
