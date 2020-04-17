using System.Data.OleDb;
using System.Threading.Tasks;

namespace System.Data.FileDatabase
{
	/// <summary>
	/// Provides a transaction object that an be used to execute
	/// a series of statements in a single transactions. The
	/// transaction is ended when Transaction.EndTransaction()
	/// is called specify true if successful or false to roll
	/// back the transaction.
	/// </summary>
	public interface ITransaction : ICommand, IDisposable
	{
		/// <summary>
		/// Completes the transaction and specifies if it was successful or
		/// not. Passing a value if true will Commit the transaction while
		/// passing false will roll it back.
		/// </summary>
		/// <param name="success">Specifies if the transaction should be committed (true)
		/// or rolled back (false).</param>
		void EndTransaction(bool success);
	}
}