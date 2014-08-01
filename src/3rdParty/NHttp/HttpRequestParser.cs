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
using System.IO;
using System.Text;

namespace NHttp
{
	internal abstract class HttpRequestParser : IDisposable
	{
		protected HttpClient Client { get; private set; }
		protected int ContentLength { get; private set; }

		protected HttpRequestParser(HttpClient client, int contentLength)
		{
			if (client == null)
				throw new ArgumentNullException("client");

			Client = client;
			ContentLength = contentLength;
		}

		public abstract void Parse();

		protected void EndParsing()
		{
			// Disconnect the parser.

			Client.UnsetParser();

			// Resume processing the request.

			Client.ExecuteRequest();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
