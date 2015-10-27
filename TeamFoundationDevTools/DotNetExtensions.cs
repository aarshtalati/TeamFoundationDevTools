using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TeamFoundationDevTools
{
    /// <summary>
    /// These extension methods are an attempt to keep the actual code base for both branches ( .NET 3.5 and 4.5.2 as close as possible )
    /// </summary>
    public static class DotNetExtensions
    {
        public static void CopyTo(this Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024]; // Fairly arbitrary size
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
    }
}
