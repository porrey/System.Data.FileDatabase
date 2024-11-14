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
	/// Contains the result of a DataSet query.
	/// </summary>
	public class DataSetResult : DisposableObject, IDataSetResult
	{
		/// <summary>
		/// Creates a default instance of DataSetResult.
		/// </summary>
		public DataSetResult()
		{
			this.Data = new DataSet();
		}

		/// <summary>
		/// De-constructs the object for use with Tuples.
		/// </summary>
		/// <param name="affected">Gets the number of records returned by the query.</param>
		/// <param name="data">Gets the resulting DataSet from the query.</param>
		public void Deconstruct(out int affected, out DataSet data)
		{
			affected = this.Affected;
			data = this.Data;
		}

		/// <summary>
		/// Gets the number of records returned by the query.
		/// </summary>
		public int Affected { get; set; }

		/// <summary>
		/// Gets the resulting DataSet from the query.
		/// </summary>
		public DataSet Data { get; set; }

		/// <summary>
		/// Disposes managed objects.
		/// </summary>
		protected override void OnDisposeManagedObjects()
		{
			if (this.Data != null)
			{
				this.Data.Dispose();
				this.Data = null;
				this.Affected = 0;
			}

			base.OnDisposeManagedObjects();
		}
	}
}
