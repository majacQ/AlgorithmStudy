using System;
using System.Linq;

namespace Baekjoon_2875_contest_or_intern
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = Console.ReadLine();

            var input = line.Split(' ').Select(token => Convert.ToInt32(token)).ToList();

            int maximumTeam = Math.Min(input[0] / 2, input[1]);

            int surplus = (input[0] + input[1]) - maximumTeam * 3;
            int remains = input[2] - surplus;

            maximumTeam = maximumTeam - ((remains > 0) ? (remains + 2) / 3 : 0);

            Console.WriteLine(maximumTeam);
        }
    }
}
