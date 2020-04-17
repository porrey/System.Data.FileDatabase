namespace System.Data.FileDatabase
{
	/// <summary>
	/// Represents information about an OleDb provider
	/// installed on the current system.
	/// </summary>
	public class OleDbProvider : IOleDbProvider
	{
		/// <summary>
		/// Gets the name of this provider.
		/// </summary>
		public string Name { get; internal set; }

		/// <summary>
		/// Gets the Unique Key for this provider.
		/// </summary>
		public string Key { get; internal set; }

		/// <summary>
		/// Gets the description of this provider.
		/// </summary>
		public string Description { get; internal set; }

		/// <summary>
		/// Gets the type of this provider.
		/// </summary>
		public int Type { get; internal set; }

		/// <summary>
		/// Get a value indicating if this is a parent provider.
		/// </summary>
		public bool IsParent { get; internal set; }

		/// <summary>
		/// Gets the Unique ID for this provider.
		/// </summary>
		public Guid ClsId { get; internal set; }

		/// <summary>
		/// Returns a string form of this instance.
		/// </summary>
		public override string ToString()
		{
			return $"{this.Name}, {this.Description}";
		}
	}
}
