using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study
{
	public sealed class SplayTree<TKey, TValue> where TKey : IComparable<TKey>
	{
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

		private void _Splay(Node node)
		{
			// TODO
		}

		private Node _FindNode(TKey key)
		{
			Node node = this._Root;

			while (node != null)
			{
				int cmp = key.CompareTo(node.Key);

				if (cmp < 0)
					node = node.Left;
				else if (cmp > 0)
					node = node.Right;
				else
					return node;
			}

			//Could not find key
			return null;
		}

		private void _Replace(Node a, Node b)
		{
			if (a.Parent == null)
				this._Root = b;
			else if (a == a.Parent.Left)
				a.Parent.Left = b;
			else
				a.Parent.Right = b;

			if (b != null)
				b.Parent = a.Parent;
		}

		private Node _SubtreeMinimum(Node n)
		{
			while (n != null && n.Left != null)
				n = n.Left;

			return n;
		}

		private Node _SubtreeMaximum(Node n)
		{
			while (n != null && n.Right != null)
				n = n.Right;

			return n;
		}


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

		public TValue Find(TKey key)
		{
			Node node = this._FindNode(key);

			if (node == null)
				throw new KeyNotFoundException($"Could not find value for key {key}.");
			else
				return node.Value;
		}

		public void Delete(TKey key)
		{
			Node node = this._FindNode(key);

			if (node != null)
			{
				this._Splay(node);

				if (node.Left == null)
				{
					this._Replace(node, node.Right);
				}
				else if (node.Right == null)
				{
					this._Replace(node, node.Left);
				}
				else
				{
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
