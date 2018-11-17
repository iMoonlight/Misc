using System;
using System.Linq;
using System.Globalization;

namespace CloseToTheArrayMaximum
{
    static class CloseToTheArrayMaximum
    {
        static bool inDev = false;

        static void Main(string[] args)
        {
            string input = Console.ReadLine();

            decimal[] raw = input.Split(' ').Select(s => decimal.Parse(s, CultureInfo.InvariantCulture)).ToArray();

            decimal eps = raw[0];

            int size = 0;
            size = raw.Skip(1).ToList().IndexOf(-1m);

            raw = raw.Skip(1).Take(size).ToArray();

            decimal max = raw.Max();

            int count = 0;

            foreach (decimal r in raw)
            {
                if (Math.Abs(r - max) < eps) count++;
            }

            Console.Write(count);


            //Just for debug
            if (inDev)
            {
                Console.WriteLine("------- DEBUG INFO -------");

                foreach (decimal r in raw)
                {
                    Console.Write(r + " ");
                }
                Console.WriteLine();

                Console.WriteLine("SIZE:" + size);
                Console.WriteLine("EPS:" + eps);
                Console.WriteLine("MAX: " + max);

                Console.ReadKey();
            }
        }
    }
}
