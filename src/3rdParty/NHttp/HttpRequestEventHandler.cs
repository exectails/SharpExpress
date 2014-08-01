// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library.

using System;
using System.Collections.Generic;
using System.Text;

namespace NHttp
{
	public class HttpRequestEventArgs : EventArgs
	{
		public HttpRequestEventArgs(HttpContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			Context = context;
		}

		public HttpContext Context { get; private set; }

		public HttpServerUtility Server { get { return Context.Server; } }

		public HttpRequest Request { get { return Context.Request; } }

		public HttpResponse Response { get { return Context.Response; } }
	}

	public delegate void HttpRequestEventHandler(object sender, HttpRequestEventArgs e);
}
