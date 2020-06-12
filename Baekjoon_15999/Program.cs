using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baekjoon_15999
{
    class Program
    {
        static int row, col;
        static char[][] matrix;
        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split(' ').Select(t => Convert.ToInt32(t)).ToList();
            row = input[0];
            col = input[1];

            matrix = new char[row][];
            for (int i = 0; i < row; i++)
                matrix[i] = Console.ReadLine().ToCharArray();

            int not = 0;
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    bool border = false;
                    if (!border && 0 <= r - 1 && matrix[r][c] != matrix[r - 1][c])
                        border = true;
                    if (!border && c + 1 < col && matrix[r][c] != matrix[r][c + 1])
                        border = true;
                    if (!border && r + 1 < row && matrix[r][c] != matrix[r + 1][c])
                        border = true;
                    if (!border && 0 <= c - 1 && matrix[r][c] != matrix[r][c - 1])
                        border = true;

                    if (border)
                        not++;
                }
            }

            int v = 1;
            int total = row * col;
            int valid = total - not;
            for(int i=0; i<valid; i++)
            {
                v = (v * 2) % 1000000007;
            }

            Console.WriteLine(v);
        }
    }
}
