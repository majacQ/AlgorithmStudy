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
            var points = Enumerable.Range(0, N).Select(i=>
            {
                var nums = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToList();
                return new Tuple<int, int>(nums[0], nums[1]);
            }).ToList();

            Tuple<int, int> base_bottom_left = new Tuple<int, int>(-1000000, -1000000);
            var toUpRight = points.OrderBy(point => abs(point, base_bottom_left)).ToList();
            int toUpRightMax = abs(toUpRight.FirstOrDefault(), toUpRight.LastOrDefault());

            Tuple<int, int> base_top_left = new Tuple<int, int>(-1000000, 1000000);
            var toBottomRight = points.OrderBy(point => abs(point, base_top_left)).ToList();
            int tpBottomRightMax = abs(toBottomRight.FirstOrDefault(), toBottomRight.LastOrDefault());

            Console.WriteLine(Math.Max(toUpRightMax, tpBottomRightMax));
        }
        static int abs(Tuple<int, int> a, Tuple<int, int> b) => Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
    }
}
