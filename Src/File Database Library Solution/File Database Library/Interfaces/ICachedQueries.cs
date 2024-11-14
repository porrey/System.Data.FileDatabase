//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
// 
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
