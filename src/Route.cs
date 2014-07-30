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

using System.Text.RegularExpressions;

namespace SharpExpress
{
	public class Route
	{
		/// <summary>
		/// Method of this route (GET, POST, etc)
		/// </summary>
		public string Method { get; set; }

		/// <summary>
		/// URL pattern to match
		/// </summary>
		public Regex Pattern { get; set; }

		/// <summary>
		/// Controller function executed if pattern is matched
		/// </summary>
		public ControllerFunc Controller { get; set; }

		/// <summary>
		/// New route
		/// </summary>
		/// <param name="method"></param>
		/// <param name="pattern"></param>
		/// <param name="controller"></param>
		public Route(string method, Regex pattern, ControllerFunc controller)
		{
			this.Method = method;
			this.Pattern = pattern;
			this.Controller = controller;
		}

		/// <summary>
		/// New route
		/// </summary>
		/// <param name="method"></param>
		/// <param name="pattern"></param>
		/// <param name="controller"></param>
		public Route(string method, string pattern, ControllerFunc controller)
			: this(method, new Regex("^" + pattern + "$", RegexOptions.Compiled), controller)
		{
		}
	}

	public delegate void ControllerFunc(Request req, Response res);
}
