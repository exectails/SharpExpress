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
	internal class HttpUrlEncodedRequestParser : HttpRequestParser
	{
		private readonly MemoryStream _stream;

		public HttpUrlEncodedRequestParser(HttpClient client, int contentLength)
			: base(client, contentLength)
		{
			_stream = new MemoryStream();
		}

		public override void Parse()
		{
			Client.ReadBuffer.CopyToStream(_stream, ContentLength);

			if (_stream.Length == ContentLength)
			{
				ParseContent();

				EndParsing();
			}
		}

		private void ParseContent()
		{
			_stream.Position = 0;

			string content;

			using (var reader = new StreamReader(_stream, Encoding.ASCII))
			{
				content = reader.ReadToEnd();
			}

			Client.PostParameters = HttpUtil.UrlDecode(content);
		}
	}
}
