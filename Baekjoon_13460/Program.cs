using System;
using System.Collections.Generic;
using System.Linq;

namespace Baekjoon_13460
{
    class Program
    {
        static int nRow;
        static int nCol;

        public static int[][] matrix;

        static void Main(string[] args)
        {
            // get input
            var inputs = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToList();
            nRow = inputs[0];
            nCol = inputs[1];

            matrix = new int[nRow][];
            for (int i = 0; i < nRow; i++)
            {
                matrix[i] = new int[nCol];
                var line = Console.ReadLine();
                for (int j = 0; j < nCol; j++)
                    matrix[i][j] = line[j];
            }

            // calculate
            Node start = new Node() { matrix = matrix };
            int step = Find(start);

            // answer
            Console.WriteLine(step);
        }

        private static int Find(Node start)
        {
            // else
            Queue<Node> stack = new Queue<Node>();
            stack.Enqueue(start);

            int r = -1;
            Node current = start;
            while (stack.Count > 0)
            {
                current = stack.Dequeue();

                //Console.WriteLine();
                //current.print();

                // exit condition;
                if (current.end())
                {
                    r = current.level;
                    break;
                }

                current.expand().ForEach(child => stack.Enqueue(child));
            }
            return r;
        }

        class Node
        {
            public int level;
            public int[][] matrix;
            public Node[] childs = new Node[(int)Direction.COUNT];
            public Node parent;

            internal bool end() => matrix.FirstOrDefault(ary => ary.Contains('R')) == null && matrix.FirstOrDefault(ary => ary.Contains('B')) != null;

            internal List<Node> expand()
            {
                List<Node> r = new List<Node>();
                for (int direction = (int)Direction.UP; direction < (int)Direction.COUNT; direction++)
                {
                    var matrixClone = matrix.Select(a => a.ToArray()).ToArray();

                    matrixClone = move(matrixClone, (Direction)direction);

                    if (!compare(matrix, matrixClone) && level + 1 <= 10)
                    {
                        var newnode = new Node() { level = level + 1, matrix = matrixClone, parent = this };
                        //Console.WriteLine($"Direction {(Direction)direction}");
                        //newnode.print();
                        r.Add(newnode);
                    }
                }

                return r;
            }

            private bool compare(int[][] matrix, int[][] matrixClone)
            {
                if (matrix.Length != matrixClone.Length)
                    return false;
                for (int i = 0; i < matrix.Length; i++)
                {
                    if (matrix[i].Length != matrixClone[i].Length)
                        return false;
                    if (string.Join(' ', matrix[i]) != string.Join(' ', matrixClone[i]))
                        return false;
                }
                return true;
            }

            static List<char> CannotMove = new List<char>() { '#', 'R', 'B' };

            private int[][] move(int[][] matrixClone, Direction direction)
            {
                if (direction == Direction.UP)
                    matrixClone = rotateMatrixLeft(matrixClone);
                else if (direction == Direction.RIGHT)
                    matrixClone = rotateMatrixLeft(rotateMatrixLeft(matrixClone));
                else if (direction == Direction.DOWN)
                    matrixClone = rotateMatrixRight(matrixClone);

                // move left
                matrixClone.Where(row => row.Contains('R') || row.Contains('B')).ToList().ForEach(row =>
                {
                    for (int i = 0; i < row.Length; i++)
                    {
                        if (row[i] == 'R' || row[i] == 'B')
                        {
                            int nextPos = i;
                            for (nextPos = i; nextPos >= 0; nextPos--)
                            {
                                if (nextPos == i)
                                    continue;
                                if (row[nextPos] == '.')
                                    continue;
                                else if (CannotMove.Contains((char)row[nextPos]))
                                {
                                    nextPos += 1;
                                    break;
                                }
                                else if (row[nextPos] == 'O')
                                {
                                    nextPos = -1;
                                    break;
                                }
                            }

                            if (nextPos != -1)
                                row[nextPos] = row[i];
                            if (nextPos != i)
                                row[i] = '.';
                        }
                    }

                });

                if (direction == Direction.UP)
                    matrixClone = rotateMatrixRight(matrixClone);
                else if (direction == Direction.RIGHT)
                    matrixClone = rotateMatrixRight(rotateMatrixRight(matrixClone));
                else if (direction == Direction.DOWN)
                    matrixClone = rotateMatrixLeft(matrixClone);
                return matrixClone;
            }
            public int[][] rotateMatrixRight(int[][] matrix)
            {
                /* W and H are already swapped */
                int w = matrix.Length;
                int h = matrix[0].Length;
                int[][] ret = new int[h][];
                for (int i = 0; i < h; ++i)
                {
                    ret[i] = new int[w];
                    for (int j = 0; j < w; ++j)
                    {
                        ret[i][j] = matrix[w - j - 1][i];
                    }
                }
                return ret;
            }
            public int[][] rotateMatrixLeft(int[][] matrix)
            {
                /* W and H are already swapped */
                int w = matrix.Length;
                int h = matrix[0].Length;
                int[][] ret = new int[h][];
                for (int i = 0; i < h; ++i)
                {
                    ret[i] = new int[w];
                    for (int j = 0; j < w; ++j)
                    {
                        ret[i][j] = matrix[j][h - i - 1];
                    }
                }
                return ret;
            }

            public void print()
            {
                Console.WriteLine($"LEVEL : {level}");
                Array.ForEach(matrix, row => Console.WriteLine(string.Join(' ', row.Select(v => (char)v))));
            }
        }

        enum Direction
        {
            UP,
            RIGHT,
            DOWN,
            LEFT,
            COUNT
        }


    }
}
