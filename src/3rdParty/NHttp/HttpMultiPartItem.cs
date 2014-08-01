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

using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace NHttp
{
	internal class HttpMultiPartItem
	{
		public HttpMultiPartItem(Dictionary<string, string> headers, string value, Stream stream)
		{
			if (headers == null)
				throw new ArgumentNullException("headers");

			Headers = headers;
			Value = value;
			Stream = stream;
		}

		public Dictionary<string, string> Headers { get; private set; }

		public string Value { get; private set; }

		public Stream Stream { get; private set; }
	}
}
