using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baekjoon_3055
{
    class Program
    {
        static int row;
        static int col;
        static int goal_col = -1;
        static int goal_row = -1;

        static char[][] map;

        static void Main(string[] args)
        {
            // input
            var inputs = Console.ReadLine().Split(' ').Select(t => Convert.ToInt32(t)).ToList();
            row = inputs[0];
            col = inputs[1];
            map = new char[row][];
            Enumerable.Range(0, row).ToList().ForEach(r => map[r] = Console.ReadLine().ToCharArray());

            int S_col = -1;
            int S_row = -1;
            S_row = Array.FindIndex(map, row => row.Contains('S'));
            S_col = Array.FindIndex(map[S_row], c => c == 'S');
            goal_row = Array.FindIndex(map, row => row.Contains('D'));
            goal_col = Array.FindIndex(map[goal_row], c => c == 'D');

            int step = 0;
            while (true)
            {
                int done = Array.FindIndex(map, row => row.Contains('D'));
                if (done == -1)
                {
                    break;
                }

                step++;
                // water expand map
                for (int r = 0; r < map.Length; r++)
                {
                    for (int c = 0; c < map[r].Length; c++)
                    {
                        if (map[r][c] == '.' && (
                            (r - 1 >= 0 && map[r - 1][c] == '*') ||
                            (r + 1 < map.Length && map[r + 1][c] == '*') ||
                            (c - 1 >= 0 && map[r][c - 1] == '*') ||
                            (c + 1 < map[r].Length && map[r][c + 1] == '*')
                            ))
                        {
                            map[r][c] = '$';
                        }
                    }
                }
                for (int r = 0; r < map.Length; r++)
                {
                    for (int c = 0; c < map[r].Length; c++)
                    {
                        if (map[r][c] == '$')
                        {
                            map[r][c] = '*';
                        }
                    }
                }
                // S expand map
                bool expanded = false;
                for (int r = 0; r < map.Length; r++)
                {
                    for (int c = 0; c < map[r].Length; c++)
                    {
                        if ((map[r][c] == '.' || map[r][c] == 'D') && (
                            (r - 1 >= 0 && map[r - 1][c] == 'S') ||
                            (r + 1 < map.Length && map[r + 1][c] == 'S') ||
                            (c - 1 >= 0 && map[r][c - 1] == 'S') ||
                            (c + 1 < map[r].Length && map[r][c + 1] == 'S')
                            ))
                        {
                            map[r][c] = '$';
                            expanded = true;
                        }
                    }
                }
                for (int r = 0; r < map.Length; r++)
                {
                    for (int c = 0; c < map[r].Length; c++)
                    {
                        if (map[r][c] == '$')
                        {
                            map[r][c] = 'S';
                        }
                    }
                }
                //
                if (!expanded)
                {
                    step = -1;
                    break;
                }
            }

            Console.WriteLine((step <= 0) ? "KAKTUS" : "" + step);
        }
        
    }
}
