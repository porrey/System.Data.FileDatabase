using System.Data.OleDb;
using System.Threading.Tasks;

#pragma warning disable DF0010
#pragma warning disable DF0021

namespace System.Data.FileDatabase
{
	/// <summary>
	/// Provides a transaction object that an be used to execute
	/// a series of statements in a single transactions. The
	/// transaction is ended when Transaction.EndTransaction()
	/// is called specify true if successful or false to roll
	/// back the transaction.
	/// </summary>
	public class Transaction : DisposableObject, ITransaction
	{
		internal Transaction(BatchToken token, IsolationLevel isolationLevel)
		{
			this.BatchToken = token;
			this.InternalTransaction = this.BatchToken.Connection.BeginTransaction(isolationLevel);
		}

		/// <summary>
		/// Gets/sets the batch token instance.
		/// </summary>
		protected BatchToken BatchToken { get; set; }

		/// <summary>
		/// Gets/sets the internal transaction instance.
		/// </summary>
		protected OleDbTransaction InternalTransaction { get; set; }

		/// <summary>
		/// Disposes managed objects.
		/// </summary>
		protected override void OnDisposeManagedObjects()
		{
			if (this.InternalTransaction != null)
			{
				if (this.InternalTransaction.Connection != null)
				{
					// ***
					// *** Force rollback
					// ***
					this.EndTransaction(false);
					this.InternalTransaction = null;
				}
			}

			if (this.BatchToken != null)
			{
				this.BatchToken = null;
			}
		}

		/// <summary>
		/// Completes the transaction and specifies if it was successful or
		/// not. Passing a value if true will Commit the transaction while
		/// passing false will roll it back.
		/// </summary>
		/// <param name="success">Specifies if the transaction should be committed (true)
		/// or rolled back (false).</param>
		public void EndTransaction(bool success)
		{
			if (success)
			{
				this.InternalTransaction.Commit();
			}
			else
			{
				this.InternalTransaction.Rollback();
			}
		}

		/// <summary>
		/// Asynchronously executes a SQL statement against a connection object.
		/// </summary>
		/// <param name="sql">Parameterized SQL command to execute.</param>
		/// <param name="args">SQL parameters passed to the SQL command.</param>
		/// <returns></returns>
		public Task<int> ExecuteNonQueryAsync(string sql, params object[] args)
		{
			int returnValue = 0;

			if (this.BatchToken != null)
			{
				lock (this.BatchToken)
				{
					using (OleDbCommand cmd = new OleDbCommand(String.Format(sql, args), this.BatchToken.Connection, this.InternalTransaction))
					{
						returnValue = cmd.ExecuteNonQuery();
					}
				}
			}
			else
			{
				throw new ObjectDisposedException(this.ToString());
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Executes the query, and returns the first column of the first row in the result
		/// set returned by the query. Additional columns or rows are ignored.
		/// </summary>
		/// <param name="sql">Parameterized SQL command to execute.</param>
		/// <param name="args">SQL parameters passed to the SQL command.</param>
		/// <returns>Returns the result as an object.</returns>
		public virtual Task<object> ExecuteScalarAsync(string sql, params object[] args)
		{
			object returnValue = null;

			if (this.BatchToken != null)
			{
				lock (this.BatchToken)
				{
					using (OleDbCommand cmd = new OleDbCommand(String.Format(sql, args), this.BatchToken.Connection, this.InternalTransaction))
					{
						returnValue = cmd.ExecuteScalar();
					}
				}
			}
			else
			{
				throw new ObjectDisposedException(this.ToString());
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Executes the query, and returns the first column of the first row in the result
		/// set returned by the query. Additional columns or rows are ignored.
		/// </summary>
		/// <param name="sql">Parameterized SQL command to execute.</param>
		/// <param name="args">SQL parameters passed to the SQL command.</param>
		/// <returns>Returns the result as type T.</returns>
		public virtual async Task<T> ExecuteScalarAsync<T>(string sql, params object[] args)
		{
			T returnValue = default;

			object result = await this.ExecuteScalarAsync(String.Format(sql, args));

			if (result != DBNull.Value && result != null)
			{
				returnValue = (T)result;
			}

			return returnValue;
		}

		/// <summary>
		/// Executes the query, and returns the first column of the first row in the result
		/// set returned by the query. Additional columns or rows are ignored.
		/// </summary>
		/// <param name="sql">Parameterized SQL command to execute.</param>
		/// <param name="args">SQL parameters passed to the SQL command.</param>
		/// <returns>Converts the return type to a value of type T.</returns>
		public virtual async Task<T> ExecuteConvertScalarAsync<T>(string sql, params object[] args)
		{
			T returnValue = default;

			object result = await this.ExecuteScalarAsync(String.Format(sql, args));

			if (result != DBNull.Value && result != null)
			{
				returnValue = (T)Convert.ChangeType(result, typeof(T));
			}

			return returnValue;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Task<OleDbDataReader> ExecuteReaderAsync(string sql, params object[] args)
		{
			OleDbDataReader returnValue = null;

			OleDbCommand cmd = new OleDbCommand(String.Format(sql, args), this.BatchToken.Connection, this.InternalTransaction);
			returnValue = cmd.ExecuteReader();

			return Task.FromResult(returnValue);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Task<IDataSetResult> ExecuteDataSetAsync(string sql, params object[] args)
		{
			IDataSetResult returnValue = new DataSetResult();

			using (OleDbCommand cmd = new OleDbCommand(String.Format(sql, args), this.BatchToken.Connection, this.InternalTransaction))
			{
				using (OleDbDataAdapter adp = new OleDbDataAdapter(cmd))
				{
					returnValue.Affected = adp.Fill(returnValue.Data);
				}
			}

			return Task.FromResult(returnValue);
		}
	}
}
