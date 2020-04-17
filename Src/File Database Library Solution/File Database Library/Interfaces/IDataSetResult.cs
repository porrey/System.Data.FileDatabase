namespace System.Data.FileDatabase
{
	/// <summary>
	/// Contains the result of a DataSet query.
	/// </summary>
	public interface IDataSetResult : IDisposable
	{
		/// <summary>
		/// Gets the number of records returned by the query.
		/// </summary>
		int Affected { get; set; }

		/// <summary>
		/// Gets the resulting DataSet from the query.
		/// </summary>
		DataSet Data { get; set; }

		/// <summary>
		/// De-constructs the object for use with Tuples.
		/// </summary>
		/// <param name="affected">Gets the number of records returned by the query.</param>
		/// <param name="data">Gets the resulting DataSet from the query.</param>
		void Deconstruct(out int affected, out DataSet data);
	}
}
