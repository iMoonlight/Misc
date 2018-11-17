using System;
using System.Collections.Generic;
using System.Linq;

namespace TollRoad
{
    static class TollRoad
    {
        static bool inDev = false;

        static void Main(string[] args)
        {
            string input = Console.ReadLine();

            string[] splitStrings = { "-1" };
            char[] splitChars = { ' ' };

            string[] raw = input.Split(splitStrings, options: StringSplitOptions.RemoveEmptyEntries);

            raw = raw[0].Split(splitChars, options: StringSplitOptions.RemoveEmptyEntries);

            List<int> fees = new List<int>();

            fees.Add(int.Parse(raw[0]));
            fees.Add(int.Parse(raw[1]));

            raw = raw.Skip(2).ToArray();// VehicleIDs array

            List<vehicle> vehicles = new List<vehicle>();

            foreach (string vehicle in raw)
            {
                int engineType = Int32.Parse(vehicle[0].ToString());
                int tireType = Int32.Parse(vehicle[1].ToString());
                int headlightType = Int32.Parse(vehicle[2].ToString());
                int pos = Int32.Parse(vehicle.Substring(3));

                vehicles.Add(new vehicle(fees, engineType, tireType, headlightType, pos));
            }

            vehicles = vehicles.OrderBy(v => v.pos).ToList();

            foreach(vehicle v in vehicles)
            {
                v.OutputFee();
                Console.Write(" ");
            }

            Console.Write("-1");

            //Just for debug
            if (inDev)
            {
                Console.WriteLine("------- DEBUG INFO -------");

                foreach(string r in raw)
                {
                    Console.Write(r + " ");
                }
                Console.WriteLine();

                Console.ReadKey();
            }
        }
    }

    class vehicle
    {
        List<int> fees;

        int engineType;
        int tireType;
        int headlightType;

        public readonly int pos;

        decimal fee;

        public vehicle(List<int> fees, int engineType, int tireType, int headlightType, int pos)
        {
            this.fees = fees;

            this.engineType = engineType;
            this.tireType = tireType;
            this.headlightType = headlightType;

            this.pos = pos;

            this.fee = CalcFee();
        }

        decimal CalcFee()
        {
            int feeBase;
            int feeMul = 0;

            if (engineType == 2) feeBase = fees[1]; else feeBase = fees[0];

            if (tireType == 2) feeMul += 2;

            switch (headlightType)
            {
                case 0:
                    feeMul += 2;
                    break;
                case 2:
                    feeMul -= 2;
                    break;
                default:
                    break;
            }

            if (feeMul == 0) feeMul = 1;

            if (feeMul < 0)
            {
                return feeBase / Math.Abs(feeMul);
            }
            else
            {
                return feeBase * feeMul;
            }
        }

        public void OutputFee()
        {
            Console.Write(fee.ToString("0"));
        }
    }
}
