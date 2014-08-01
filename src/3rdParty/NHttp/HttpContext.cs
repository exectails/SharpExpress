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
	public class HttpContext
	{
		internal HttpContext(HttpClient client)
		{
			Server = client.Server.ServerUtility;
			Request = new HttpRequest(client);
			Response = new HttpResponse(this);
		}

		public HttpServerUtility Server { get; private set; }

		public HttpRequest Request { get; private set; }

		public HttpResponse Response { get; private set; }
	}
}
