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

using HttpMultipartParser;
using NHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SharpExpress
{
	public class Request
	{
		private HttpContext _context;

		/// <summary>
		/// All GET/POST parameters.
		/// </summary>
		internal ValueCollection Parameters { get; set; }

		/// <summary>
		/// All files sent via POST.
		/// </summary>
		public List<FormFile> Files { get; internal set; }

		/// <summary>
		/// Request method (GET, POST, etc.)
		/// </summary>
		public string Method { get; private set; }

		/// <summary>
		/// IP of the requesting client
		/// </summary>
		public string ClientIp { get { return _context.Request.RemoteEndPoint.Address.ToString(); } }

		/// <summary>
		/// Agent of the requesting client
		/// </summary>
		public string UserAgent { get { return _context.Request.UserAgent; } }

		/// <summary>
		/// Host address the client used in the request
		/// </summary>
		public string HttpHost { get { return _context.Request.LocalEndPoint.Address.ToString(); } }

		/// <summary>
		/// Port the client used in the request
		/// </summary>
		public int HttpPort { get { return _context.Request.LocalEndPoint.Port; } }

		/// <summary>
		/// New request
		/// </summary>
		/// <param name="context"></param>
		public Request(HttpContext context)
		{
			_context = context;
			this.Method = _context.Request.HttpMethod.ToUpper();

			this.Parameters = new ValueCollection();
			this.Files = new List<FormFile>();

			this.ReadQueryString(context.Request);
			this.ReadEntityBody(context.Request);
		}

		/// <summary>
		/// Reads parameters from URL (GET)
		/// </summary>
		/// <param name="request"></param>
		private void ReadQueryString(HttpRequest request)
		{
			foreach (var key in request.QueryString.AllKeys)
				this.Parameters[key] = request.Params[key];
		}

		/// <summary>
		/// Reads parameters from entity body (POST)
		/// </summary>
		/// <param name="request"></param>
		private void ReadEntityBody(HttpRequest request)
		{
			foreach (var key in request.Form.AllKeys)
				this.Parameters[key] = request.Params[key];

			foreach (var key in request.Files.AllKeys)
			{
				var file = request.Files[key];
				this.Files.Add(new FormFile(key, file.FileName, file.InputStream, file.ContentType));
			}
		}

		/// <summary>
		/// Returns cookie value or default if cookie doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public string Cookie(string name, string def = null)
		{
			var cookie = _context.Request.Cookies[name];
			if (cookie == null)
				return def;

			return cookie.Value;
		}

		/// <summary>
		/// Returns parameter value or default if parameter doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public string Parameter(string name, string def = null)
		{
			var parameter = this.Parameters[name];
			if (parameter == null)
				return def;

			return parameter;
		}
	}
}
