using System;
using System.Collections.Generic;
using System.Linq;

// https://www.acmicpc.net/problem/12467

// [input]
//2
//3 6 5
//5 9 9 9 9 9
//0 8 9 0 2 5
//3 9 9 9 9 9
//3 6 3
//3 8 10 11 10 8
//7 5 2 12 8 8
//6 9 11 9 8 4
// [output]
//Case #1: 3
//Case #2: 5

namespace Baekjoon_12467
{
    public class Matrix<Type>
    {
        public Type[][] matrix;
        public int Rows => matrix?.Length ?? -1;
        public int Cols => matrix?.FirstOrDefault()?.Length ?? -1;

        public Type[] this[int index] => matrix[index];

        public Matrix(int _rows, int _cols, Func<int,int, Type> initializer)
        {
            // make matrix
            matrix = Enumerable.Range(0, _rows).Select(r => new Type[_cols]).ToArray();
            // initialize elements
            Enumerable.Range(0, _rows).ToList().ForEach(r => Enumerable.Range(0, _cols).ToList().ForEach(c => matrix[r][c] = initializer(r, c)));
        }

        public void print()
        {
            foreach(var r in matrix)
            {
                Console.WriteLine(string.Join(' ', r.Select(c => c.ToString())));
            }
        }

        public List<Tuple<int, int>> searchDFS(int r, int c, Func<int, int, bool> predicate, Func<int,int, bool> endCondition = null)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            Stack<Tuple<int, int>> q = new Stack<Tuple<int, int>>();
            q.Push(new Tuple<int, int>(r, c));
            while (q.Count > 0)
            {
                var current = q.Peek();
                q.Pop();
                result.Add(current);
                if (endCondition != null && endCondition(current.Item1, current.Item2))
                    break;

                findNeighbor(current.Item1, current.Item2, predicate)   // filter with predicate
                    .Where(neighbor => result.FindIndex(cell => cell.Item1 == neighbor.Item1 && cell.Item2 == neighbor.Item2) == -1)    // remove visited neighbor
                    .ToList().ForEach((cell) => q.Push(cell));
            }
            return result;
        }

