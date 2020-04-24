using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Baekjoon_17613
{
    class Program
    {
        public static Dictionary<string, int> lookuptable = new Dictionary<string, int>();

        public static void Main(string[] args)
        {
            int nTest = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < nTest; i++)
            {
                var inputs = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToList();

                //int step = 0;

                //while(inputs[0]!=0 && inputs[1] != 0)
                //{
                //    var tmax = getExpo(inputs[1]);

                //    step += tmax;

                //    inputs[0] = Math.Max(inputs[0] - (int)Math.Pow(2, tmax), 0);
                //    inputs[1] = Math.Max(inputs[1] - (int)Math.Pow(2, tmax), inputs[0]);
                //}
                //Console.WriteLine(step);


                int min = getMaxMinStep(inputs[0], inputs[1]);

                Console.WriteLine(min);
                //inputs[0] = Math.Max(inputs[0], (int)Math.Pow(2, getExpo(inputs[1])));
                //inputs[1] = Math.Min(inputs[1], (int)Math.Pow(2, getExpo(inputs[0]) + 1));
                //int min = getJump(inputs[0]);
                //for (int j = inputs[0] + 1; j <= inputs[1]; j++)
                //{
                //    min = Math.Max(min, getJump(j));
                //}

                //Console.WriteLine(min);
            }
        }

        public static int getMaxMinStep(int range_min, int range_max)
        {
            int step = 0;
            int exp = getExpo(range_min + 1);

            if (lookuptable.ContainsKey($"{range_min},{range_max}"))
                return lookuptable[$"{range_min},{range_max}"];

            if (range_max < range_min)
                return step;
            if (range_min == 0 && range_max == 0 )
                return step;

            //Console.WriteLine($"Check range [{range_min},{range_max}]");
            for (; exp <= getExpo(range_max + 1); exp++)
            {
                int current_min = (int)Math.Pow(2, exp) - 1;   // inclusive
                int current_max = (int)Math.Pow(2, exp + 1) - 2;   // exclusive

                //Console.WriteLine($"when exp={exp} [{current_min},{current_max}]");

                step = Math.Max(step,
                    exp + getMaxMinStep(
                        Math.Max(0, range_min - current_min),
                        Math.Min(current_max - current_min, range_max - current_min)
                        ));
            }
            lookuptable[$"{range_min},{range_max}"] = step;
            return step;
        }

        public static int getJump(int target)
        {
            //int t = target;
            int step = 0;
            while (target > 0)
            {
                step += getExpo(target + 1);
                target -= (int)Math.Pow(2, getExpo(target + 1)) - 1;
            }
            // Console.WriteLine($"{t} -> {step}");
            return step;
        }

        public static int getExpo(int v)
        {
            int a = 0;
            while(v > 1)
            {
                a++;
                v = v >> 1;
            }
            return a;
        }
    }
}