using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
	public class Trie
	{
		private class Node
		{
			public char C { get; set; }
			public Dictionary<char, Node> Children { get; set; }
			public Node Parent { get; set; }
			public bool IsTerminal { get; set; }

			public Node(Node parent, char c)
			{
				Parent = parent;
				C = c;
				Children = new Dictionary<char, Node>();
				IsTerminal = false;
			}

			public string Word
			{
				get
				{
					StringBuilder sb = new StringBuilder();
					sb.Insert(0, C.ToString(CultureInfo.InvariantCulture));

					Node node = Parent;
					while (node != null)
					{
						sb.Insert(0, node.C.ToString(CultureInfo.InvariantCulture));
						node = node.Parent;
					}

					return sb.ToString();
				}
			}
		}
		
		private Node _Root { get; set; }

		private bool _CaseSensitive { get; set; }

		public int Count { get; set; }

		public Trie(bool caseSensitive = false)
		{
			_CaseSensitive = caseSensitive;
		}

		public Trie(IEnumerable<string> words, bool caseSensitive = false)
			: this(caseSensitive)
		{
			foreach (string word in words)
			{
				Add(word);
			}
		}

		private Node _FindNode(string word)
		{
			word = NormalizeWord(word);

			word = NormalizeWord(word);

			if (String.IsNullOrWhiteSpace(word))
				return null;

			Node node = _Root;

			foreach (char c in word)
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

		private string NormalizeWord(string word)
		{
			if (String.IsNullOrWhiteSpace(word))
				word = String.Empty;

			word = word.Trim();

			if (!_CaseSensitive)
				word = word.ToLowerInvariant();

			return word;
		}

		public void Add(string word)
		{
			word = NormalizeWord(word);

			Node node = _Root;

			foreach (char c in word)
			{
				if (!node.Children.ContainsKey(c))
					node.Children.Add(c, new Node(node, c));

				node = node.Children[c];
			}

			node.IsTerminal = true;
		}

		public void Remove(string word)
		{
			Node node = _FindNode(word);

			if (node == null)
				return;

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
				}
			}
		}

		public bool Contains(string word)
		{
			Node node = _FindNode(word);

			return node != null;
		}

		public void Clear()
		{
			this._Root = null;
		}
	}
}
