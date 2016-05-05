using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study
{
	/// <summary>
	/// A self-adjusting binary search tree with the additional property that recently accessed elements are quick to access again.
	/// </summary>
	public sealed class SplayTree<TKey, TValue> where TKey : IComparable<TKey>
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

		#endregion

		/// <summary>
		/// Insert new value
		/// </summary>
		public void Insert(TKey key, TValue value)
		{
			// Get the parent of the position we should insert
			Node node = this._Root;
			Node parent = null;

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
					node.Value = value;
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
			}

			this._Splay(node);
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
		public void Delete(TKey key)
		{
			// Find appropriate node
			Node node = this._FindNode(key);

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
	}
}
