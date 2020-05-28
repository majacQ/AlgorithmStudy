using System;
using System.Collections.Generic;
using System.Linq;

namespace Baekjoon_2655
{
    public class Program
    {
        private static int bricksCount;

        private static int[] bricksHeight = new int[100];
        private static int[] bricksWeight = new int[100];
        private static int[] bricksArea = new int[100];
        private static int[] bricksCalculatedHeight = new int[100];
        private static int[] bricksCalculatedIndex = new int[100];


        static void Main(string[] args)
        {
            bricksCount = Convert.ToInt32(Console.ReadLine());

            for(int i=0; i<bricksCount; i++)
            {
                var tokens = Console.ReadLine().Split(' ').Select(token => Convert.ToInt32(token)).ToList();
                bricksArea[i] = tokens[0];
                bricksHeight[i] = tokens[1];
                bricksWeight[i] = tokens[2];
            }

            var maxHeightForEachBricks = Enumerable.Range(0, bricksCount).Select(x => calculateMaximumHeighest(x)).ToList();
            var maxHeightBrickIndex = maxHeightForEachBricks.IndexOf(maxHeightForEachBricks.Max());

            Stack<int> BrickHistory = new Stack<int>();
            int currentBrickIndex = maxHeightBrickIndex;
            while(bricksCalculatedIndex[currentBrickIndex] != -1)
            {
                BrickHistory.Push(currentBrickIndex);
                currentBrickIndex = bricksCalculatedIndex[currentBrickIndex];
            }
            BrickHistory.Push(currentBrickIndex);

            Console.WriteLine(BrickHistory.Count);
            while (BrickHistory.Count > 0)
            {
                Console.WriteLine(BrickHistory.Peek() + 1);
                BrickHistory.Pop();
            }
        }

        static int calculateMaximumHeighest(int i)
        {
            if(bricksCalculatedHeight[i] == 0)
            {
                var availableChildIndex = Enumerable.Range(0, bricksCount).Where(x => bricksWeight[x] < bricksWeight[i]).Where(y => bricksArea[y] < bricksArea[i]).ToList();

                if (availableChildIndex.Count == 0) // bricks[i] is topest bricks
                {
                    bricksCalculatedHeight[i] = bricksHeight[i];
                    bricksCalculatedIndex[i] = -1;
                }
                else
                {
                    var availableHeightsForEachChild = availableChildIndex.Select(x => calculateMaximumHeighest(x)).ToList();

                    int largestChildHeight = availableHeightsForEachChild.Max();
                    int largestChildIndex = availableChildIndex[availableHeightsForEachChild.IndexOf(largestChildHeight)];

                    bricksCalculatedHeight[i] = bricksHeight[i] + largestChildHeight;
                    bricksCalculatedIndex[i] = largestChildIndex;
                }
            }

            return bricksCalculatedHeight[i];
        }

    }
}
