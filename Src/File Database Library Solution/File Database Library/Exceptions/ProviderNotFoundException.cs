using System.Data.Common;

namespace System.Data.FileDatabase
{
	/// <summary>
	/// Thrown when the an Access Database engine provider
	/// cannot be found on the system.
	/// </summary>
	public class ProviderNotFoundException : DbException
	{
		/// <summary>
		/// Creates default instance of ProviderNotFoundException.
		/// </summary>
		public ProviderNotFoundException()
			: base("A Microsoft Database engine provider could not be found on this system.")
		{
		}
	}
}
