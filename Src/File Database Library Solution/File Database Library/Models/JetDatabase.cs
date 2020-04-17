using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable DF0010

namespace System.Data.FileDatabase
{
	/// <summary>
	/// Base functionality for Jet Engine based data sources.
	/// </summary>
	public abstract class JetDatabase : OleDbContext, IJetDatabase, ICachedQueries
	{
		/// <summary>
		/// Default provider strings that can be used on the Provider property.
		/// </summary>
		public static class Providers
		{
			/// <summary>
			/// Represents the Jet Database Engine 4.0
			/// </summary>
			public const string AccessEngine4 = "Microsoft.Jet.OLEDB.4.0";
			/// <summary>
			/// Represents the Jet Database Engine 12.0
			/// </summary>
			public const string AccessEngine12 = "Microsoft.ACE.OLEDB.12.0";
			/// <summary>
			/// Represents the Jet Database Engine 16.0
			/// </summary>
			public const string AccessEngine16 = "Microsoft.ACE.OLEDB.16.0";
		}

		private readonly object _lockObject = new object();
		private string _filePath = String.Empty;
		private JetEngineType _jetEngine = JetEngineType.MicrosoftJet12_x;
		private string _provider = Providers.AccessEngine12;
		private string _connectionString = String.Empty;
		private Dictionary<string, DataSet> _cachedQueries = new Dictionary<string, DataSet>();

		/// <summary>
		/// Creates an instance of a JetDatabase object
		/// with the given path.
		/// </summary>
		/// <param name="filePath"></param>
		public JetDatabase(string filePath)
			: base(null, false)
		{
			_filePath = filePath;
		}

		/// <summary>
		/// Gets/sets the full path to the data source.
		/// </summary>
		public virtual string FullPath
		{
			get
			{
				return _filePath;
			}
			set
			{
				_filePath = value;
			}
		}

		/// <summary>
		/// Checks if the data source specified in FullPath exists or not.
		/// </summary>
		public virtual bool Exists
		{
			get
			{
				return File.Exists(this.FullPath);
			}
		}

		/// <summary>
		/// Deletes the data source as specified in the FullPath.
		/// </summary>
		public virtual Task DeleteAsync()
		{
			if (this.Exists)
			{
				File.Delete(this.FullPath);
			}

			return Task.FromResult(0);
		}

		/// <summary>
		/// Gets/sets the provider string used in the connection String.
		/// </summary>
		public virtual string Provider
		{
			get
			{
				return _provider;
			}
			set
			{
				_provider = value;
			}
		}

		/// <summary>
		/// Gets/sets the Jet Engine type required when using the COM based object
		/// and methods. This is not required for standard SQL commands.
		/// </summary>
		public virtual JetEngineType JetEngine
		{
			get
			{
				return _jetEngine;
			}
			set
			{
				_jetEngine = value;
			}
		}

		/// <summary>
		/// Checks if the table exists in the underlying data structure. Table
		/// implementation may vary by provider.
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public virtual bool ContainsTable(string tableName)
		{
			bool returnValue = false;

			// ***
			// *** Check if the table exists and create it if it
			// *** doesn't
			// ***
			using (OleDbConnection conn = new OleDbConnection(this.ConnectionString))
			{
				conn.Open();

				DataTable schemaTable = conn.GetSchema("TABLES");
				conn.Close();

				EnumerableRowCollection<DataRow> qry = from tbl in schemaTable.AsEnumerable()
													   where tbl.Field<string>("TABLE_NAME") == tableName
													   select tbl;

				returnValue = (qry.Count() == 1);
			}

			return returnValue;
		}

		/// <summary>
		/// Gets/sets the specific connection string used to connect
		/// to the data source in the underlying provider.
		/// </summary>
		public override string ConnectionString
		{
			get
			{
				if (String.IsNullOrEmpty(_connectionString))
				{
					this.OnInitializeConnectionString();
				}

				return _connectionString;
			}
			set
			{
				_connectionString = value;
			}
		}

		/// <summary>
		/// Gets the cache of queries as a Dictionary with string based key and
		/// corresponding DataSet object.
		/// </summary>
		public virtual Dictionary<string, DataSet> CachedQueries
		{
			get
			{
				return _cachedQueries;
			}
		}

		/// <summary>
		/// Creates a cached query with the given key and SQL statement.
		/// </summary>
		/// <param name="key">The unique key for the query.</param>
		/// <param name="sql">A SQL statement that will generate the DataSet.</param>
		/// <returns>The number of rows affected by the query.</returns>
		public virtual async Task<int> CreateCachedQueryAsync(string key, string sql)
		{
			int returnValue = 0;

			using (IDataSetResult result = await this.ExecuteDataSetAsync(sql))
			{
				this.CachedQueries.Add(key, result.Data);
				returnValue = result.Affected;
			}

			return returnValue;
		}

		/// <summary>
		/// Gets a cached query by specifying the key.
		/// </summary>
		/// <param name="key">The unique key for the query.</param>
		/// <returns>The DataSet of the cached query.</returns>
		public virtual DataSet this[string key]
		{
			get
			{
				DataSet returnValue = null;

				if (this.CachedQueries.ContainsKey(key))
				{
					returnValue = this.CachedQueries[key];
				}

				return returnValue;
			}
		}

		/// <summary>
		/// Starts a batch token used to keep a connection to the
		/// data source open for two or more SQL statements. Note these
		/// SQL statements are run in a single transaction unless one
		/// is specifically created.
		/// </summary>
		/// <returns></returns>
		public BatchToken StartBatch()
		{
			BatchToken returnValue = new BatchToken(this.ConnectionString);

			// ***
			// *** Open a connection
			// ***
			OleDbConnection conn = new OleDbConnection(this.ConnectionString);
			conn.Open();
			returnValue.Connection = conn;

			return returnValue;
		}

		/// <summary>
		/// Called when the provider connection string is going
		/// to be initialized.
		/// </summary>
		protected virtual void OnInitializeConnectionString()
		{
			_connectionString = String.Empty;
		}

		/// <summary>
		/// Resets the connection String.
		/// </summary>
		protected virtual void ResetConnectionString()
		{
			_connectionString = String.Empty;
		}

		/// <summary>
		/// Gets all available OLE DB providers installed on the
		/// current machine. Note this list will vary when running
		/// under 32-bit versions of the OS versus 64-bit versions.
		/// </summary>
		/// <returns></returns>
		public static Task<IEnumerable<IOleDbProvider>> GetOleDbProvidersAsync()
		{
			IList<IOleDbProvider> returnValue = null;

			using (OleDbDataReader rdr = OleDbEnumerator.GetRootEnumerator())
			{
				returnValue = new List<IOleDbProvider>();

				while (rdr.Read())
				{
					OleDbProvider item = new OleDbProvider
					{
						Name = rdr.GetString(0),
						Key = rdr.GetString(1),
						Description = rdr.GetString(2),
						Type = rdr.GetInt32(3),
						IsParent = rdr.GetBoolean(4),
						ClsId = Guid.Parse(rdr.GetString(5))
					};

					returnValue.Add(item);
				};
			}

			return Task.FromResult((IEnumerable<IOleDbProvider>)returnValue);
		}
	}
}
