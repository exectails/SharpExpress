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
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace SharpExpress
{
	public class Request
	{
		private HttpListenerContext _context;

		/// <summary>
		/// All GET/POST parameters.
		/// </summary>
		public ValueCollection Parameters { get; internal set; }

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
		public Request(HttpListenerContext context)
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
		/// <param name="listenerRequest"></param>
		private void ReadQueryString(HttpListenerRequest listenerRequest)
		{
			foreach (var x in listenerRequest.QueryString.AllKeys)
			{
				var key = HttpUtility.UrlDecode(x);
				var value = HttpUtility.UrlDecode(listenerRequest.QueryString[x] ?? "");

				if (key == null)
				{
					key = value;
					value = "";
				}

				this.Parameters[key] = value;
			}
		}

		/// <summary>
		/// Reads parameters from entity body (POST)
		/// </summary>
		/// <param name="listenerRequest"></param>
		private void ReadEntityBody(HttpListenerRequest listenerRequest)
		{
			if (!listenerRequest.HasEntityBody)
				return;

			if (listenerRequest.ContentType.ToLower() == "application/x-www-form-urlencoded")
			{
				// Query string from stream
				using (var reader = new StreamReader(listenerRequest.InputStream, listenerRequest.ContentEncoding))
				{
					var query = reader.ReadToEnd();
					var kvs = query.Split('&');
					for (int i = 0; i < kvs.Length; ++i)
					{
						var kv = kvs[i].Split('=');
						this.Parameters[HttpUtility.UrlDecode(kv[0])] = HttpUtility.UrlDecode(kv[1]);
					}
				}
			}
			else if (listenerRequest.ContentType.ToLower().StartsWith("multipart/form-data;"))
			{
				var parser = new MultipartFormDataParser(listenerRequest.InputStream);

				// POST
				foreach (var kv in parser.Parameters)
					this.Parameters[kv.Key] = kv.Value.Data;

				// POST / Files
				foreach (var file in parser.Files)
					this.Files.Add(new FormFile(file.Name, file.FileName, file.Data, file.ContentType));
			}
			else
			{
				throw new Exception("Unknown content type '" + listenerRequest.ContentType + "'.");
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
	}
}
