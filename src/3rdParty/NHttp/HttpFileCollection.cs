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
using System.Collections.Specialized;

namespace NHttp
{
	public class HttpFileCollection : NameObjectCollectionBase
	{
		private string[] _allKeys;

		internal HttpFileCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		public HttpPostedFile Get(string name)
		{
			return (HttpPostedFile)BaseGet(name);
		}

		public HttpPostedFile this[string name]
		{
			get { return Get(name); }
		}

		public HttpPostedFile Get(int index)
		{
			return (HttpPostedFile)BaseGet(index);
		}

		public string GetKey(int index)
		{
			return BaseGetKey(index);
		}

		public HttpPostedFile this[int index]
		{
			get { return Get(index); }
		}

		public string[] AllKeys
		{
			get
			{
				if (_allKeys == null)
					_allKeys = BaseGetAllKeys();

				return _allKeys;
			}
		}

		internal void AddFile(string name, HttpPostedFile httpPostedFile)
		{
			BaseAdd(name, httpPostedFile);
		}
	}
}
