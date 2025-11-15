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
using System.Data.OleDb;

namespace System.Data.FileDatabase
{
    /// <summary>
    /// Defines a command interface for a database connection.
    /// </summary>
    public interface ICommand : IDisposable
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		Task<int> ExecuteNonQueryAsync(string sql, params object[] args);

		/// <summary>
		///
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		Task<object> ExecuteScalarAsync(string sql, params object[] args);

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		Task<T> ExecuteScalarAsync<T>(string sql, params object[] args);

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		Task<T> ExecuteConvertScalarAsync<T>(string sql, params object[] args);

		/// <summary>
		///
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		Task<IDataSetResult> ExecuteDataSetAsync(string sql, params object[] args);

		/// <summary>
		///
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		Task<OleDbDataReader> ExecuteReaderAsync(string sql, params object[] args);
	}
}