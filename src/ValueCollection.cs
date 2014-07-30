// SharpExpress - Simple express-inspired web server
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

// SharpExpress - Simple express-inspired web server
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;

namespace SharpExpress
{
	public class ValueCollection : Dictionary<string, string>
	{
		/// <summary>
		/// Returns whether the collection contains the value
		/// with the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool Has(string name)
		{
			return this.ContainsKey(name);
		}

		/// <summary>
		/// Returns the value for the specified name or the default
		/// value it the name doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public string Get(string name, string def = null)
		{
			if (this.Has(name))
				return this[name];

			return def;
		}
	}
}
