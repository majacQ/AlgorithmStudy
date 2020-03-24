using System;
using System.Collections.Generic;
using System.Linq;

namespace Baekjoon_9095_sum_1_2_3
{
    class Program
    {
        static List<int> table = new List<int>() { 0, 1, 2, 4 };

        static int get(int n)
        {
            while (table.Count < n + 1)
                table.Add(table[table.Count - 1] + table[table.Count - 2] + table[table.Count - 3]);

            return table[n];
        }

        static void Main(string[] args)
        {
            int nTestcase = Convert.ToInt32(Console.ReadLine());

            for(int i=0; i<nTestcase; i++)
            {
                int n = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(get(n));
            }

        }
    }
}
