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

using Mustache;
using System.Collections.Generic;
using System.IO;

namespace SharpExpress.Engines
{
	public class HandlebarsEngine : IEngine
	{
		protected FormatCompiler _compiler;
		protected Dictionary<string, Generator> _cache;

		public HandlebarsEngine()
		{
			_compiler = new FormatCompiler();
			_cache = new Dictionary<string, Generator>();

			_compiler.RemoveNewLines = false;
		}

		public string RenderString(string template, object args = null)
		{
			var generator = this.GetGenerator(template);
			var result = "";

			try
			{
				result = generator.Render(args);
			}
			catch (KeyNotFoundException ex)
			{
				result = "Error: " + ex.Message;
			}

			return result;
		}

		/// <summary>
		/// Returns generator from cache or creates and caches it.
		/// </summary>
		/// <param name="template"></param>
		/// <returns></returns>
		protected Generator GetGenerator(string template)
		{
			if (_cache.ContainsKey(template))
				return _cache[template];

			var generator = _compiler.Compile(template);

			return (_cache[template] = generator);
		}
	}
}
