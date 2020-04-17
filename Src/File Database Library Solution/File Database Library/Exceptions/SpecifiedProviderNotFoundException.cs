using System.Data.Common;

namespace System.Data.FileDatabase
{
	/// <summary>
	/// Thrown when the an Access Database engine provider
	/// cannot be found on the system.
	/// </summary>
	public class SpecifiedProviderNotFoundException : DbException
	{
		/// <summary>
		/// Creates default instance of ProviderNotFoundException.
		/// </summary>
		public SpecifiedProviderNotFoundException(string providerName)
			: base($"The Microsoft Database engine provider '{providerName}' could not be found on this system.")
		{
		}
	}
}
