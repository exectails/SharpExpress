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

using SharpExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.SharpExpress
{
	public class WebApplicationTests
	{
		[Fact]
		public void Listen()
		{
			var test = "Hello from test!";

			var app = new WebApplication();
			app.Get("/test", (req, res) => res.Send(test));
			app.Listen(0);

			var address = string.Format("http://localhost:" + app.EndPoint.Port + "/test");
			var web = new WebClient();
			var response = web.DownloadString(address);

			Assert.Equal(test, response);
		}
	}
}
