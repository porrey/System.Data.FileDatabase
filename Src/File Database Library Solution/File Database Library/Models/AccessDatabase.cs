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
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Data.FileDatabase
{
    /// <summary>
    /// Provides functionality against an AccessDatabase.
    /// </summary>
    public class AccessDatabase : JetDatabase
	{
		/// <summary>
		/// Creates an instance of AccessDatabase with the given
		/// path.
		/// </summary>
		/// <param name="accessDbPath"></param>
		public AccessDatabase(string accessDbPath)
			: base(accessDbPath)
		{
			this.Provider = AccessDatabase.DefaultProvider;
		}

		/// <summary>
		/// Creates an instance of AccessDatabase with the given
		/// path.
		/// </summary>
		/// <param name="accessDbPath"></param>
		/// <param name="provider"></param>
		public AccessDatabase(string accessDbPath, string provider)
			: base(accessDbPath)
		{
			this.Provider = provider;
		}

		/// <summary>
		/// Gets sets the default provider for all connections
		/// unless overridden in a specific instance.
		/// </summary>
		public static string DefaultProvider
		{
			get
			{
				return AccessDatabase.GetProviderName();
			}
			set
			{
				MicrosoftAccessProvider.SetProviderByNameAsync(value).Wait();
			}
		}

		/// <summary>
		/// Override the initializer.
		/// </summary>
		protected override void OnInitializeConnectionString()
		{
			this.ConnectionString = String.Format("Provider={0};Data Source=\"{1}\";Persist Security Info=True", this.Provider, this.FullPath);
		}

		/// <summary>
		/// Gets/sets the COM class name when using COM based features.
		/// </summary>
		public string JetReplicationClass { get; set; } = "ADOX.Catalog.6.0";

		/// <summary>
		/// Creates an empty Access Database. Requires COM objects.
		/// </summary>
		/// <returns>Returns true if successful; false otherwise.</returns>
		public Task<bool> CreateEmptyAccessDatabaseAsync()
		{
			bool returnValue = false;

			//
			// Create an instance of a Jet Replication Object
			//
			Type objectType = Type.GetTypeFromProgID(this.JetReplicationClass);

			if (objectType != null)
			{
				object comObject = Activator.CreateInstance(objectType);

				string connectionString = String.Format("Provider={0}; Data Source={1}; Jet OLEDB:Engine Type={2}", this.Provider, this.FullPath, ((int)this.JetEngine).ToString());

                //
                // Create tan object array for the parameters
                //
                object[] oParams =
                [
                    connectionString
				];

				//
				// Create the Access Database by calling the Create method on the COM object.
				//
				_ = comObject.GetType().InvokeMember("Create", BindingFlags.InvokeMethod, null, comObject, oParams);

				//
				// Clean up the COM object.
				//
				Marshal.ReleaseComObject(comObject);

				returnValue = File.Exists(this.FullPath);
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Compact and repairs the Access database. Requires COM objects.
		/// </summary>
		/// <returns>Returns true if successful; false otherwise.</returns>
		public Task<bool> CompactAndRepairAsync()
		{
			bool returnValue = false;

			try
			{
				//
				// ADOX.Catalog
				//
				string sourceConnectionString = String.Format("Provider={0}; Data Source={1}; Jet OLEDB:Engine Type={2}", this.Provider, this.FullPath, Convert.ToInt32(this.JetEngine));
				string tempFile = String.Format("{0}.tmp", this.FullPath);
				string destinationConnectionString = String.Format("Provider={0}; Data Source={1}; Jet OLEDB:Engine Type={2}", this.Provider, tempFile, Convert.ToInt32(this.JetEngine));

				//
				// Create an instance of a Jet Replication Object
				//
				Type objectType = Type.GetTypeFromProgID("JRO.JetEngine");

				if (objectType != null)
				{
					object comObject = Activator.CreateInstance(objectType);

                    //
                    // Create tan object array for the parameters
                    //
                    object[] oParams =
                    [
                        sourceConnectionString,
						destinationConnectionString
					];

					object result = comObject.GetType().InvokeMember("CompactDatabase", BindingFlags.InvokeMethod, null, comObject, oParams);

					//
					// Clean up
					//
					Marshal.ReleaseComObject(comObject);
					comObject = null;

					//
					// Copy the temp file to the original file
					//
					if (File.Exists(tempFile))
					{
						File.Delete(this.FullPath);
						File.Move(tempFile, this.FullPath);
						returnValue = File.Exists(this.FullPath);
					}

					returnValue = File.Exists(this.FullPath);
				}
				else
				{
					throw new Exception("Could not find 'JRO.JetEngine'.");
				}
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException != null)
				{
					throw ex.InnerException;
				}
				else
				{
					throw;
				}
			}

			return Task.FromResult(returnValue);
		}

		private static string GetProviderName()
		{
			string returnValue = String.Empty;

			if (MicrosoftAccessProvider.ProviderFound)
			{
				returnValue = MicrosoftAccessProvider.Current.Name;
			}
			else
			{
				MicrosoftAccessProvider.CheckForAccessProviderAsync().Wait();

				if (MicrosoftAccessProvider.ProviderFound)
				{
					returnValue = MicrosoftAccessProvider.Current.Name;
				}
				else
				{
					throw new ProviderNotFoundException();
				}
			}

			return returnValue;
		}
	}
}
