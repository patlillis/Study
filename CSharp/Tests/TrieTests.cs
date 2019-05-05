using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharp.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSharp.Tests
{
    [TestClass]
    public class TrieTests
    {
        private Random _Rand { get; set; }

        private Dictionary<string, string> _Words = new Dictionary<string, string>();

        private Trie<string> _Trie { get; set; }

        public TrieTests()
        {
            this._Rand = new Random();

            HashSet<string> addedKeys = new HashSet<string> { "" };
            int numEntries = _Rand.Next(1000, 2000);
            for (int i = 0; i < numEntries; i++)
            {
                string key = "";
                while (addedKeys.Contains(key))
                {
                    key = TestUtils.Words[_Rand.Next(0, TestUtils.Words.Count)];
                }

                string value = TestUtils.Words[_Rand.Next(0, TestUtils.Words.Count)];

                _Words.Add(key, value);
                addedKeys.Add(key);
            }
        }

        [TestInitialize]
        public void _Initialize()
        {
            _Trie = new Trie<string>();
            foreach (var w in _Words)
            {
                _Trie.Add(w);
            }
        }

        [TestMethod]
        public void KeysTest()
        {
            var keys = _Trie.Keys;
            Assert.AreEqual(keys.Count, _Words.Count);
            foreach (var w in _Words)
            {
                Assert.IsTrue(keys.Contains(w.Key));
            }
        }

        [TestMethod]
        public void ValuesTest()
        {
            var values = _Trie.Values;
            Assert.AreEqual(values.Count, _Words.Count);
            foreach (var w in _Words)
            {
                Assert.IsTrue(values.Contains(w.Value));
            }
        }

        [TestMethod]
        public void IsReadOnlyTest()
        {
            Assert.IsFalse(_Trie.IsReadOnly);
        }

        [TestMethod]
        public void IndexerSetTest()
        {
            var t = new Trie<string>();

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
            foreach (var w in TestUtils.Words)
            {
                if (_Words.ContainsKey(w))
                {
                    Assert.AreEqual(_Trie[w], _Words[w]);
                }
                else
                {
                    Exception ex = null;

                    try
                    {
                        string unused = _Trie[w];
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
            _Trie.Clear();
            Assert.AreEqual(_Trie.Count, 0);
            foreach (var w in _Words)
            {
                Assert.IsFalse(_Trie.Contains(w));
                Assert.IsFalse(_Trie.ContainsKey(w.Key));
            }
        }

        [TestMethod]
        public void AddTest()
        {
            Trie<string> t = new Trie<string>();

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
            Trie<string> t = new Trie<string>();

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
            Stopwatch sw = Stopwatch.StartNew();

            foreach (string w in TestUtils.Words)
            {
                bool removed = _Trie.Remove(w);
                Assert.AreEqual(removed, _Words.ContainsKey(w));
                Assert.IsFalse(_Trie.ContainsKey(w));
                Assert.IsFalse(_Trie.Remove(w));
            }
        }

        [TestMethod]
        public void RemoveTest()
        {
            foreach (string w in TestUtils.Words)
            {
                if (_Words.ContainsKey(w))
                {
                    string value = _Words[w];

                    Assert.IsFalse(_Trie.Remove(new KeyValuePair<string, string>(w, value + "_test")));
                    Assert.IsTrue(_Trie.Remove(new KeyValuePair<string, string>(w, value)));
                    Assert.IsFalse(_Trie.ContainsKey(w));
                    Assert.IsFalse(_Trie.Remove(new KeyValuePair<string, string>(w, value)));
                }
                else
                {
                    Assert.IsFalse(_Trie.Remove(new KeyValuePair<string, string>(w, "")));
                }
            }
        }

        [TestMethod]
        public void FindTest()
        {
            foreach (var w in TestUtils.Words)
            {
                if (_Words.ContainsKey(w))
                {
                    Assert.AreEqual(_Trie.Find(w), _Words[w]);
                }
                else
                {
                    Exception ex = null;

                    try
                    {
                        string unused = _Trie.Find(w);
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
            foreach (var w in TestUtils.Words)
            {
                string value;

                Assert.IsFalse(_Trie.TryGetValue(w + "_Test", out value));
                Assert.IsNull(value);

                if (_Words.ContainsKey(w))
                {
                    Assert.IsTrue(_Trie.TryGetValue(w, out value));
                    Assert.AreEqual(_Trie.Find(w), _Words[w]);
                }
                else
                {
                    Assert.IsFalse(_Trie.TryGetValue(w, out value));
                    Assert.IsNull(value);
                }
            }
        }

        [TestMethod]
        public void ContainsKeyTest()
        {
            foreach (var w in TestUtils.Words)
            {
                Assert.AreEqual(_Words.ContainsKey(w), _Trie.ContainsKey(w));
            }
        }

        [TestMethod]
        public void ContainsTest()
        {
            foreach (var w in TestUtils.Words)
            {
                if (_Words.ContainsKey(w))
                {
                    Assert.IsTrue(_Trie.Contains(w, _Words[w]));
                    Assert.IsFalse(_Trie.Contains(w, _Words[w] + "_Test"));
                }
                else
                {
                    Assert.IsFalse(_Trie.Contains(w, "Test"));
                    Assert.IsFalse(_Trie.Contains(w, ""));
                }
            }
        }

        [TestMethod]
        public void ContainsKVPTest()
        {
            foreach (var w in TestUtils.Words)
            {
                if (_Words.ContainsKey(w))
                {
                    Assert.IsTrue(_Trie.Contains(new KeyValuePair<string, string>(w, _Words[w])));
                    Assert.IsFalse(_Trie.Contains(new KeyValuePair<string, string>(w, _Words[w] + "_Test")));
                }
                else
                {
                    Assert.IsFalse(_Trie.Contains(new KeyValuePair<string, string>(w, "Test")));
                    Assert.IsFalse(_Trie.Contains(new KeyValuePair<string, string>(w, "")));
                }
            }
        }

        [TestMethod]
        public void CopyToTest()
        {
            //Test null array
            Exception ex = null;
            try { _Trie.CopyTo(null, 0); }
            catch (Exception innerEx)
            {
                ex = innerEx;
            }
            Assert.IsInstanceOfType(ex, typeof(ArgumentException));

            //Test index out of range
            ex = null;
            try { _Trie.CopyTo(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>() }, -1); }
            catch (Exception innerEx)
            {
                ex = innerEx;
            }
            Assert.IsInstanceOfType(ex, typeof(ArgumentException));

            //Test arrayIndex too large
            ex = null;
            try { _Trie.CopyTo(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>() }, 1); }
            catch (Exception innerEx)
            {
                ex = innerEx;
            }
            Assert.IsInstanceOfType(ex, typeof(ArgumentException));

            var arr = new KeyValuePair<string, string>[_Words.Count];
            _Trie.CopyTo(arr, 0);
            foreach (var item in arr)
            {
                Assert.IsNotNull(item.Key);
                Assert.IsNotNull(item.Value);

                Assert.IsTrue(_Words.ContainsKey(item.Key));
                Assert.AreEqual(_Words[item.Key], item.Value);
            }
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            IEnumerator<KeyValuePair<string, string>> enumerator = _Trie.GetEnumerator();
            Assert.IsNotNull(enumerator);

            int count = 0;
            while (enumerator.MoveNext())
            {
                count++;
                var item = enumerator.Current;
                Assert.IsTrue(_Words.ContainsKey(item.Key));
                Assert.AreEqual(item.Value, _Words[item.Key]);
            }

            Assert.AreEqual(count, _Words.Count);
        }

        [TestMethod]
        public void GetIEnumerableEnumeratorTest()
        {
            IEnumerator enumerator = ((IEnumerable)_Trie).GetEnumerator();
            Assert.IsNotNull(enumerator);

            int count = 0;
            while (enumerator.MoveNext())
            {
                count++;
                var item = enumerator.Current;
                Assert.IsInstanceOfType(item, typeof(KeyValuePair<string, string>));

                KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)item;

                Assert.IsTrue(_Words.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, _Words[kvp.Key]);
            }

            Assert.AreEqual(count, _Words.Count);
        }
    }
}
