using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TeamFoundationDevTools
{
    public static class DotNetExtensions
    {
        public static void CopyTo(this Stream destination)
        {
            CopyTo(destination,81920);
        }

        public static void CopyTo(this Stream destination, int bufferSize)
        {
            byte[] array = new byte[bufferSize];
            int count;
            while ((count = destination.Read(array, 0, array.Length)) != 0)
            {
               destination.Write(array, 0, count);
            }
        }

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
