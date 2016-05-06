using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study
{
	public class SuffixTree
	{
		private class Node
		{
			public Edge Parent { get; set; }
			public List<Edge> Children { get; set; }

			/// <summary>
			/// Only used for leaf nodes
			/// </summary>
			public int SubstringIndex { get; set; }
		}

		private class Edge
		{
			public Node From { get; set; }
			public Node To { get; set; }

			public int TextStart { get; set; }

			/// <summary>
			/// -1 means "current end"
			/// </summary>
			public int TextEnd { get; set; }
		}

		private Node _Root { get; set; }

		public string Text { get; set; }

		public SuffixTree(string text)
		{
			// Ukkonen's Algorithm
			// TODO
		}

		public bool Contains(string substring)
		{
			return false;
		}

		public int FirstIndexOf(string substring)
		{
			return -1;
		}
	}
}
