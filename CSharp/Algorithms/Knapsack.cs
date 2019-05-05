using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp.Algorithms
{
    public static class KnapsackProblem
    {
        public struct Item
        {
            public int Weight { get; set; }
            public int Value { get; set; }

            public Item(int weight, int value)
            {
                this.Weight = weight;
                this.Value = value;
            }

            public override string ToString()
            {
                return $"Weight: {this.Weight}, Value: {this.Value}";
            }
        }

        /// <summary>
        /// Solves the 0/1 Knapsack Problem
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="items"></param>
        /// <param name="fractional"></param>
        /// <returns></returns>
        public static List<int> Solve(int capacity, List<Item> items)
        {
            List<int> itemsToTake = new List<int>();

            int[] weights = items.Select(x => x.Weight).ToArray();
            int[] values = items.Select(x => x.Value).ToArray();
            int[,] optimalWeights = new int[items.Count + 1, capacity + 1];

            int i;
            int c;

            // arr[i, c] is optimal weight less than c that we can achieve using only the first i items

            // arr[0, c] = 0
            // arr[i, c] = arr[i - 1, c] if weights[i] > c
            // arr[i, c] = max(arr[i - 1, c], arr[i - 1, c - weights[i]] + values[i]) if weights[i] << c

            for (i = 1; i < items.Count + 1; i++)
            {
                for (c = 0; c <= capacity; c++)
                {
                    if (weights[i - 1] > c)
                        optimalWeights[i, c] = optimalWeights[i - 1, c];
                    else
                        optimalWeights[i, c] = Math.Max(optimalWeights[i - 1, c], optimalWeights[i - 1, c - weights[i - 1]] + values[i - 1]);
                }
            }


            // Work backwards to compute which items to take.
            i = items.Count;
            c = capacity;

            while (i > 0)
            {
                int weightWithItem = optimalWeights[i, c];
                int weightWithoutItem = optimalWeights[i - 1, c];

                if (weightWithItem == weightWithoutItem)
                {
                    // Item shouldn't be included
                    i--;
                }
                else
                {
                    // Item should be included!
                    itemsToTake.Add(i - 1);
                    c -= weights[i - 1];
                    i--;
                }
            }

            itemsToTake.Sort();
            return itemsToTake;
        }
    }
}
