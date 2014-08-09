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

using System.IO;

namespace SharpExpress
{
	public class FormFile
	{
		/// <summary>
		/// Name of the form field
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Name of the uploaded file (empty if no file was uploaded)
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		/// Stream to read the file from
		/// </summary>
		public Stream File { get; private set; }

		/// <summary>
		/// Content type of the file
		/// </summary>
		public string ContentType { get; private set; }

		/// <summary>
		/// True if a file was uploaded (FileName is not empty).
		/// </summary>
		public bool HasData { get { return !string.IsNullOrEmpty(this.FileName); } }

		/// <summary>
		/// New file
		/// </summary>
		/// <param name="name"></param>
		/// <param name="fileName"></param>
		/// <param name="file"></param>
		/// <param name="type"></param>
		public FormFile(string name, string fileName, Stream file, string type)
		{
			this.Name = name;
			this.FileName = fileName;
			this.File = file;
			this.ContentType = type;
		}

		/// <summary>
		/// Writes uploaded file to filePath. Creates parent folder(s) if it
		/// doesn't exist yet.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public bool MoveTo(string filePath)
		{
			var folder = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			using (var inFile = this.File)
			using (var outFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
				inFile.CopyTo(outFile);

			return true;
		}
	}
}
