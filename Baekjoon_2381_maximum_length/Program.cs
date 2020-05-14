using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baekjoon_2381_maximum_length
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = Convert.ToInt32(Console.ReadLine());

            Tuple<int, int> min = null;
            Tuple<int, int> max = null;

            Tuple<int, int> current;
            for (int i=0;i<N;i++)
            {
                var input = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToList();

                current = new Tuple<int, int>(input[0] + 1000000, input[1] + 1000000);

                min = (min == null) ? current : new Tuple<int, int>(Math.Max(min.Item1, current.Item1), Math.Min(min.Item2, current.Item2));
                max = (max == null) ? current : new Tuple<int, int>(Math.Min(max.Item1, current.Item1), Math.Max(max.Item2, current.Item2));
            }
            Console.WriteLine(min);
            Console.WriteLine(max);

        }
    }
}
