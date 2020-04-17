using System.Data.Common;

namespace System.Data.FileDatabase
{
	/// <summary>
	/// Thrown when a connection attempt is made but the connection
	/// string has not been set.
	/// </summary>
	public class ConnectionStringNotSetException : DbException
	{
		/// <summary>
		/// Creates default instance of ConnectionStringNotSetException.
		/// </summary>
		public ConnectionStringNotSetException()
			: base("The database connection string has not been set.")
		{
		}
	}
}
