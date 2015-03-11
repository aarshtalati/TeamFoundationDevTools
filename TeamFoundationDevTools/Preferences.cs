using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamFoundationDevTools
{
	enum PreferenceMenu
	{
		View,
		Change
	}

	static class Preferences
	{
		public static string outputFilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		
		internal static string GetFileName()
		{
			return string.Format("TFSSearch_{0}.txt", DateTime.Now.ToString("__yyyy_MM_dd_HH_mm_ss_fff"));
		}

		public static bool detailedScreenOutput = false;
		public static bool detailedFileOutput = true;

		

		internal static void PreferencesHome(PreferenceMenu? choice = null)
		{
			switch (choice)
			{
				case PreferenceMenu.View:
					ViewPreferences(true);
					break;


				case PreferenceMenu.Change:
					ChangePreferences();
					break;


				default:
					Console.WriteLine("Preferences Operations :");
					Console.WriteLine("0.\t View");
					Console.WriteLine("1.\t Change");
					string message = "Your Selection : ";
					int selection = Utils.GetValidIntChoice(-1, 0, 1, ref message);

					if(selection == (int)PreferenceMenu.View)
							ViewPreferences();
					else
							ChangePreferences();

					break;
			}


		}

		static void ViewPreferences(bool displayOnly = false)
		{
			Console.WriteLine(("Search Preferences for :").PadRight(50, ' ') + " " + Connectivity.tfsProjectCollectionUri.ToString());
			Console.WriteLine(("Results To :").PadRight(50, ' ') + " " + outputFilePath + "\\" + GetFileName() + " (file name may vary)");
			Console.WriteLine(("Detailed Output on Screen :").PadRight(50, ' ') + " {0}", detailedScreenOutput ? "Yes" : "No");
			Console.WriteLine(("Detailed Output in txt :").PadRight(50, ' ') + " {0}", detailedFileOutput ? "Yes" : "No");

			if (!displayOnly)
			{
				Console.WriteLine("\n\n\n>>> Press Any key to return.");
				Console.ReadKey();
			}
		}

		static void ChangePreferences()
		{
			Console.WriteLine("YET TO BE IMPLEMENTED ... ");
			Console.ReadKey();
		}
	}
}
