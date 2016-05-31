using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Study.DataStructures
{
	/// <summary>
	/// A self-adjusting binary search tree with the additional property that recently accessed elements are quick to access again.
	/// </summary>
	public class SplayTree<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
	{
		/// <summary>
		/// Inner nodes
		/// </summary>
		private class Node
		{
			public TKey Key;
			public TValue Value;

			public Node Parent;
			public Node Left, Right;

			public Node(TKey key, TValue value)
			{
				this.Key = key;
				this.Value = value;
			}
		}

		private Node _Root { get; set; }

		public int Count { get; set; }

		public ICollection<TKey> Keys
		{
			get
			{
				List<TKey> keys = new List<TKey>();
				foreach (Node node in this._GetSubTree(_Root))
					keys.Add(node.Key);

				return keys;
			}
		}

		public ICollection<TValue> Values
		{
			get
			{
				List<TValue> values = new List<TValue>();
				foreach (Node node in this._GetSubTree(_Root))
					values.Add(node.Value);

				return values;
			}
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public TValue this[TKey key]
		{
			get { return this.Find(key); }

			set { this._Add(key, value, true); }
		}


		#region Private Methods

		/// <summary>
		/// Rotate tree until node is at root
		/// </summary>
		/// <param name="node">The node that should go to the root</param>
		private void _Splay(Node node)
		{
			// Keep splaying until node is at root
			while (node.Parent != null)
			{
				if (node.Parent.Parent == null)
				{
					// Either Zig or Zag
					// This is just a simple rotation.
					if (node.Parent.Left == node)
						_RightRotate(node.Parent);
					else
						_LeftRotate(node.Parent);
				}
				else
				{
					// Definitely have a grandparent.
					// This means we have to do 2 rotations

					if (node.Parent.Left == node)
					{
						// node is Left child of parent.

						if (node.Parent.Parent.Left == node.Parent)
						{
							// Zig Zig
							_RightRotate(node.Parent.Parent);
							_RightRotate(node.Parent);
						}
						else
						{
							//Zig Zag
							_RightRotate(node.Parent);
							//node's grandparent has now become node's parent
							_LeftRotate(node.Parent);
						}
					}
					else
					{
						// node is Right child of parent.

						if (node.Parent.Parent.Right == node.Parent)
						{
							// Zag Zag
							_LeftRotate(node.Parent.Parent);
							_LeftRotate(node.Parent);
						}
						else
						{
							//Zag Zig
							_LeftRotate(node.Parent);
							//node's grandparent has now become node's parent
							_RightRotate(node.Parent);
						}
					}
				}
			}
		}

		/// <summary>
		/// Rotate once right. Node's left child will become its parent
		/// </summary>
		/// <param name="node">The node that will be rotated</param>
		private void _RightRotate(Node node)
		{
			Node replacement = node.Left;

			if (replacement != null)
			{
				// Set up relationship between parent and replacement
				replacement.Parent = node.Parent;
				if (node.Parent == null)
				{
					this._Root = replacement;
				}
				else
				{
					if (node.Parent.Left == node)
						node.Parent.Left = replacement;
					else
						node.Parent.Right = replacement;
				}

				// Move replacement's right child to node's left slot
				node.Left = replacement.Right;
				if (node.Left != null)
					node.Left.Parent = node;

				// Set up relationship between node and replacement
				replacement.Right = node;
				node.Parent = replacement;
			}
		}

		/// <summary>
		/// Rotate once left. Node's right child will become its parent
		/// </summary>
		/// <param name="node">The node that will be rotated</param>
		private void _LeftRotate(Node node)
		{
			Node replacement = node.Right;

			if (replacement != null)
			{
				// Set up relationship between parent and replacement
				replacement.Parent = node.Parent;
				if (node.Parent == null)
				{
					this._Root = replacement;
				}
				else
				{
					if (node.Parent.Right == node)
						node.Parent.Right = replacement;
					else
						node.Parent.Left = replacement;
				}

				// Move replacement's left child to node's right slot
				node.Right = replacement.Left;
				if (node.Right != null)
					node.Right.Parent = node;

				// Set up relationship between node and replacement
				replacement.Left = node;
				node.Parent = replacement;
			}
		}

		/// <summary>
		/// Find the node in the tree associated with the key
		/// Returns null if key is not in tree
		/// </summary>
		private Node _FindNode(TKey key)
		{
			Node node = this._Root;

			// Keep traversing tree until we find key or reach a leaf.
			while (node != null)
			{
				int cmp = key.CompareTo(node.Key);

				if (cmp < 0)
				{
					// Key is somewhere to the left
					node = node.Left;
				}
				else if (cmp > 0)
				{
					// Key is somewhere to the right
					node = node.Right;
				}
				else
				{
					// Found it!
					return node;
				}
			}

			//Could not find key
			return null;
		}

		/// <summary>
		/// Similar to rotation, but node a is completely removed from the tree, and replace by node b
		/// </summary>
		/// <param name="a">Node to be removed</param>
		/// <param name="b">Node to take a's place</param>
		private void _Replace(Node a, Node b)
		{
			if (a.Parent == null)
			{
				// a used to be root, now b is
				this._Root = b;
			}
			else if (a == a.Parent.Left)
			{
				// a was left child
				a.Parent.Left = b;
			}
			else
			{
				// a was right child
				a.Parent.Right = b;
			}

			// Make sure b's parent pointer is correct
			if (b != null)
				b.Parent = a.Parent;
		}

		/// <summary>
		/// Find the smallest descendent of n
		/// </summary>
		private Node _SubtreeMinimum(Node n)
		{
			while (n != null && n.Left != null)
				n = n.Left;

			return n;
		}

		/// <summary>
		/// Find the largest descendent of n
		/// </summary>
		private Node _SubtreeMaximum(Node n)
		{
			while (n != null && n.Right != null)
				n = n.Right;

			return n;
		}

		/// <summary>
		/// Add key/value pair to tree
		/// </summary>
		/// <param name="key">Key to add</param>
		/// <param name="value">Value associated with key</param>
		/// <param name="overwrite">If key is already present, should it be overwritten with new value?</param>
		/// <returns>True if key was set to value, false otherwise</returns>
		private bool _Add(TKey key, TValue value, bool overwrite)
		{
			// Get the parent of the position we should insert
			Node node = this._Root;
			Node parent = null;
			bool inserted = false;

			while (node != null)
			{
				parent = node;

				int cmp = key.CompareTo(node.Key);
				if (cmp < 0)
				{
					// Key should go somewhere to the left
					node = node.Left;
				}
				else if (cmp > 0)
				{
					// Key should go somewhere to the right
					node = node.Right;
				}
				else
				{
					// Key should go right here!
					if (overwrite)
					{
						node.Value = value;
						inserted = true;
					}
					break;
				}
			}


			if (node == null)
			{
				// Need to insert a new node.
				node = new Node(key, value);
				node.Parent = parent;
				if (parent == null)
					this._Root = node;
				else if (key.CompareTo(parent.Key) < 0)
					parent.Left = node;
				else
					parent.Right = node;

				this.Count++;
				inserted = true;
			}

			this._Splay(node);

			return inserted;
		}

		private void _Remove(Node node)
		{
			// If key wasn't in tree, don't worry about it
			if (node != null)
			{
				// Splay
				this._Splay(node);

				if (node.Left == null)
				{
					// Replace node with its right child
					this._Replace(node, node.Right);
				}
				else if (node.Right == null)
				{
					// Replace node with its left child
					this._Replace(node, node.Left);
				}
				else
				{
					// Node has both children, need to be a little smarter
					Node min = this._SubtreeMinimum(node.Right);
					if (min.Parent != node)
					{
						this._Replace(min, min.Right);
						min.Right = node.Right;
						min.Right.Parent = min;
					}

					this._Replace(node, min);
					min.Left = node.Left;
					min.Left.Parent = min;
				}

				this.Count--;
			}
		}

		private IEnumerable<Node> _GetSubTree(Node node)
		{
			if (node != null)
			{
				if (node.Left != null)
				{
					foreach (Node l in this._GetSubTree(node.Left))
						yield return l;
				}

				yield return node;

				if (node.Right != null)
				{
					foreach (Node r in this._GetSubTree(node.Right))
						yield return r;
				}
			}
		}

		#endregion


		/// <summary>
		/// Add new value
		/// </summary>
		/// <exception cref="ArgumentException">Thrown when an element with the same key already exists.</exception>
		public void Add(TKey key, TValue value)
		{
			bool added = this._Add(key, value, false);

			if (!added)
				throw new ArgumentException($"An element with the key {key} already exists.");
		}

		/// <summary>
		/// Find the value in the tree associated with the key
		/// </summary>
		/// <exception cref="KeyNotFoundException">Thrown when the given key is not present.</exception>
		public TValue Find(TKey key)
		{
			// Find appropriate node
			Node node = this._FindNode(key);

			// Return or throw exception
			if (node == null)
				throw new KeyNotFoundException($"Could not find value for key {key}.");
			else
				return node.Value;
		}

		/// <summary>
		/// Delete the value associated with this key. Allows deletion of non-present keys
		/// </summary>
		public bool Remove(TKey key)
		{
			// Find appropriate node
			Node node = this._FindNode(key);

			if (node != null)
			{
				this._Remove(node);
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool ContainsKey(TKey key)
		{
			return this._FindNode(key) != null;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			Node node = this._FindNode(key);

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

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			this._Root = null;
			this.Count = 0;
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			Node node = this._FindNode(item.Key);

			return node != null && node.Value.Equals(item.Value);
		}

		public bool Contains(TKey key, TValue value)
		{
			return Contains(new KeyValuePair<TKey, TValue>(key, value));
		}

		/// <summary>
		/// Copies the elements from the tree into the array, beginning at arrayIndex
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		/// <exception cref="ArgumentException">Thrown when array is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than 0</exception>
		/// <exception cref="ArgumentException">Thrown when the number of elements in the tree is greated than the available space from index to the end of the destination array</exception>
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentException("null parameter", nameof(array));

			if (this.Count > (array.Length - arrayIndex))
				throw new ArgumentException("The number of elements in the tree is greated than the available space from index to the end of the destination array", nameof(arrayIndex));

			if (arrayIndex < 0)
				throw new ArgumentException("arrayIndex less then zero", nameof(arrayIndex));

			IEnumerator<KeyValuePair<TKey, TValue>> allNodes = this.GetEnumerator();
			for (int i = 0; i < this.Count; i++)
			{
				array[i + arrayIndex] = allNodes.Current;
				if (!allNodes.MoveNext())
					break;
			}
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			Node node = this._FindNode(item.Key);

			if (node != null && node.Value.Equals(item.Value))
			{
				this._Remove(node);
				return true;
			}
			else
			{
				return false;
			}
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (Node node in this._GetSubTree(this._Root))
			{
				yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			// Lets call the generic version here
			return this.GetEnumerator();
		}
	}
}
