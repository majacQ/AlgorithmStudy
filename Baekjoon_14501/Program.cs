using System;
using System.Linq;

namespace Baekjoon_14501
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] prize = new int[20];
            int[] time = new int[20];
            int N = Convert.ToInt32(Console.ReadLine());

            for (int i = 1; i <= N; i++)
            {
                var input = Console.ReadLine().Split(' ').Select(t => Convert.ToInt32(t)).ToArray();
                time[i] = input[0];
                prize[i] = input[1];
            }

            // 
            int[][] max = new int[20][];
            for (int i = 1; i < max.Length; i++)
                max[i] = new int[20];

            for (int totalEnd = 1; totalEnd <= N; totalEnd++)
            {
                int localMax = 0;
                for (int start = 1; start <= totalEnd; start++)
                {
                    int end = start + time[start] - 1;
                    if (end > totalEnd)
                        continue;

                    localMax = Math.Max(localMax, max[1][start-1] + prize[start] + max[end+1][totalEnd]);
                }
                max[1][totalEnd] = localMax;
            }

            Console.WriteLine(max[1][N]);
                  


        }
    }
}
