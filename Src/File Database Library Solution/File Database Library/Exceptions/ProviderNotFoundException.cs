﻿//
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
