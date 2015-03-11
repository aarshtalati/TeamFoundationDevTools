using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TeamFoundationDevTools
{
	static class Utils
	{
		/// <summary>
		/// Gets and Validates user input based on specified criteria
		/// </summary>
		/// <param name="preset">preset unaccepted value</param>
		/// <param name="min_val">minimum value</param>
		/// <param name="max_val">maximum value</param>
		/// <param name="message">message to print</param>
		/// <returns></returns>
		internal static int GetValidIntChoice(int preset, int min_val, int max_val, ref string message)
		{
			Console.WriteLine("\n\n");
			int selection = preset;
			string input = null;

			do
			{

				// clear last invalid input
				if (selection != preset)
				{
					Console.SetCursorPosition(0, Console.CursorTop - 1);
					Console.Write(new string(' ', message.Length + input.Length));
					Console.SetCursorPosition(0, Console.CursorTop);
				}

				// print the message to get user input
				Console.Write(message);
				input = Console.ReadLine();

			} while (!int.TryParse(input, out selection) || min_val < 0 || selection > max_val);

			input = null;
			return selection;
		}

		internal static void DumpData(ref StringBuilder sbContent, ref string fileName)
		{
			using (StreamWriter sw = File.AppendText(fileName))
			{
				sw.Write(sbContent.ToString());
				sw.Flush();
			}

			sbContent = new StringBuilder();
		}
	}
}
