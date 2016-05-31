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

		private Dictionary<string, string> _Words = new Dictionary<string, string>();

		private SplayTree<string, string> _Tree { get; set; }

		public SplayTreeTests()
		{
			this._Rand = new Random();

			HashSet<string> addedKeys = new HashSet<string> { "" };
			int numEntries = _Rand.Next(1000, 2000);
			for (int i = 0; i < numEntries; i++)
			{
				string key = "";
				while (addedKeys.Contains(key))
				{
					key = Utils.Words[_Rand.Next(0, Utils.Words.Count)];
				}

				string value = Utils.Words[_Rand.Next(0, Utils.Words.Count)];

				_Words.Add(key, value);
				addedKeys.Add(key);
			}
		}

		[TestInitialize]
		public void _Initialize()
		{
			_Tree = new SplayTree<string, string>();
			foreach (var w in _Words)
			{
				_Tree.Add(w);
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

		[TestMethod]
		public void KeysTest()
		{
			var keys = _Tree.Keys;
			Assert.AreEqual(keys.Count, _Words.Count);
			foreach (var w in _Words)
			{
				Assert.IsTrue(keys.Contains(w.Key));
			}
		}

		[TestMethod]
		public void ValuesTest()
		{
			var values = _Tree.Values;
			Assert.AreEqual(values.Count, _Words.Count);
			foreach (var w in _Words)
			{
				Assert.IsTrue(values.Contains(w.Value));
			}
		}

		[TestMethod]
		public void IsReadOnlyTest()
		{
			Assert.IsFalse(_Tree.IsReadOnly);
		}

		[TestMethod]
		public void IndexerSetTest()
		{
			var t = new SplayTree<string, string>();

			foreach (var w in _Words)
			{
				t[w.Key] = w.Value + "_Test";
				t[w.Key] = w.Value;
			}

			Assert.AreEqual(t.Count, _Words.Count);
			foreach (var w in _Words)
			{
				Assert.IsTrue(t.Contains(w));
			}
		}

		[TestMethod]
		public void IndexerGetTest()
		{
			foreach (var w in Utils.Words)
			{
				if (_Words.ContainsKey(w))
				{
					Assert.AreEqual(_Tree[w], _Words[w]);
				}
				else
				{
					Exception ex = null;

					try
					{
						string unused = _Tree[w];
					}
					catch (Exception innerEx)
					{
						ex = innerEx;
					}

					Assert.IsInstanceOfType(ex, typeof(KeyNotFoundException));
				}
			}
		}

		[TestMethod]
		public void ClearTest()
		{
			_Tree.Clear();
			Assert.AreEqual(_Tree.Count, 0);
			foreach (var w in _Words)
			{
				Assert.IsFalse(_Tree.Contains(w));
				Assert.IsFalse(_Tree.ContainsKey(w.Key));
			}
		}

		[TestMethod]
		public void AddTest()
		{
			SplayTree<string, string> t = new SplayTree<string, string>();

			foreach (var w in _Words)
			{
				t.Add(w.Key, w.Value);
				Assert.IsTrue(t.Contains(w.Key, w.Value));

				Exception ex = null;

				try
				{
					t.Add(w.Key, w.Value + "_Test");
				}
				catch (Exception innerEx)
				{
					ex = innerEx;
				}

				Assert.IsInstanceOfType(ex, typeof(ArgumentException));
			}
		}

		[TestMethod]
		public void AddKVPTest()
		{
			SplayTree<string, string> t = new SplayTree<string, string>();

			foreach (var w in _Words)
			{
				t.Add(new KeyValuePair<string, string>(w.Key, w.Value));
				Assert.IsTrue(t.Contains(w.Key, w.Value));

				Exception ex = null;

				try
				{
					t.Add(new KeyValuePair<string, string>(w.Key, w.Value + "_Test"));
				}
				catch (Exception innerEx)
				{
					ex = innerEx;
				}

				Assert.IsInstanceOfType(ex, typeof(ArgumentException));
			}
		}

		[TestMethod]
		public void RemoveKeyTest()
		{
			foreach (string w in Utils.Words)
			{
				bool removed = _Tree.Remove(w);
				Assert.AreEqual(removed, _Words.ContainsKey(w));
				Assert.IsFalse(_Tree.ContainsKey(w));
				Assert.IsFalse(_Tree.Remove(w));
			}
		}

		[TestMethod]
		public void RemoveTest()
		{
			foreach (string w in Utils.Words)
			{
				if (_Words.ContainsKey(w))
				{
					string value = _Words[w];

					Assert.IsFalse(_Tree.Remove(new KeyValuePair<string, string>(w, value + "_test")));
					Assert.IsTrue(_Tree.Remove(new KeyValuePair<string, string>(w, value)));
					Assert.IsFalse(_Tree.ContainsKey(w));
					Assert.IsFalse(_Tree.Remove(new KeyValuePair<string, string>(w, value)));
				}
				else
				{
					Assert.IsFalse(_Tree.Remove(new KeyValuePair<string, string>(w, "")));
				}
			}
		}

		[TestMethod]
		public void FindTest()
		{
			foreach (var w in Utils.Words)
			{
				if (_Words.ContainsKey(w))
				{
					Assert.AreEqual(_Tree.Find(w), _Words[w]);
				}
				else
				{
					Exception ex = null;

					try
					{
						string unused = _Tree.Find(w);
					}
					catch (Exception innerEx)
					{
						ex = innerEx;
					}

					Assert.IsInstanceOfType(ex, typeof(KeyNotFoundException));
				}
			}
		}

		[TestMethod]
		public void TryGetValueTest()
		{
			foreach (var w in Utils.Words)
			{
				string value;

				Assert.IsFalse(_Tree.TryGetValue(w + "_Test", out value));
				Assert.IsNull(value);

				if (_Words.ContainsKey(w))
				{
					Assert.IsTrue(_Tree.TryGetValue(w, out value));
					Assert.AreEqual(_Tree.Find(w), _Words[w]);
				}
				else
				{
					Assert.IsFalse(_Tree.TryGetValue(w, out value));
					Assert.IsNull(value);
				}
			}
		}

		[TestMethod]
		public void ContainsKeyTest()
		{
			foreach (var w in Utils.Words)
			{
				Assert.AreEqual(_Words.ContainsKey(w), _Tree.ContainsKey(w));
			}
		}

		[TestMethod]
		public void ContainsTest()
		{
			foreach (var w in Utils.Words)
			{
				if (_Words.ContainsKey(w))
				{
					Assert.IsTrue(_Tree.Contains(w, _Words[w]));
					Assert.IsFalse(_Tree.Contains(w, _Words[w] + "_Test"));
				}
				else
				{
					Assert.IsFalse(_Tree.Contains(w, "Test"));
					Assert.IsFalse(_Tree.Contains(w, ""));
				}
			}
		}

		[TestMethod]
		public void ContainsKVPTest()
		{
			foreach (var w in Utils.Words)
			{
				if (_Words.ContainsKey(w))
				{
					Assert.IsTrue(_Tree.Contains(new KeyValuePair<string, string>(w, _Words[w])));
					Assert.IsFalse(_Tree.Contains(new KeyValuePair<string, string>(w, _Words[w] + "_Test")));
				}
				else
				{
					Assert.IsFalse(_Tree.Contains(new KeyValuePair<string, string>(w, "Test")));
					Assert.IsFalse(_Tree.Contains(new KeyValuePair<string, string>(w, "")));
				}
			}
		}

		[TestMethod]
		public void CopyToTest()
		{
			//Test null array
			Exception ex = null;
			try { _Tree.CopyTo(null, 0); }
			catch (Exception innerEx)
			{
				ex = innerEx;
			}
			Assert.IsInstanceOfType(ex, typeof(ArgumentException));

			//Test index out of range
			ex = null;
			try { _Tree.CopyTo(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>() }, -1); }
			catch (Exception innerEx)
			{
				ex = innerEx;
			}
			Assert.IsInstanceOfType(ex, typeof(ArgumentException));

			//Test arrayIndex too large
			ex = null;
			try { _Tree.CopyTo(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>() }, 1); }
			catch (Exception innerEx)
			{
				ex = innerEx;
			}
			Assert.IsInstanceOfType(ex, typeof(ArgumentException));

			var arr = new KeyValuePair<string, string>[_Words.Count];
			_Tree.CopyTo(arr, 0);

			//TODO: Add Asserts
		}
	}
}
