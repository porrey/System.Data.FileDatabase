using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace System.Data.FileDatabase
{
	/// <summary>
	/// Defines an interface for an object that can hold
	/// cached DataSet queries.
	/// </summary>
	public interface ICachedQueries
	{
		/// <summary>
		/// Gets the cache of queries as a Dictionary with string based key and
		/// corresponding DataSet object.
		/// </summary>
		Dictionary<string, DataSet> CachedQueries { get; }

		/// <summary>
		/// Creates a cached query with the given key and SQL statement.
		/// </summary>
		/// <param name="key">The unique key for the query.</param>
		/// <param name="sql">A SQL statement that will generate the DataSet.</param>
		/// <returns>The number of rows affected by the query.</returns>
		Task<int> CreateCachedQueryAsync(string key, string sql);

		/// <summary>
		/// Gets a cached query by specifying the key.
		/// </summary>
		/// <param name="key">The unique key for the query.</param>
		/// <returns>The DataSet of the cached query.</returns>
		DataSet this[string key] { get; }
	}
}
