using System.Data.OleDb;

namespace System.Data.FileDatabase
{
	/// <summary>
	///
	/// </summary>
	public interface IOleDbContext : ICommand, IDisposable
	{
		/// <summary>
		///
		/// </summary>
		OleDbConnection Connection { get; set; }

		/// <summary>
		///
		/// </summary>
		string ConnectionString { get; set; }
	}
}