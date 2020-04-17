using System.Threading.Tasks;

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
		Task DeleteAsync();
	}
}
