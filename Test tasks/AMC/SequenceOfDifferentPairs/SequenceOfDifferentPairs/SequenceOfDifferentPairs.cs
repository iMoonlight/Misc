using System;
using System.Collections.Generic;
using System.Linq;

namespace SequenceOfDifferentPairs
{
    static class SequenceOfDifferentPairs
    {
        //const bool inDev = true;

        static void Main(string[] args)
        {
            // 1 2 3 4 5 6 -1 1 2 -1
            string input = Console.ReadLine();

            List<int> raw = input.Split(' ').Select(s => int.Parse(s.Trim(' '))).ToList();

            int separatorIdnex = raw.IndexOf(-1);
            int terminationIdnex = raw.LastIndexOf(-1);

            int[] AB = raw.Take(separatorIdnex).ToArray();
            int[] C = raw.Skip(separatorIdnex + 1).Take(terminationIdnex - separatorIdnex - 1).ToArray();

            Dictionary<int, int> ABd = new Dictionary<int, int>();

            for(int i = 1; i < AB.Length; i += 2)
            {
                ABd.Add(AB[i -1], AB[i]);
            }

            int Bi = -1;
            
            for(int i = 0; i < C.Length; i++)
            {
                if (ABd.TryGetValue(C[i], out Bi))
                {
                    Console.Write(Bi + " ");
                    continue;
                }
                Console.Write(0 + " ");
            }

            Console.Write(-1);

            //// Just for debug
            //if(inDev)
            //{
            //    Console.ReadKey();
            //}
        }
    }
}
