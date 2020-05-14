using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baekjoon_14888
{
    class Program
    {
        public enum Operator
        {
            Plus,
            Minus,
            Multply,
            Divide,
            OperatorCounts
        }

        static int n;
        static int[] numbers;
        static int[] operators;

        static void Main(string[] args)
        {
            n = Convert.ToInt32(Console.ReadLine());
            numbers = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToArray();
            operators = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToArray();

            Node root = new Node() { level = 0, value = numbers[0], remainOperators = operators };
            int max = root.FindMax();
            int min = root.FindMin(); // getMin(n, numbers, operators);

            Console.WriteLine(max);
            Console.WriteLine(min);
        }

        public class Node
        {
            public int level;       // current number index
            public int value;       // result until current number
            public int[] remainOperators;

            internal int FindMax()
            {
                if (level == n - 1)
                    return value;

                // get child
                List<Node> childs = new List<Node>();
                for (int i = 0; i < (int)Operator.OperatorCounts; i++)
                {
                    if (remainOperators[i] > 0)
                    {
                        var nextRemain = remainOperators.ToArray();
                        nextRemain[i]--;
                        Node child = new Node()
                        {
                            level = level + 1,
                            value = calculate(value, (Operator)i, numbers[level + 1]),
                            remainOperators = nextRemain
                        };

                        childs.Add(child);
                    }
                }
                return childs.Max(c => c.FindMax());
            }

            private int calculate(int value, Operator i, int v)
            {
                switch (i)
                {
                    case Operator.Plus:
                        return value + v;
                    case Operator.Minus:
                        return value - v;
                    case Operator.Multply:
                        return value * v;
                    case Operator.Divide:
                        return value / v;
                }
                return -1;
            }

            internal int FindMin()
            {
                if (level == n - 1)
                    return value;

                // get child
                List<Node> childs = new List<Node>();
                for (int i = 0; i < (int)Operator.OperatorCounts; i++)
                {
                    if (remainOperators[i] > 0)
                    {
                        var nextRemain = remainOperators.ToArray();
                        nextRemain[i]--;
                        Node child = new Node()
                        {
                            level = level + 1,
                            value = calculate(value, (Operator)i, numbers[level + 1]),
                            remainOperators = nextRemain
                        };

                        childs.Add(child);
                    }
                }
                return childs.Min(c => c.FindMin());
            }
        }

    }
}
