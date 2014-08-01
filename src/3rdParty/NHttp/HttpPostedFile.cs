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
	public class HttpPostedFile
	{
		public HttpPostedFile(int contentLength, string contentType, string fileName, Stream inputStream)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");
			if (inputStream == null)
				throw new ArgumentNullException("inputStream");

			ContentLength = contentLength;
			ContentType = contentType;
			FileName = fileName;
			InputStream = inputStream;
		}

		public int ContentLength { get; private set; }

		public string ContentType { get; private set; }

		public string FileName { get; private set; }

		public Stream InputStream { get; private set; }
	}
}
