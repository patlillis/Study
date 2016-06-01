using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;


namespace Study.DataStructures.Tries
{
	public class Trie<TValue> : IDictionary<string, TValue>
	{
		private class Node
		{
			public char C { get; set; }
			public string Key { get; set; }
			public Dictionary<char, Node> Children { get; set; }
			public Node Parent { get; set; }
			public bool IsTerminal { get; set; }
			public TValue Value { get; set; }

			public Node()
			{
				Children = new Dictionary<char, Node>();
				IsTerminal = false;
			}

			public Node(Node parent, char c)
				: this()
			{
				Parent = parent;
				C = c;
			}
		}

		public Trie()
		{
			_Root = new Node();
		}

		private Node _Root { get; set; }

		public int Count { get; set; }

		public ICollection<string> Keys
		{
			get
			{
				List<string> keys = new List<string>();
				foreach (Node node in this._GetSubTrie(_Root))
				{
					if (node.IsTerminal)
						keys.Add(node.Key);
				}

				return keys;
			}
		}

		public ICollection<TValue> Values
		{
			get
			{
				List<TValue> values = new List<TValue>();
				foreach (Node node in this._GetSubTrie(_Root))
				{
					if (node.IsTerminal)
						values.Add(node.Value);
				}

				return values;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public TValue this[string key]
		{
			get { return this.Find(key); }

			set { this._Add(key, value, true); }
		}

		#region Private Methods

		private Node _FindNode(string key)
		{
			Node node = _Root;

			foreach (char c in key)
			{
				if (!node.Children.ContainsKey(c))
					return null;

				node = node.Children[c];
			}

			if (node.IsTerminal)
				return node;
			else
				return null;
		}

		/// <summary>
		/// Add key/value pair to tree
		/// </summary>
		/// <param name="key">Key to add</param>
		/// <param name="value">Value associated with key</param>
		/// <param name="overwrite">If key is already present, should it be overwritten with new value?</param>
		/// <returns>True if key was set to value, false otherwise</returns>
		private bool _Add(string key, TValue value, bool overwite)
		{
			bool inserted = false;

			Node node = _Root;
			int charIndex = 0;

			//Go as far as we can in the existing tree
			for (; charIndex < key.Length; charIndex++)
			{
				char c = key[charIndex];

				if (!node.Children.ContainsKey(c))
					node.Children.Add(c, new Node(node, c));
				
				node = node.Children[c];
			}
			
			//Only want to set to value if either
			//	(1) this is just an internal node (so it doesn't have a value)
			//	(2) overwrite is true.
			if (!node.IsTerminal || overwite)
			{
				if (!node.IsTerminal)
				{
					//Converting a non-terminal to a terminal means incrementing the count.
					Count++;
				}

				node.IsTerminal = true;
				node.Key = key;
				node.Value = value;
				inserted = true;
            }

			return inserted;
		}
		
		private IEnumerable<Node> _GetSubTrie(Node parent)
		{
			if (parent != null)
			{
				yield return parent;

				foreach (Node child in parent.Children.Values)
					foreach (Node n in _GetSubTrie(child))
						yield return n;
			}
		}

		#endregion Private Methods

		public void Clear()
		{
			_Root.IsTerminal = false;
			_Root.Children.Clear();
			_Root.Key = null;
			_Root.Value = default(TValue);
			Count = 0;
		}

		/// <summary>
		/// Add new value
		/// </summary>
		/// <exception cref="ArgumentException">Thrown when an element with the same key already exists.</exception>
		public void Add(string key, TValue value)
		{
			bool added = this._Add(key, value, false);

			if (!added)
				throw new ArgumentException($"An element with the key {key} already exists.");
		}

		public void Add(KeyValuePair<string, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		public bool Remove(string key)
		{
			Node node = _FindNode(key);

			if (node == null)
				return false;

			//Case 1 - node has children.
			//	don't delete, just set IsTerminal to false
			if (node.Children.Count > 0)
			{
				node.IsTerminal = false;
			}
			//Case 2 - Node has no children.
			// delete node, and any parents that don't have children and aren't terminals.
			else
			{
				char c = node.C;
				node = node.Parent;

				while (node != null)
				{
					node.Children.Remove(c);

					//See if we should continue up the tree
					if (node.Children.Count == 0 && !node.IsTerminal)
					{
						c = node.C;
						node = node.Parent;
					}
					else
					{
						break;
					}
				}
			}

			Count--;
			return true;
		}

		public bool Remove(KeyValuePair<string, TValue> item)
		{
			Node node = this._FindNode(item.Key);

			if (node != null && node.Value.Equals(item.Value))
			{
				this.Remove(item.Key);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Find the value in the tree associated with the key
		/// </summary>
		/// <exception cref="KeyNotFoundException">Thrown when the given key is not present.</exception>
		public TValue Find(string key)
		{
			// Find appropriate node
			Node node = this._FindNode(key);

			// Return or throw exception
			if (node == null)
				throw new KeyNotFoundException($"Could not find value for key {key}.");
			else
				return node.Value;
		}

		public bool TryGetValue(string key, out TValue value)
		{
			Node node = _FindNode(key);

			if (node != null)
			{
				value = node.Value;
				return true;
			}
			else
			{
				value = default(TValue);
				return false;
			}
		}

		public bool ContainsKey(string key)
		{
			Node node = _FindNode(key);

			return node != null;
		}

		public bool Contains(string key, TValue value)
		{
			Node node = _FindNode(key);

			return node != null && node.Value.Equals(value);
		}

		public bool Contains(KeyValuePair<string, TValue> item)
		{
			return Contains(item.Key, item.Value);
		}

		/// <summary>
		/// Copies the elements from the tree into the array, beginning at arrayIndex
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		/// <exception cref="ArgumentException">Thrown when array is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than 0</exception>
		/// <exception cref="ArgumentException">Thrown when the number of elements in the tree is greated than the available space from index to the end of the destination array</exception>
		public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentException("null parameter", nameof(array));

			if (arrayIndex < 0)
				throw new ArgumentException("arrayIndex less then zero", nameof(arrayIndex));

			if (this.Count > (array.Length - arrayIndex))
				throw new ArgumentException("The number of elements in the tree is greated than the available space from index to the end of the destination array", nameof(arrayIndex));


			IEnumerator<KeyValuePair<string, TValue>> allNodes = this.GetEnumerator();


			int i = 0;
			while (allNodes.MoveNext())
				array[i++ + arrayIndex] = allNodes.Current;
		}

		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
		{
			foreach (Node node in this._GetSubTrie(this._Root))
			{
				if (node.IsTerminal)
					yield return new KeyValuePair<string, TValue>(node.Key, node.Value);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
