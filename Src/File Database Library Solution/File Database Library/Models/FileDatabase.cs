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
