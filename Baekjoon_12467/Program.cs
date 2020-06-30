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

        public Matrix(int _rows, int _cols, Func<int, int, Type> initializer)
        {
            // make matrix
            matrix = Enumerable.Range(0, _rows).Select(r => new Type[_cols]).ToArray();
            // initialize elements
            Enumerable.Range(0, _rows).ToList().ForEach(r => Enumerable.Range(0, _cols).ToList().ForEach(c => matrix[r][c] = initializer(r, c)));
        }

        public void print()
        {
            foreach (var r in matrix)
            {
                Console.WriteLine(string.Join(' ', r.Select(c => c.ToString())));
            }
        }

        public List<Tuple<int, int>> searchDFS(int r, int c, Func<int, int, bool> predicate, Func<int, int, bool> endCondition = null)
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

        public bool isEdge(int r, int c) => r == 0 || r == Rows - 1 || c == 0 || c == Cols - 1;
    }

    class Node
    {
        public int height;
        public int waterlevel;
        public bool isInsideIsland;
        public bool visit;
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

                Matrix<Node> matrix = new Matrix<Node>(Rows, Cols, (r, c) => new Node() { height = inputMatrix[r][c] });

                int day = findDays(matrix, M);

                Console.WriteLine($"Case #{t}: {day}");
            }
        }

        private static int findDays(Matrix<Node> matrix, int m)
        {
            int day = 0;
            while (!matrix.matrix.All(r => r.All(v => v.height == 0)))
            {
                day++;

                // accumulate water
                matrix.ForEach((r, c) =>
                {
                    // isInsideIsland
                    if (matrix[r][c].height != 0)
                        matrix[r][c].isInsideIsland = true;
                    else
                    {
                        var connectedSealevelNodes = matrix.searchDFS(r, c,
                            (_r, _c) => (matrix[_r][_c].height == 0),           // find all connected sea level nodes
                            (_r, _c) => matrix.isEdge(_r, _c));                  // exit if reach to edge of map

                        var lastNode = connectedSealevelNodes.Last();

                        // if last connected sea level node is edge, (r,c) is inside island
                        matrix[r][c].isInsideIsland = !matrix.isEdge(lastNode.Item1, lastNode.Item2) ? true : false;
                    }
                    // visit
                    matrix[r][c].visit = false;
                });
                List<Tuple<int, int>> boundaries = new List<Tuple<int, int>>();
                matrix.ForEach((r, c) =>
                {
                    // check if (r,c) is boundary node of island
                    if ((matrix.isEdge(r, c) && matrix[r][c].isInsideIsland) || matrix.findNeighbor(r, c, (_r, _c) => matrix[_r][_c].isInsideIsland == false).Count > 0)
                        boundaries.Add(new Tuple<int, int>(r, c));
                });
                while (boundaries.Count > 0)  // until no more boundaries left
                {
                    // lowest boundary first
                    boundaries = boundaries.OrderBy(cell => matrix[cell.Item1][cell.Item2].height).ToList();
                    var current = boundaries.First();
                    boundaries.RemoveAt(0);

                    //
                    int r = current.Item1;
                    int c = current.Item2;

                    // find all nodes that is 1.connected, 2.inside island, 3.lower than current boundary
                    var sinkedNodes = matrix.searchDFS(r, c, (_r, _c) => matrix[_r][_c].isInsideIsland && !matrix[_r][_c].visit && matrix[_r][_c].height <= matrix[r][c].height);
                    sinkedNodes.ForEach(n =>
                    {
                        matrix[n.Item1][n.Item2].waterlevel = matrix[r][c].height;
                        matrix[n.Item1][n.Item2].visit = true;
                    });

                    // find new boundary from sinked nodes
                    var newBoundaries = sinkedNodes.SelectMany(n =>
                        // not yet visited, and not in boundary list
                        matrix.findNeighbor(n.Item1, n.Item2, (_r, _c) => !matrix[_r][_c].visit && boundaries.FindIndex(_n => _n.Item1 == n.Item1 && _n.Item2 == n.Item2) == -1)
                    );
                    boundaries.AddRange(newBoundaries);
                }

                // erode
                matrix.ForEach((r, c) =>
                {
                    // find min lower waterlevel around me
                    int minWaterLevel = 100;
                    var lowerWaterLevels = matrix.findNeighbor(r, c, (_r, _c) => matrix[_r][_c].waterlevel < matrix[r][c].waterlevel).Select(n => matrix[n.Item1][n.Item2].waterlevel).ToList();
                    if (matrix.isEdge(r, c))
                        lowerWaterLevels.Add(0);
                    minWaterLevel = (lowerWaterLevels.Count == 0) ? matrix[r][c].waterlevel : lowerWaterLevels.Min();

                    // calculate real erode
                    int erode = Math.Min(matrix[r][c].waterlevel - minWaterLevel, m);
                    matrix[r][c].height = Math.Max(matrix[r][c].height - erode, 0);
                });
            }
            return day;
        }

    }
}
