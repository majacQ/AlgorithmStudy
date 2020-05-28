using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baekjoon_14890_GyungSaRo
{
    class Program
    {
        static int N, L;
        static int[][] map;
        static int[][] diffmap_v;
        static int[][] diffmap_h;

        static void Main(string[] args)
        {
            var tokens = Console.ReadLine().Split(' ');
            N = Convert.ToInt32(tokens[0]);
            L = Convert.ToInt32(tokens[1]);

            map = new int[N][];

            for (int row = 0; row < N; row++)
                map[row] = Console.ReadLine().Split(' ').Select(t => Convert.ToInt32(t)).ToArray();

            diffmap_v = new int[N][];
            for (int i = 0; i < N; i++)
                diffmap_v[i] = new int[N];
            diffmap_h = new int[N][];
            for (int i = 0; i < N; i++)
                diffmap_h[i] = new int[N];

            int r = calculate();

            Console.WriteLine(r);

        }

        private static int calculate()
        {
            for (int row = 0; row < N; row++)
            {
                for (int column = 0; column < N; column++)
                {
                    if (0 <= column - 1 && column < N)
                        diffmap_h[row][column] = map[row][column] - map[row][column - 1];
                    if (0 <= row - 1 && row < N)
                        diffmap_v[column][row] = map[row][column] - map[row - 1][column];
                }
            }

            //
            int hN = diffmap_h.Where(checkRow).Count();
            int vN = diffmap_v.Where(checkRow).Count();

            return vN + hN;
        }

        static bool checkRow(int[] row)
        {
            row = row.ToArray();
            if (row.Count(c => Math.Abs(c) >= 2) > 0)
                return false;

            int lastcheck = -1;
            for (int i = 0; i < N; i++)
            {
                if (row[i] == 1)
                {
                    // i-1, ... i-L must 0
                    if (!(i - L >= 0))
                        return false;
                    for (int t = i - 1; t >= i - L; t--)
                    {
                        if (row[t] != 0)
                            return false;
                    }
                }
                if (row[i] == -1)
                {
                    // i, ... i+L-1 must 0
                    if (!(i + L - 1 < N))
                        return false;
                    for (int t = i; t <= i + L - 1; t++)
                    {
                        if (row[t] != 0)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
