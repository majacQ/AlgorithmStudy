using System;
using System.Collections.Generic;
using System.Linq;

namespace Baekjoon_12100
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = Convert.ToInt32(Console.ReadLine());

            var matrix = Enumerable.Range(0, N).Select(n => Console.ReadLine().Split(' ').Select(t => Convert.ToInt32(t)).ToArray()).ToArray();

            Node root = new Node() { level = 0, matrix = matrix };
            int max = root.findMax();

            Console.WriteLine(max);
        }

        public class Node
        {
            public int level;
            public int[][] matrix;
            public List<Node> childs = new List<Node>();

            public int findMax()
            {
                if (level >= 5)
                    return matrix.Max(row => row.Max());

                // get childs
                childs.Add(new Node() { level = level + 1, matrix = moveMatrixLeft(matrix.Select(row => row.ToArray()).ToArray()) });   // left
                childs.Add(new Node() { level = level + 1, matrix = rotateMatrix(rotateMatrix(rotateMatrix(moveMatrixLeft(rotateMatrix(matrix))))) });
                childs.Add(new Node() { level = level + 1, matrix = rotateMatrix(rotateMatrix(moveMatrixLeft(rotateMatrix(rotateMatrix(matrix))))) });
                childs.Add(new Node() { level = level + 1, matrix = rotateMatrix(moveMatrixLeft(rotateMatrix(rotateMatrix(rotateMatrix(matrix))))) });

                //
                return childs.Max(c => c.findMax());
            }

            public static int[][] rotateMatrix(int[][] _matrix)
            {
                int n = _matrix.Length;
                var clone = _matrix.Select(row => row.ToArray()).ToArray();

                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        clone[i][j] = _matrix[n - j - 1][i];
                    }
                }
                return clone;
            }
            public static int[][] moveMatrixLeft(int[][] _matrix)
            {
                return _matrix.Select(row =>
                {
                    var numbers = row.Where(x => x != 0).ToList();

                    List<int> nextRow = new List<int>();

                    bool canMerge = true;
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        if (nextRow.Count <= 0)
                            nextRow.Add(numbers[i]);
                        else if (canMerge && nextRow.Last() == numbers[i])
                        {
                            nextRow[nextRow.Count - 1] *= 2;
                            canMerge = false;
                        }
                        else
                        {
                            nextRow.Add(numbers[i]);
                            canMerge = true;
                        }
                    }

                    while (row.Length > nextRow.Count)
                        nextRow.Add(0);

                    return nextRow.ToArray();
                }).ToArray();
            }

        }
    }
}
