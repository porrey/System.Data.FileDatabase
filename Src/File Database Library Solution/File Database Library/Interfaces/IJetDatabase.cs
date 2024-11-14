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
namespace System.Data.FileDatabase
{
    /// <summary>
    /// Base functionality for Jet Engine based data sources.
    /// </summary>
    public interface IJetDatabase : ICommand
	{
		/// <summary>
		/// Checks if the data source specified in FullPath exists or not.
		/// </summary>
		bool Exists { get; }

		/// <summary>
		/// Gets/sets the full path to the data source.
		/// </summary>
		string FullPath { get; set; }

		/// <summary>
		/// Gets/sets the Jet Engine type required when using the COM based object
		/// and methods. This is not required for standard SQL commands.
		/// </summary>
		JetEngineType JetEngine { get; set; }

		/// <summary>
		/// Gets/sets the provider string used in the connection String.
		/// </summary>
		string Provider { get; set; }

		/// <summary>
		/// Gets/sets the specific connection string used to connect
		/// to the data source in the underlying provider.
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// Checks if the table exists in the underlying data structure. Table
		/// implementation may vary by provider.
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		bool ContainsTable(string tableName);

		/// <summary>
		/// Deletes the data source as specified in the FullPath.
		/// </summary>
		Task<bool> DeleteAsync();

        /// <summary>
        /// Deletes the data source as specified in the FullPath.
        /// </summary>
        bool Delete();
    }
}
