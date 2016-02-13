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

using NHttp;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace SharpExpress
{
	public class Response
	{
		private WebApplication _app;
		private HttpContext _context;

		public StringBuilder StringBuffer { get; private set; }

		/// <summary>
		/// Set to true once something was sent.
		/// </summary>
		public bool Sent { get; private set; }

		/// <summary>
		/// Content type of the response
		/// </summary>
		public string ContentType
		{
			get { return _context.Response.ContentType; }
			set { _context.Response.ContentType = value; }
		}

		/// <summary>
		/// Status code for the response
		/// </summary>
		public HttpStatusCode StatusCode
		{
			get { return (HttpStatusCode)_context.Response.StatusCode; }
			set
			{
				_context.Response.StatusCode = (int)value;
				_context.Response.StatusDescription = System.Web.HttpWorkerRequest.GetStatusDescription(_context.Response.StatusCode);
			}
		}

		/// <summary>
		/// Status description for the response
		/// </summary>
		public string StatusDescription
		{
			get { return _context.Response.StatusDescription; }
			set { _context.Response.StatusDescription = value; }
		}

		/// <summary>
		/// Output stream for the response
		/// </summary>
		public Stream OutputStream { get { return _context.Response.OutputStream; } }

		/// <summary>
		/// New response
		/// </summary>
		/// <param name="context"></param>
		/// <param name="writer"></param>
		public Response(WebApplication app, HttpContext context)
		{
			_app = app;
			_context = context;

			this.StringBuffer = new StringBuilder();
		}

		/// <summary>
		/// Sends text to browser
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Send(string format, params object[] args)
		{
			this.Send(string.Format(format, args));
		}

		/// <summary>
		/// Sends text to browser
		/// </summary>
		/// <param name="data"></param>
		public void Send(string data)
		{
			if (!this.Sent) this.Sent = true;

			this.StringBuffer.Append(data);
		}

		/// <summary>
		/// Redirects request to url.
		/// </summary>
		/// <param name="url"></param>
		public void Redirect(string url)
		{
			_context.Response.Redirect(url);
		}

		/// <summary>
		/// Renders template and sends result to browser.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="args"></param>
		public void Render(string filePath, object args = null)
		{
			if (!File.Exists(filePath))
			{
				this.Send("Missing template '{0}'.", filePath);
				return;
			}

			var ext = Path.GetExtension(filePath).Trim('.');
			var engine = _app.GetEngine(ext);

			string file = "";
			using (var sr = new StreamReader(filePath))
				file = sr.ReadToEnd();

			var result = engine.RenderString(file, args);

			this.Send(result);
		}

		/// <summary>
		/// Sets cookie
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="path"></param>
		/// <param name="expires"></param>
		public void Cookie(string name, string value, string path, DateTime? expires)
		{
			var cookie = new HttpCookie(name, value);

			if (path != "")
				cookie.Path = path;

			if (expires != null)
				cookie.Expires = expires.Value.ToUniversalTime();

			_context.Response.Cookies.Add(cookie);
		}

		/// <summary>
		/// Sets cookie
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Cookie(string name, string value)
		{
			this.Cookie(name, value, "", null);
		}

		/// <summary>
		/// Sets cookie
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="expires"></param>
		public void Cookie(string name, string value, DateTime? expires)
		{
			this.Cookie(name, value, "", expires);
		}

		/// <summary>
		/// Sends string buffer to client
		/// </summary>
		internal void Send()
		{
			using (var output = _context.Response.OutputStream)
			{
				var buffer = Encoding.UTF8.GetBytes(this.StringBuffer.ToString());
				output.Write(buffer, 0, buffer.Length);
			}
		}

		/// <summary>
		/// Sends file or 404 if file doesn't exist.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="res"></param>
		public void SendFile(string filePath)
		{
			// 404
			if (!File.Exists(filePath))
			{
				this.StatusCode = HttpStatusCode.NotFound;
				this.Send("NotFound");
				return;
			}

			// Set content type based on file extension
			switch (Path.GetExtension(filePath))
			{
				case ".jpg":
				case ".jpeg": this.ContentType = "image/jpeg"; break;
				case ".gif": this.ContentType = "image/gif"; break;
				case ".png": this.ContentType = "image/png"; break;
				case ".ico": this.ContentType = "image/x-icon"; break;

				case ".htm":
				case ".html": this.ContentType = "text/html"; break;
				case ".js": this.ContentType = "application/javascript"; break;
				case ".json": this.ContentType = "application/json"; break;
				case ".css": this.ContentType = "text/css"; break;

				case ".xml": this.ContentType = "text/xml"; break;
				case ".txt": this.ContentType = "text/plain"; break;

				default:
					this.ContentType = "application/octet-stream";
					break;
			}

			// Send file
			using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			using (var output = _context.Response.OutputStream)
			{
				fs.CopyTo(output);
			}
		}

		/// <summary>
		/// Sets control-cache header.
		/// </summary>
		/// <param name="val"></param>
		/// <example>
		/// SetControlCache("public, max-age=20");
		/// </example>
		public void SetControlCache(string val)
		{
			_context.Response.CacheControl = val;
		}
	}
}
