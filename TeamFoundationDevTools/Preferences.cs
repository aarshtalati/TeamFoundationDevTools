using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public static bool detailedScreenOutput = false;
		public static bool detailedFileOutput = true;
		public static string searchInProject = "All";

		internal static string tempString = null;

		internal static string GetFileName()
		{
			return string.Format("TFDT_Search_{0}.txt", DateTime.Now.ToString("__yyyy_MM_dd_HH_mm_ss_fff"));
		}


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

					if (selection == (int)PreferenceMenu.View)
						ViewPreferences();
					else
						ChangePreferences();
					break;
			}


		}

		static void ViewPreferences(bool displayOnly = false)
		{
			Console.WriteLine(("Search Preferences for :").PadRight(50, ' ') + " " + Connectivity.tfsProjectCollectionUri.ToString());
			Console.WriteLine(("Output Directory :").PadRight(50, ' ') + " " + outputFilePath);
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
			Console.WriteLine("Just so you know, your changes to preferences would be reset upon exit. Change Preference : ");

			Console.WriteLine(("0\t Save Search Results To :").PadRight(50, ' ') + Preferences.outputFilePath);
			Console.WriteLine(("1\t Search in a Project :").PadRight(50, ' ') + Preferences.searchInProject);
			Console.WriteLine(("2\t Detailed Screen Output :").PadRight(50, ' ') + (Preferences.detailedScreenOutput ? "Y" : "N"));
			Console.WriteLine(("3\t Detailed File Output :").PadRight(50, ' ') + (Preferences.detailedFileOutput ? "Y" : "N"));

			string message = "Preference to Update : ";
			int selection = Utils.GetValidIntChoice(-1, 0, 3, ref message);

			switch (selection)
			{
				case 0:
					Console.Write("Updating, Save Search Results To : ");
					GetValidStringInput(ref Preferences.outputFilePath);

					break;

				case 1:
					Console.Write("Updating, Search in a Project :");

					break;

				case 2:
					Console.Write("Updating, Detailed Screen Output :");

					break;

				case 3:
					Console.Write("Updating, Detailed File Output :");

					break;

				default:
					break;
			}
			Console.ReadKey();
		}

		private static void GetValidStringInput(ref string field)
		{
			tempString = Console.ReadLine();
			if (tempString != null && tempString.Length != 0)
			{
				field = tempString;
				Console.WriteLine("... Updated");
			}
			else
			{
				Console.WriteLine("#ERR : Invalid, value did not change");
			}
		}
	}
}