        public List<Tuple<int, int>> searchBFS(int r, int c, Func<int, int, bool> predicate)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            Queue<Tuple<int, int>> q = new Queue<Tuple<int, int>>();
            q.Enqueue(new Tuple<int, int>(r, c));
            while (q.Count > 0)
            {
                var current = q.Dequeue();
                result.Add(current);

                findNeighbor(current.Item1, current.Item2, predicate)   // filter with predicate
                    .Where(neighbor => result.FindIndex(cell => cell.Item1 == neighbor.Item1 && cell.Item2 == neighbor.Item2) == -1)    // remove visited neighbor
                    .ToList().ForEach((cell) => q.Enqueue(cell));
            }
            return result;
        }

        public List<Tuple<int, int>> findNeighbor(int r, int c, Func<int, int, bool> predicate)
        {
            var next = new List<Tuple<int, int>>();
            if (0 <= r - 1 && predicate(r - 1, c))
                next.Add(new Tuple<int, int>(r - 1, c));
            if (r + 1 < Rows && predicate(r + 1, c))
                next.Add(new Tuple<int, int>(r + 1, c));
            if (0 <= c - 1 && predicate(r, c - 1))
                next.Add(new Tuple<int, int>(r, c - 1));
            if (c + 1 < Cols && predicate(r, c + 1))
                next.Add(new Tuple<int, int>(r, c + 1));
            return next;
        }

        public void ForEach(Action<int, int> action) => Enumerable.Range(0, Rows).ToList().ForEach(r => Enumerable.Range(0, Cols).ToList().ForEach(c => action(r, c)));

        internal Matrix<Type> clone() => new Matrix<Type>(Rows, Cols, (r, c) => matrix[r][c]);
    }

    class Program
    {
        static void Main(string[] args)
        {
            int testcases = Convert.ToInt32(Console.ReadLine());

            for (int t = 1; t <= testcases; t++)
            {
                var inputs = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToArray();
                int Rows = inputs[0];
                int Cols = inputs[1];
                int M = inputs[2];

                var inputMatrix = Enumerable.Range(0, Rows).Select(row => Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToArray()).ToArray();

                Matrix<int> matrix = new Matrix<int>(Rows, Cols, (r, c) => inputMatrix[r][c]);

                int day = findDays(matrix, M);

                Console.WriteLine($"Case #{t}: {day}");

            }
        }

        private static int findDays(Matrix<int> matrix, int m)
        {
            int day = 0;
            while (!isEmpty(matrix))
            {
                day++;

                //Console.WriteLine($"Days {day}");
                //Console.WriteLine($"Initial State");
                //matrix.print();

                // accumulate water
                var waterlevel = matrix.clone();
                var isInsideIsland = new Matrix<bool>(matrix.Rows, matrix.Cols, (r, c) => false);
                matrix.ForEach((r, c) =>
                {
                    if (matrix[r][c] != 0)
                        isInsideIsland[r][c] = true;
                    else
                    {
                        var connectedZero = matrix.searchDFS(r, c, (_r, _c) => (matrix[_r][_c] == 0), (_r, _c) => _r == 0 || _r == matrix.Rows - 1 || _c == 0 || _c == matrix.Cols - 1);
                        isInsideIsland[r][c] = connectedZero.FindIndex(cell => cell.Item1 == 0 || cell.Item1 == matrix.Rows - 1 || cell.Item2 == 0 || cell.Item2 == matrix.Cols - 1) == -1;
                    }
                });
                List<Tuple<int, int>> boundaries = new List<Tuple<int, int>>();
                var isBoundary = new Matrix<bool>(matrix.Rows, matrix.Cols, (r, c) => false);
                matrix.ForEach((r, c) =>
                {
                    if (isInsideIsland[r][c] == false)
                        return;

                    if (r == 0 || r == matrix.Rows - 1 || c == 0 || c == matrix.Cols - 1)
                        isBoundary[r][c] = true;
                    else if (matrix.findNeighbor(r, c, (_r, _c) => isInsideIsland[_r][_c] == false).Count > 0)
                        isBoundary[r][c] = true;

                    if (isBoundary[r][c])
                        boundaries.Add(new Tuple<int, int>(r, c));
                });
                var visited = new Matrix<bool>(matrix.Rows, matrix.Cols, (r, c) => false);
                boundaries.OrderBy(c => matrix[c.Item1][c.Item2]).ToList().ForEach(cell =>
                {
                    var flows = matrix.searchBFS(cell.Item1, cell.Item2, (_r, _c) => isInsideIsland[_r][_c] && !visited[_r][_c] && matrix[_r][_c] <= matrix[cell.Item1][cell.Item2]);
                    flows.ForEach(_cell => waterlevel[_cell.Item1][_cell.Item2] = matrix[cell.Item1][cell.Item2]);
                    flows.ForEach(_cell => visited[_cell.Item1][_cell.Item2] = true);
                });
                //Console.WriteLine($"After Accumulating Water State");
                //waterlevel.print();

                // erode
                var erosion = waterlevel.clone();
                erosion.ForEach((r, c) =>
                {
                    // find minNeighbor than me
                    int minNeighbor = 100;
                    var lowerNeighborsWaterlevel = erosion.findNeighbor(r, c, (_r, _c) => waterlevel[_r][_c] < waterlevel[r][c]).Select(cell => waterlevel[cell.Item1][cell.Item2]).ToList();
                    if (r == 0 || r == erosion.Rows - 1 || c == 0 || c == erosion.Cols - 1)
                        lowerNeighborsWaterlevel.Add(0);
                    minNeighbor = (lowerNeighborsWaterlevel.Count == 0) ? waterlevel[r][c] : lowerNeighborsWaterlevel.Min();

                    // calculate real erode
                    int erode = Math.Min(waterlevel[r][c] - minNeighbor, m);
                    erosion[r][c] = erode;
                });
                matrix.ForEach((r, c) => matrix[r][c] = Math.Max(matrix[r][c] - erosion[r][c], 0));
                //Console.WriteLine($"After Erosion State");
                //matrix.print();

            }
            return day;
        }

        private static bool isEmpty(Matrix<int> matrix) => matrix.matrix.All(r => r.All(v => v == 0));
    }
}
