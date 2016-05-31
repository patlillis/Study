using Microsoft.VisualStudio.TestTools.UnitTesting;
using Study.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using Item = Study.Algorithms.KnapsackProblem.Item;

namespace Study.Tests
{
	[TestClass]
	public class KnapsackTests
	{
		private Random _Rand { get; set; }

		public KnapsackTests()
		{
			this._Rand = new Random();
		}

		[TestMethod]
		public void CapacityZeroTest()
		{
			List<int> solution;

			// 0 capacity, 0 items => 0
			solution = KnapsackProblem.Solve(0, new List<Item>());
			Assert.AreEqual(solution.Count, 0);


			// 0 capacity, 1 item => 0
			List<Item> items = new List<Item>
			{
				new Item(5, 5)
			};
			
			solution = KnapsackProblem.Solve(0, items);
			Assert.AreEqual(solution.Count, 0);


			// 0 capacity, bunch of items => 0
			items = new List<Item>();
			for (int i = 0; i < 100; i++)
				items.Add(new Item(this._Rand.Next(), this._Rand.Next()));

			solution = KnapsackProblem.Solve(0, items);
			Assert.AreEqual(solution.Count, 0);
		}

		[TestMethod]
		public void FindsSolutionTest()
		{
			for (int i = 0; i < 10; i++)
			{
				List<Item> items = new List<Item>();

				for (int j = 0; j < 1000; j++)
				{
					Item item = new Item(this._Rand.Next(1, 1000), this._Rand.Next());
					items.Add(item);
				}

				List<int> weights = items.Select(item => item.Weight).ToList();
				int capacity = this._Rand.Next(weights.Min(), weights.Max());
				List<int> solution = KnapsackProblem.Solve(capacity, items);
				int totalUsedCapacity = solution.Sum(s => weights[s]);
				Assert.IsTrue(totalUsedCapacity <= capacity);
			}
		}

		[TestMethod]
		public void GreedyTest()
		{
			List<int> solution;

			// Most valuable item should be the only one included
			List<Item> items = new List<Item>
			{
				new Item(1, 500),
				new Item(1, 300),
				new Item(2, 500)
			};

			solution = KnapsackProblem.Solve(1, items);
			Assert.AreEqual(solution.Count, 1);
			Assert.IsTrue(solution.Contains(0));

			items.Add(new Item(1, 500));
			solution = KnapsackProblem.Solve(2, items);
			Assert.AreEqual(solution.Count, 2);
			Assert.IsTrue(solution.Contains(0));
			Assert.IsTrue(solution.Contains(3));
		}

		[TestMethod]
		public void CombineLessValuableTest()
		{
			List<int> solution;

			// Most valuable item should be the only one included
			List<Item> items = new List<Item>
			{
				new Item(55, 55),
				new Item(50, 45),
				new Item(50, 45)
			};

			solution = KnapsackProblem.Solve(100, items);
			Assert.AreEqual(solution.Count, 2);
			Assert.IsTrue(solution.Contains(1));
			Assert.IsTrue(solution.Contains(2));
		}
	}
}
