using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
	public class Trie
	{
		public static string ALPHABET = "abcdefghijklmnopqrstuvwxyz";

		private class Node
		{
			public char Value;

			public Node[] Children;

			public Node(char value)
			{
				Value = value;
				Children = new Node[ALPHABET.Length];
			}
		}

		private Dictionary<int, char> _CharacterByIndex = new Dictionary<int, char>();
		private Dictionary<char, int> _IndexByCharacter = new Dictionary<char, int>();

		private Node _Root { get; set; }

		public int Count { get; set; }

		public Trie()
		{
			for (int i = 0; i < ALPHABET.Length; i++)
			{
				_CharacterByIndex[i] = ALPHABET[i];
				_IndexByCharacter[ALPHABET[i]] = i;
			}
		}

		public void Add(string value)
		{
			Node node = _Root;
			int i = 0;

			// Match as much of the string as we can
			for (; i < value.Length; i++)
			{
				if (node.Children[value[i]] != null)
					node = node.Children[value[i]];
				else
					break;
			}

			// Append new nodes, if necessary
			for (; i < value.Length; i++)
			{
				node.Children[value[i]] = new Node(value[i]);
				node = node.Children[value[i]];
			}
		}

		public void Remove(string value)
		{

		}

		public bool Contains(string value)
		{
			Node node = _Root;

			foreach (char c in value)
			{
				node = node.Children[_IndexByCharacter[c]];
				if (node == null)
					break;
			}

			if (node != null)
				return true;
			else
				return false;
		}

		public void Clear()
		{
			this._Root = null;
		}
	}
}
