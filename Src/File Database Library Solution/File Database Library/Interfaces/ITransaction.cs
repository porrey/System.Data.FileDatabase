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

		/// <summary>
		/// Completes the transaction and specifies if it was successful or
		/// not. Passing a value if true will Commit the transaction while
		/// passing false will roll it back.
		/// </summary>
		/// <param name="success">Specifies if the transaction should be committed (true)
		/// or rolled back (false).</param>
		Task EndTransactionAsync(bool success);

    }
}