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
	public class HttpServerUtility
	{
		internal HttpServerUtility()
		{
		}

		public string MachineName
		{
			get { return Environment.MachineName; }
		}

		public string HtmlEncode(string value)
		{
			return HttpUtil.HtmlEncode(value);
		}

		public string HtmlDecode(string value)
		{
			return HttpUtil.HtmlDecode(value);
		}

		public string UrlEncode(string text)
		{
			return Uri.EscapeDataString(text);
		}

		public string UrlDecode(string text)
		{
			return UrlDecode(text, Encoding.UTF8);
		}

		public string UrlDecode(string text, Encoding encoding)
		{
			if (encoding == null)
				throw new ArgumentNullException("encoding");

			return HttpUtil.UriDecode(text, encoding);
		}
	}
}
