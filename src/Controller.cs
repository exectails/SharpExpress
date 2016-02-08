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

using System;
using System.IO;
using System.Net;

namespace SharpExpress
{
	public interface IController
	{
		void Index(Request req, Response res);
	}

	public class StaticController : IController
	{
		public virtual void Index(Request req, Response res)
		{
			var relativeFilePath = req.Path.TrimStart('/');
			var fullFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, relativeFilePath)).Replace('\\', '/');

			res.SendFile(fullFilePath);
		}
	}
}
