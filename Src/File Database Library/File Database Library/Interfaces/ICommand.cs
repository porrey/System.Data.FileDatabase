using System.Data.OleDb;
using System.Threading.Tasks;

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