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
            
            Node root = new Node() { map = map, level = 0, Ss = new List<Tuple<int, int>>() { new Tuple<int, int>(S_col, S_row) } };
            int level = root.find();

            Console.WriteLine((level <= 0) ? "KAKTUS" : "" + level);
        }

        enum DIrection
        {
            UP,
            RIGHT,
            DOWN,
            LEFT,
            COUNT
        }
        class Node
        {
            public int level;
            public char[][] map;
            public List<Tuple<int, int>> Ss = new List<Tuple<int, int>>();

            public int find()
            {
                int minlevel = -1;

                Node current = this;
                while (minlevel == -1)
                {
                    if (current.done())
                    {
                        minlevel = current.level;
                    }
                    else
                    {
                        current = current.expand();
                        if (current == null)
                            break;
                    }
                }

                return minlevel;
            }

            public Node expand()
            {
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

                //
                List<Tuple<int, int>> totalChilds = new List<Tuple<int, int>>();
                Ss.ForEach(s =>
                {
                    int x = s.Item1;
                    int y = s.Item2;
                    if (y - 1 >= 0 && map[y - 1][x] != '*' && map[y - 1][x] != 'X')
                        totalChilds.Add(new Tuple<int, int>(x, y - 1));
                    if (y + 1 < map.Length && map[y + 1][x] != '*' && map[y + 1][x] != 'X')
                        totalChilds.Add(new Tuple<int, int>(x, y + 1));
                    if (x - 1 >= 0 && map[y][x - 1] != '*' && map[y][x - 1] != 'X')
                        totalChilds.Add(new Tuple<int, int>(x - 1, y));
                    if (x + 1 < map[0].Length && map[y][x + 1] != '*' && map[y][x + 1] != 'X')
                        totalChilds.Add(new Tuple<int, int>(x + 1, y));
                });

                // this node cannot expand anymore
                if (totalChilds.Count == 0)
                    return null;
                return new Node() { map = map, level = level + 1, Ss = totalChilds };
            }

            public bool done()
            {
                return Ss.FindIndex(s => s.Item1 == goal_col && s.Item2 == goal_row) != -1;
            }
        }
    }
}
