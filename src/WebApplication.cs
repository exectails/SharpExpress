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
using SharpExpress.Engines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpExpress
{
	public class WebApplication
	{
		private const string DefaultEngine = "_default";
		private const int DefaultPort = 80;

		private HttpServer _server;
		private List<Route> _routes;
		private Dictionary<string, IEngine> _engines;
		private HashSet<string> _statics;

		/// <summary>
		/// Amount of requests handled
		/// </summary>
		public int HandledRequests { get; protected set; }

		/// <summary>
		/// New Web Application
		/// </summary>
		public WebApplication()
		{
			_server = new HttpServer();

			_routes = new List<Route>();

			_engines = new Dictionary<string, IEngine>();
			_engines.Add(DefaultEngine, new HtmlEngine());
			_statics = new HashSet<string>();
		}

		/// <summary>
		/// Starts listening for requests
		/// </summary>
		/// <param name="port"></param>
		public void Listen(int port = DefaultPort)
		{
			_server.EndPoint = new IPEndPoint(IPAddress.Any, port);

			_server.RequestReceived += this.OnRequest;

			_server.Start();
		}

		/// <summary>
		/// Stops listening
		/// </summary>
		public void Stop()
		{
			_server.Stop();
		}

		/// <summary>
		/// Handles new requests
		/// </summary>
		/// <param name="state"></param>
		protected void OnRequest(object state, HttpRequestEventArgs e)
		{
			var context = e.Context;

			//Console.WriteLine("{0,-21} {1,-23} {2} {3}", DateTime.Now, context.Request.RemoteEndPoint, context.Request.HttpMethod, context.Request.RawUrl);

			this.HandledRequests++;

			try
			{
				var rawUrl = context.Request.RawUrl.Replace('\\', '/').TrimEnd('/');

				// Get rid of the GET query
				var questionMarkIndex = rawUrl.IndexOf('?');
				if (questionMarkIndex > -1)
					rawUrl = rawUrl.Substring(0, questionMarkIndex);

				var req = new Request(context);
				var res = new Response(this, context);

				// Static
				if (this.TryStatic(rawUrl, context, res))
					return;

				// Routes
				this.Route(rawUrl, context, req, res);

				res.Send();
			}
			catch (Exception ex)
			{
				// log
			}
		}

		#region Routes

		public void On(string[] methods, string pattern, ControllerFunc controller)
		{
			pattern = Regex.Replace(pattern, @":(\w+)", @"(?<$1>\w+)");
			pattern = pattern.Replace("*", ".*");
			pattern = pattern.TrimEnd('/');

			foreach (var method in methods)
				_routes.Add(new Route(method.ToUpper(), pattern, controller));
		}

		public void On(string[] methods, string pattern, IController controller) { this.On(methods, pattern, controller.Index); }

		public void Get(string pattern, ControllerFunc controller) { this.On(new string[] { "get" }, pattern, controller); }
		public void Get(string pattern, IController controller) { this.On(new string[] { "get" }, pattern, controller); }

		public void Post(string pattern, ControllerFunc controller) { this.On(new string[] { "post" }, pattern, controller); }
		public void Post(string pattern, IController controller) { this.On(new string[] { "post" }, pattern, controller); }

		public void All(string pattern, ControllerFunc controller) { this.On(new string[] { "get", "post" }, pattern, controller); }
		public void All(string pattern, IController controller) { this.On(new string[] { "get", "post" }, pattern, controller); }

		private void Route(string rawUrl, HttpContext context, Request req, Response res)
		{
			var handled = false;

			foreach (var route in _routes.Where(a => string.Compare(a.Method, req.Method, true) == 0))
			{
				var match = route.Pattern.Match(rawUrl);
				if (!match.Success) continue;

				res.ContentType = MediaTypeNames.Text.Html;
				res.StatusCode = HttpStatusCode.OK;
				res.StatusDescription = "OK";

				// Match parameters
				foreach (string key in route.Pattern.GetGroupNames().Where(a => a != "0"))
					req.Parameters[key] = match.Groups[key].Value;

				route.Controller(req, res);

				handled = true;

				break;
			}

			// Fallback
			if (!handled)
			{
				res.ContentType = MediaTypeNames.Text.Plain;
				res.StatusCode = HttpStatusCode.OK;
				res.StatusDescription = "OK";

				res.Send("Cannot {0} {1}", context.Request.HttpMethod, context.Request.RawUrl);
			}
		}

		#endregion Routes

		#region Engines

		public IEngine GetEngine(string type)
		{
			if (_engines.ContainsKey(type))
				return _engines[type];

			return _engines[DefaultEngine];
		}

		public void Engine(string type, IEngine engine)
		{
			_engines[type] = engine;
		}

		#endregion Engines

		#region Statics

		/// <summary>
		/// Adds static paths
		/// </summary>
		/// <param name="paths"></param>
		public void Static(params string[] paths)
		{
			foreach (var path in paths)
				_statics.Add(path.Replace('\\', '/').Trim('/'));
		}

		/// <summary>
		/// Tries to handle the request as a static file. Returns true if successful.
		/// </summary>
		/// <param name="rawUrl"></param>
		/// <param name="context"></param>
		/// <param name="res"></param>
		/// <returns></returns>
		private bool TryStatic(string rawUrl, HttpContext context, Response res)
		{
			var relativeFilePath = rawUrl.TrimStart('/');
			var fullFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, relativeFilePath)).Replace('\\', '/');

			foreach (var s in _statics)
			{
				// Is in sub folder of static folder?
				var staticFolderPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, s)).Replace('\\', '/') + '/';
				if (!fullFilePath.StartsWith(staticFolderPath))
					continue;

				res.SendFile(fullFilePath);
				return true;
			}

			return false;
		}

		#endregion Statics
	}
}
