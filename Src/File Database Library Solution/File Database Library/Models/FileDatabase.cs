namespace System.Data.FileDatabase
{
	/// <summary>
	/// Allows direct querying of a text file.
	/// </summary>
	public class FileDatabase : JetDatabase
	{
		/// <summary>
		/// Creates an instance of FileDatabase pointing to the
		/// specified folder.
		/// </summary>
		/// <param name="folderPath">The path where the data files exist.</param>
		public FileDatabase(string folderPath)
			: base(folderPath)
		{
			this.Provider = AccessDatabase.DefaultProvider;
		}

		/// <summary>
		/// Creates an instance of FileDatabase pointing to the
		/// specified folder and by providing a flag to indicate
		/// whether or not the file contains column headers.
		/// </summary>
		/// <param name="folderPath">The path where the data files exist.</param>
		/// <param name="fileHasColumnHeaders">A boolean value indicating whether or not the file contains column headers.</param>
		public FileDatabase(string folderPath, bool fileHasColumnHeaders)
			: this(folderPath)
		{
			this.FileHasColumnHeaders = fileHasColumnHeaders;
		}

		/// <summary>
		/// Gets/sets a value indicating if the file being
		/// queried contains column headers or not.
		/// </summary>
		public bool FileHasColumnHeaders { get; set; } = false;

		/// <summary>
		/// Gets/sets the format of the file. The default value is FixLength.
		/// </summary>
		public string Format { get; set; } = "FixedLength";

		/// <summary>
		/// Initializes the connection string.
		/// </summary>
		protected override void OnInitializeConnectionString()
		{
			this.ConnectionString = String.Format($"Provider={this.Provider};Data Source=\"{this.FullPath}\";Extended Properties=\"text;HDR={0};FMT={this.Format}\"", this.FileHasColumnHeaders ? "Y" : "N");
		}
	}
}
