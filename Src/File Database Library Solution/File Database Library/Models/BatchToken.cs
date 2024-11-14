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
        /// Creates a transaction object that an be used to execute
        /// a series of statements in a single transactions. The
        /// transaction is ended when Transaction.EndTransaction()
        /// is called specify true if successful or false to roll
        /// back the transaction.
        /// </summary>
        /// <param name="isolationLevel">Specifies the transaction locking behavior for the connection.</param>
        /// <returns>An instance of Transaction that can be used to execute
        /// statements in a single transaction.</returns>
        public Task<Transaction> CreateTransactionAsync(IsolationLevel isolationLevel)
        {
            return Task.FromResult(new Transaction(this, isolationLevel));
        }

        /// <summary>
        /// Ends the batch by closing the open database connection.
        /// </summary>
        public void EndBatch()
		{
			this.Dispose();
		}

        /// <summary>
        /// Ends the batch by closing the open database connection.
        /// </summary>
        public Task EndBatchAsync()
        {
            this.Dispose();
			return Task.CompletedTask;
        }
    }
}
