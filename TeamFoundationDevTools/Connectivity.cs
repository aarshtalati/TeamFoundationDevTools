using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TeamFoundationDevTools
{
	static class Connectivity
	{
		static string message = null;
		internal static Uri tfsProjectCollectionUri = null;

		internal static void GetRegisteredTfsConnectionUri()
		{
			tfsProjectCollectionUri = null;
			List<RegisteredProjectCollection> projectCollections = new List<RegisteredProjectCollection>(RegisteredTfsConnections.GetProjectCollections());

			if (projectCollections.Count == 0)
				return;

			int selectedIndex = -1;
			RegisteredProjectCollection p = null;
			string line = string.Format("{0}\t {1}\t {2}", ("Index").PadRight(3), ("Name").PadRight(50), ("Online").PadRight(1));

			message = string.Format("Input TFS Connection Index {0}: ", projectCollections.Count == 1 ? "" : "(0-" + (projectCollections.Count - 1) + ") ");

			Console.WriteLine("{0} Registered TFS Connection(s) found :\n", projectCollections.Count);
			Console.WriteLine(line);

			for (int i = 0, N = projectCollections.Count; i < N; i++)
			{
				p = projectCollections[i];
				line = string.Format("{0}\t {1}\t {2}", (i.ToString()).PadRight(3), (p.Name).PadRight(50), (p.Offline ? "N" : "Y").PadRight(1));
				Console.WriteLine(line);
			}

			Console.WriteLine("\n");

			if (projectCollections.Count == 1)
				selectedIndex = 0;
			else
				selectedIndex = Utils.GetValidIntChoice(selectedIndex, 0, projectCollections.Count - 1, ref message);

			p = projectCollections[selectedIndex];
			tfsProjectCollectionUri = p.Uri;


			Console.WriteLine("\n{0}Selected Project Collection : {1} ( {2} )", projectCollections.Count == 1 ? "Auto-" : "", p.Name, p.Uri.ToString());
			Console.WriteLine(">>> Press Any key to return.");
			Console.ReadKey();

			message = null;
		}
	}
}
