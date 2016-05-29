using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Tries
{
	/// <summary>
	/// Inspired by Trie implementations found at https://trienet.codeplex.com/license
	/// </summary>
	public interface ITrie<TValue>
	{
		IEnumerable<TValue> Retrieve(string query);
		void Add(string key, TValue value);
		void Add(KeyValuePair<string, TValue> item);
	}
}
