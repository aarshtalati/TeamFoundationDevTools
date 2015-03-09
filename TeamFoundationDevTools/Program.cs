using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Diagnostics;
using System.IO;

namespace TeamFoundationDevTools
{
	class Program
	{
		static Uri tfsProjectCollectionUri = null;

		static void Main(string[] args)
		{
			Greet();
			tfsProjectCollectionUri = GetRegisteredTfsConnectionUrl();

			if (tfsProjectCollectionUri == null || tfsProjectCollectionUri.ToString().Length == 0)
			{
				Console.WriteLine("");
			}
			Console.ReadKey();
		}

		private static void Greet()
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("===========================================");
			Console.WriteLine("Welcome to TFDT : Team Foundation Dev Tools");
			Console.WriteLine("===========================================");
			Console.ResetColor();
		}

		private static Uri GetRegisteredTfsConnectionUrl()
		{
			List<RegisteredProjectCollection> projectCollections = new List<RegisteredProjectCollection>(RegisteredTfsConnections.GetProjectCollections());

			if (projectCollections.Count == 0)
				return null;

			Uri uri = null;
			int selectedIndex = -1;
			RegisteredProjectCollection p = null;
			string
				input = "",
				message = string.Format("Input TFS Connection Index {0}: ", projectCollections.Count == 1 ? "" : "(0-" + (projectCollections.Count - 1) + ") "),
				line = string.Format("{0}\t {1}\t {2}", ("Index").PadRight(3), ("Name").PadRight(50), ("Online").PadRight(1));

			Console.WriteLine("{0} Registered TFS Connection(s) found :\n", projectCollections.Count);
			Console.WriteLine(line);

			for (int i = 0, N = projectCollections.Count; i < N; i++)
			{
				p = projectCollections[i];
				line = string.Format("{0}\t {1}\t {2}", (i.ToString()).PadRight(3), (p.Name).PadRight(50), (p.Offline ? "N" : "Y").PadRight(1));
				Console.WriteLine(line);
			}

			Console.WriteLine("\n");

			do
			{
				// clear last invalid line in the case of invalid input

				if (selectedIndex != -1)
				{
					Console.SetCursorPosition(0, Console.CursorTop - 1);
					Console.Write(new string(' ', message.Length + input.Length));
					Console.SetCursorPosition(0, Console.CursorTop);
				}

				Console.Write(message);
				input = Console.ReadLine();
			}
			while (!int.TryParse(input, out selectedIndex) || selectedIndex < 0 || selectedIndex > projectCollections.Count - 1);

			p = projectCollections[selectedIndex];
			uri = p.Uri;

			Console.WriteLine("\n{0} ( {1} ) Selected", p.Name, p.Uri.ToString());

			return uri;
		}
	}
}
