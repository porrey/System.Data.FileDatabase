using System.Data;

namespace System.Data.FileDatabase
{
	/// <summary>
	/// Creates an active connection to a database and leaves that
	/// connection open to explicitly calling EndBatch.
	/// </summary>
	public class BatchToken : OleDbContext
	{
		internal BatchToken(string connectionString)
			: base(connectionString, true)
		{
		}

		/// <summary>
		/// Creates a transaction object that an be used to execute
		/// a series of statements in a single transactions. The
		/// transaction is ended when Transaction.EndTransaction()
		/// is called specify true if successful or false to roll
		/// back the transaction.
		/// </summary>
		/// <param name="isolationLevel">Specifies the transaction locking behavior for the connection.</param>
		/// <returns>An instance of Transaction that can be used to execute
		/// statements in a single transaction.</returns>
		public Transaction CreateTransaction(IsolationLevel isolationLevel)
		{
			return new Transaction(this, isolationLevel);
		}

		/// <summary>
		/// Ends the batch by closing the open database connection.
		/// </summary>
		public void EndBatch()
		{
			this.Dispose();
		}
	}
}
