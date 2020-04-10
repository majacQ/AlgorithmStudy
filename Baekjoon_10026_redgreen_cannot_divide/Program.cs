using System;
using System.Collections;
using System.Collections.Generic;

namespace Baekjoon_10026_redgreen_cannot_divide
{
    class Program
    {

        static char[,] matrix = new char[100, 100];

        static void Main(string[] args)
        {
            int N = Convert.ToInt32(Console.ReadLine());

            int normal=0, abnormal=0;

            for(int i=0; i<N; i++)
            {
                var line = Console.ReadLine();
                var charAry = line.Trim().ToCharArray();
                for (int j = 0; j < N; j++)
                    matrix[i, j] = charAry[j];
            }

            normal = iterateGraph(N, matrix);

            for(int i=0; i<N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (matrix[i, j] == 'R')
                        matrix[i, j] = 'G';
                }
            }
            abnormal = iterateGraph(N, matrix);

            Console.WriteLine($"{normal} {abnormal}");
        }

        private static int iterateGraph(int N, char[,] matrix)
        {
            int[,] visited = new int[100, 100];
            int count = 0;
            Stack<Tuple<int, int>> remains = new Stack<Tuple<int, int>>();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (visited[i, j] != 0)
                        continue;

                    count++;
                    remains.Push(new Tuple<int, int>(i, j));
                    while (remains.Count > 0)
                    {
                        var current = remains.Pop();

                        visited[current.Item1, current.Item2] = count;

                        if (current.Item1 > 0 && visited[current.Item1 - 1, current.Item2] == 0 && matrix[current.Item1, current.Item2] == matrix[current.Item1 - 1, current.Item2])
                        {
                            remains.Push(new Tuple<int, int>(current.Item1 - 1, current.Item2));
                        }
                        if (current.Item1 < N - 1 && visited[current.Item1 + 1, current.Item2] == 0 && matrix[current.Item1, current.Item2] == matrix[current.Item1 + 1, current.Item2])
                        {
                            remains.Push(new Tuple<int, int>(current.Item1 + 1, current.Item2));
                        }
                        if (current.Item2 > 0 && visited[current.Item1, current.Item2 - 1] == 0 && matrix[current.Item1, current.Item2] == matrix[current.Item1, current.Item2 - 1])
                        {
                            remains.Push(new Tuple<int, int>(current.Item1, current.Item2 - 1));
                        }
                        if (current.Item2 < N - 1 && visited[current.Item1, current.Item2 + 1] == 0 && matrix[current.Item1, current.Item2] == matrix[current.Item1, current.Item2 + 1])
                        {
                            remains.Push(new Tuple<int, int>(current.Item1, current.Item2 + 1));
                        }
                    }
                }
            }

            //for (int i = 0; i < N; i++)
            //{
            //    for (int j = 0; j < N; j++)
            //    {
            //        Console.Write(visited[i, j]);
            //    }
            //    Console.WriteLine("");
            //}
            return count;
        }
    }
}
