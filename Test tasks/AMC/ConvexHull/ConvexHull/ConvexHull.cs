using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace ConvexHull
{
    static class ConvexHull
    {
        static bool inDev = false;

        static void Main(string[] args)
        {
            string input = Console.ReadLine();

            char[] splitChars = { ' ' };

            string[] raw = input.Split(splitChars, options: StringSplitOptions.RemoveEmptyEntries);

            int count = Int32.Parse(raw[0]) * 2; //Point contains 2 numbers

            raw = raw.Skip(1).ToArray();

            raw = raw.Select(r => r.Trim(' ')).ToArray();

            HashSet<PointDc> uniquePoints = new HashSet<PointDc>(); // Cutoff dublicates

            for (int i = 0; i < count - 1; i += 2)
            {
                decimal x = decimal.Parse(raw[i], CultureInfo.InvariantCulture);
                decimal y = decimal.Parse(raw[i + 1], CultureInfo.InvariantCulture);

                uniquePoints.Add(new PointDc(x, y));
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Back to the fu.... Jarvis algo

            List<PointDc> points = new List<PointDc>();
            List<PointDc> result = new List<PointDc>();

            points.AddRange(uniquePoints);

            if (points.Count < 3)
            {
                Console.Write(0);

                return;
            }

            // Velosiped
            for (int i = 1; i < points.Count; i++) //skip first
            {
                if (points[i].X < points[0].X)
                {
                    PointDc tempP = points[0];
                    points[0] = points[i];
                    points[i] = tempP;
                }
            }

            result.Add(points[0]);

            PointDc temp = points[0];
            points[0] = points[points.Count - 1];
            points[points.Count - 1] = temp;

            while (true)
            {
                int rightThenIndex = 0;

                for (int i = 0; i < points.Count; i++)
                {
                    if (result[result.Count - 1].Orientation(points[rightThenIndex], points[i]) < 0.0m)
                    {
                        rightThenIndex = i;
                    }
                }

                if (points[rightThenIndex] == result[0])
                {
                    break;
                }
                else
                {
                    result.Add(points[rightThenIndex]);
                    points.RemoveAt(rightThenIndex);
                }
            }

            Console.Write(result.Count);
            foreach (PointDc r in result)
            {
                Console.Write(" " + r.X.ToString("0.00000000") + " " + r.Y.ToString("0.00000000"));
            }


            //Just for debug
            if (inDev)
            {
                Console.WriteLine("------- DEBUG INFO -------");

                Console.WriteLine("points:" + points.Count);
                foreach (PointDc point in points)
                {
                    Console.WriteLine(point.X + " " + point.Y);
                }

                Console.WriteLine("result:" + result.Count);
                foreach (PointDc point in result)
                {
                    Console.WriteLine(point.X + " " + point.Y);
                }

                Console.ReadKey();
            }
        }
    }

    public class PointDc
    {
        public decimal X;
        public decimal Y;

        public PointDc(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }
    }

    public static class Extensions
    {
        public static decimal Orientation(this PointDc a, PointDc b, PointDc c)
        {
            //float delta = (a.X - o.X) * (a.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);

            //if (delta > 0f) return 1; // Clock wise
            //if (delta < 0f) return -1; // Counterclock wise

            return ((b.X - a.X) * (c.Y - b.Y) - (b.Y - a.Y) * (c.X - b.X));
        }
    }
}
