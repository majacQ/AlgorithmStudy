using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baekjoon_2644
{
    class Program
    {
        static int n;

        static int[] hierarchy = new int[101];

        static int start;
        static int end;

        static void Main(string[] args)
        {
            n = Convert.ToInt32(Console.ReadLine());
            var inputs = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToList();

            start = inputs[0];
            end = inputs[1];

            int m = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < m; i++)
            {
                inputs = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToList();
                hierarchy[inputs[1]] = inputs[0];
            }

            List<int> parentsOfStart = new List<int>();
            List<int> parentsOfEnd = new List<int>();

            int current = start;
            parentsOfStart.Add(current);
            while (hierarchy[current] != 0)
            {
                parentsOfStart.Add(hierarchy[current]);
                current = hierarchy[current];
            }

            current = end;
            parentsOfEnd.Add(current);
            while (hierarchy[current] != 0)
            {
                parentsOfEnd.Add(hierarchy[current]);
                current = hierarchy[current];
            }

            //Console.WriteLine(string.Join(">", parentsOfStart.Select(x => x.ToString())));
            //Console.WriteLine(string.Join(">", parentsOfEnd.Select(x => x.ToString())));
            int c = -1;
            for (int i = 0; i < parentsOfStart.Count; i++)
            {
                int t = parentsOfEnd.IndexOf(parentsOfStart[i]);

                if (t != -1)
                {
                    c = i + t;
                    break;
                }
            }
            Console.WriteLine(c);


        }
    }
}
