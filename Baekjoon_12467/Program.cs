using System;
using System.Linq;

namespace Baekjoon_12467
{
    class Program
    {
        static void Main(string[] args)
        {
            int testcases = Convert.ToInt32(Console.ReadLine());

            for (int t = 1; t <= testcases; t++)
            {
                var inputs = Console.ReadLine().Split(' ').Select(t => Convert.ToInt32(t)).ToArray();
                int Rows = inputs[0];
                int Cols = inputs[1];
                int M = inputs[2];

                var matrix = Enumerable.Range(0, Rows).Select(i => Console.ReadLine().Split(' ').Select(t => Convert.ToInt32(t)).ToArray()).ToArray();
                int day = findDays(matrix, Rows, Cols, M);

                Console.WriteLine($"Case #{t} : {day}");

            }
        }

        private static int findDays(int[][] matrix, int rows, int cols, int m)
        {
            int day = 0;
            while (!isEmpty(matrix, rows, cols))
            {
                day++;

                Console.WriteLine($"Days {day}");

                Console.WriteLine($"Initial State");
                print(matrix, rows, cols);

                // accumulate water
                var clone = matrix.Select(r => r.Select(c => 100).ToArray()).ToArray();
                for (int distance = 0; distance < Math.Max(rows, cols) / 2 + 1; distance++)
                {
                    for (int r = distance; r < rows - distance; r++)
                    {
                        for (int c = distance; c < cols - distance; c++)
                        {
                            if (r == distance || r == rows - distance - 1 || c == distance || c == cols - distance - 1)
                            {
                                if (distance == 0)
                                {
                                    clone[r][c] = matrix[r][c];
                                }
                            }
                        }
                    }
                }
                matrix = clone;

                Console.WriteLine($"After Accumulating Water State");
                print(matrix, rows, cols);

                // erode

                Console.WriteLine($"After Erosion State");
                print(matrix, rows, cols);

            }
            return day;
        }

        private static void print(int[][] matrix, int rows, int cols)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Console.Write(matrix[r][c]);
                }
                Console.WriteLine();
            }
        }

        private static bool isEmpty(int[][] matrix, int rows, int cols) => matrix.SelectMany(x => x).Where(x => x != 0).Count() <= 0;
    }
}
