using Microsoft.VisualStudio.TestTools.UnitTesting;
using Study.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Study.Tests
{
	[TestClass]
	public class SplayTreeTests
	{
		private Random _Rand { get; set; }

		public SplayTreeTests()
		{
			this._Rand = new Random();
		}

		[TestMethod]
		public void FindTest()
		{
			foreach (int repeat in Enumerable.Range(0, 10))
			{
				SplayTree<int, int> t = new SplayTree<int, int>();
				Dictionary<int, int> addedKeys = new Dictionary<int, int>();
				
				foreach (int i in Enumerable.Range(0, 100))
				{
					int key = this._Rand.Next();
					int value1 = this._Rand.Next();
					int value2 = this._Rand.Next();

					t[key] = value1;
					Assert.AreEqual(t.Find(key), value1);
					t[key] = value2;
					Assert.AreEqual(t.Find(key), value2);

					addedKeys[key] = value2;
				}
				
				// Test in order
				foreach (var key in addedKeys)
				{
					Assert.AreEqual(t.Find(key.Key), key.Value);
				}
			}
		}

		[TestMethod]
		public void FindDeletedKeyTest()
		{
			SplayTree<int, int> t = new SplayTree<int, int>();
			HashSet<int> addedKeys = new HashSet<int>();

			foreach (int i in Enumerable.Range(0, 100))
			{
				int key = this._Rand.Next();
				addedKeys.Add(key);

				t.Add(key, this._Rand.Next());
			}

			foreach (var key in addedKeys)
			{
				t.Remove(key);

				Exception ex = null;
				try
				{
					t.Find(key);
				}
				catch (Exception innerEx)
				{
					ex = innerEx;
				}

				Assert.IsInstanceOfType(ex, typeof(KeyNotFoundException));
			}
		}

		[TestMethod]
		public void CountTest()
		{
			foreach (int repeat in Enumerable.Range(0, 10))
			{
				SplayTree<int, int> t = new SplayTree<int, int>();
				HashSet<int> addedKeys = new HashSet<int>();

				int count = 0;
				foreach (int i in Enumerable.Range(0, 100))
				{
					int key = this._Rand.Next();

					if (addedKeys.Add(key))
					{
						count++;

						t[key] = this._Rand.Next();
						Assert.AreEqual(t.Count, count);
						t[key] = this._Rand.Next();
						Assert.AreEqual(t.Count, count);

					}
				}

				count = t.Count;
				foreach (int i in addedKeys)
				{
					t.Remove(i);
					count--;

					Assert.AreEqual(t.Count, count);

					t.Remove(i);
					Assert.AreEqual(t.Count, count);
				}

				foreach (int i in Enumerable.Range(0, 100))
				{
					int key = this._Rand.Next();

					t[key] = this._Rand.Next();
					Assert.AreEqual(t.Count, 1);

					t.Remove(key);
					Assert.AreEqual(t.Count, 0);
				}
			}
		}
	}
}
