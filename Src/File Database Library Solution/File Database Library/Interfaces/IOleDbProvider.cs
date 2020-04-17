namespace System.Data.FileDatabase
{
	/// <summary>
	/// Represents information about an OleDb provider
	/// installed on the current system.
	/// </summary>
	public interface IOleDbProvider
	{
		/// <summary>
		/// Gets the name of this provider.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the Unique Key for this provider.
		/// </summary>
		string Key { get; }

		/// <summary>
		/// Gets the description of this provider.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Gets the type of this provider.
		/// </summary>
		int Type { get; }

		/// <summary>
		/// Get a value indicating if this is a parent provider.
		/// </summary>
		bool IsParent { get; }

		/// <summary>
		/// Gets the Unique ID for this provider.
		/// </summary>
		Guid ClsId { get; }
	}
}
