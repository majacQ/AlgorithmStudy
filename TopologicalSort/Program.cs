using System;
using System.Collections.Generic;
using System.Linq;

namespace TopologicalSort
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> vertices = new List<int>() { 0, 1, 2, 3, 4, 5 };
            List<Tuple<int, int>> edges = new List<Tuple<int, int>>() {
                new Tuple<int,int>(0,2), new Tuple<int,int>(0,3),
                new Tuple<int,int>(1,3), new Tuple<int,int>(1,4),
                new Tuple<int,int>(2,3), new Tuple<int,int>(2,5),
                new Tuple<int,int>(3,5),
                new Tuple<int,int>(4,5)
            };

            List<int> L = new List<int>(); // Empty list that will contain the result
            List<int> S = new List<int>(); // Set of all vertices with no incoming edge

            // find all vertices with no incoming edge
            S = vertices.Where(vertex => edges.Where(edge => edge.Item2 == vertex).Count() == 0).ToList();
            while (S.Count > 0)
            {
                //
                var currentVertex = S[0]; // S[new Random().Next(S.Count)];
                S.Remove(currentVertex);
                L.Add(currentVertex);

                // remove current vertex and its outgoing edges
                vertices.Remove(currentVertex);
                edges.RemoveAll(edge => edge.Item1 == currentVertex);

                // find all vertices with no incoming edge again
                S = vertices.Where(vertex => edges.Where(edge => edge.Item2 == vertex).Count() == 0).ToList();
            }

            if (edges.Count > 0)
                Console.WriteLine("Cycle!");
            else
                for (int i = 0; i < L.Count; i++)
                    Console.WriteLine(L[i]);

        }

    }
}
