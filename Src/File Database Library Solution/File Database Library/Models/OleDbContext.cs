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
using System.Data.OleDb;

#pragma warning disable DF0010

namespace System.Data.FileDatabase
{
    /// <summary>
    ///
    /// </summary>
    public abstract class OleDbContext : DisposableObject, IOleDbContext
	{
		private OleDbConnection _connection = null;
		private readonly object _lockObject = new();

		internal OleDbContext(string connectionString, bool keepConnectionActive)
		{
			this.KeepConnectionActive = keepConnectionActive;
			this.ConnectionString = connectionString;
		}

		/// <summary>
		///
		/// </summary>
		public virtual OleDbConnection Connection
		{
			get
			{
				OleDbConnection returnValue = null;

				//
				// Check for a connection String.
				//
				if (!String.IsNullOrEmpty(this.ConnectionString))
				{
					//
					// If the KeepConnectionActive is true, then open
					// a connection to the database and leave it open.
					//
					if (this.KeepConnectionActive)
					{
						//
						// Check if the connection has been created yet.
						//
						if (_connection == null)
						{
							//
							// Create the connection.
							//
							_connection = new(this.ConnectionString);
							_connection.Open();
						}

						returnValue = _connection;
					}
					else
					{
						//
						// Create a new connection every time.
						//
						returnValue = new(this.ConnectionString);
						returnValue.Open();
					}
				}
				else
				{
					throw new ConnectionStringNotSetException();
				}

				return returnValue;
			}
			set
			{
				_connection = value;
			}
		}

		/// <summary>
		///
		/// </summary>
		protected virtual bool KeepConnectionActive { get; set; } = false;

		/// <summary>
		///
		/// </summary>
		public virtual string ConnectionString { get; set; }

		/// <summary>
		///
		/// </summary>
		protected override void OnDisposeManagedObjects()
		{
			if (_connection != null)
			{
				_connection.Close();
				_connection = null;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Task<int> ExecuteNonQueryAsync(string sql, params object[] args)
		{
			int returnValue = 0;

			lock (_lockObject)
			{
				OleDbConnection connection = this.Connection;

				try
				{
					using (OleDbCommand cmd = new(String.Format(sql, args), connection))
					{
						returnValue = cmd.ExecuteNonQuery();
					}
				}
				finally
				{
					this.ConnectionShutdownAsync(connection);
					connection = null;
				}
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Task<object> ExecuteScalarAsync(string sql, params object[] args)
		{
			object returnValue = null;

			lock (_lockObject)
			{
				OleDbConnection connection = this.Connection;

				try
				{
					using (OleDbCommand cmd = new(String.Format(sql, args), connection))
					{
						returnValue = cmd.ExecuteScalar();
					}
				}
				finally
				{
					this.ConnectionShutdownAsync(connection);
					connection = null;
				}
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
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
		///
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
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
		public virtual Task<IDataSetResult> ExecuteDataSetAsync(string sql, params object[] args)
		{
			IDataSetResult returnValue = new DataSetResult();

			lock (_lockObject)
			{
				OleDbConnection connection = this.Connection;

				try
				{
					using (OleDbDataAdapter adp = new(String.Format(sql, args), connection))
					{
						returnValue.Affected = adp.Fill(returnValue.Data);
					}
				}
				finally
				{
					this.ConnectionShutdownAsync(connection);
					connection = null;
				}
			}

			return Task.FromResult(returnValue);
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

			OleDbCommand cmd = new(String.Format(sql, args), this.Connection);
            Task.FromResult(cmd.ExecuteReader());

			return Task.FromResult(returnValue);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		protected virtual Task ConnectionShutdownAsync(OleDbConnection connection)
		{
			if (connection != null && !this.KeepConnectionActive)
			{
				connection.Close();
				connection.Dispose();
			}

			return Task.FromResult(0);
		}
	}
}
