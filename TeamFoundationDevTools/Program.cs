using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamFoundationDevTools
{
	class Program
	{
		static string tempString = null;
		static Dictionary<int, string> menus = null;

		static void Main(string[] args)
		{
			Home();
			Console.WriteLine(">>> Press Any Key to Exit ...");
			Console.ReadKey();
		}

		private static void ResetScreen()
		{
			Console.Clear();
			Greet();
		}

		private static void Home()
		{
			int selection = -1;

			while (selection != 3)
			{
				ResetScreen();

				if (menus == null)
				{
					menus = new Dictionary<int, string>();
					menus[0] = Connectivity.tfsProjectCollectionUri == null ? "Not Connected. ( Connect ? )" : "Connected ! ( Change ? )";
					menus[1] = "Preferences";
					menus[2] = "Search";
					menus[3] = "List Users";
					menus[4] = "Exit";
				}

				tempString = string.Format("Your Selection (0-{0}) : ", menus.Count - 1);

				// Print Menu
				foreach (KeyValuePair<int, string> menu_item in menus)
				{
					if (menu_item.Key == 0 && menu_item.Value == "Not Connected. ( Connect ? )")
					{
						Console.BackgroundColor = ConsoleColor.Yellow;
						Console.ForegroundColor = ConsoleColor.Red;
					}

					if (menu_item.Key == 0 && menu_item.Value == "Connected ! ( Change ? )")
						Console.ForegroundColor = ConsoleColor.Green;

					Console.WriteLine("{0}.\t {1}", menu_item.Key, menu_item.Value);
					Console.ResetColor();
				}

				selection = Utils.GetValidIntChoice(-1, 0, menus.Count - 1, ref tempString);

				menus = null;
				ResetScreen();

				if (Connectivity.tfsProjectCollectionUri == null && selection != 0)
				{
					Console.Write("#ERR : \nYou've got to connect first from the main menu. Press Any Key ...");
					Console.ReadKey();
					continue;
				}

				switch (selection)
				{
					case 0:
						Connectivity.GetRegisteredTfsConnectionUri();
						break;

					case 1:
						Preferences.PreferencesHome();
						break;

					case 2:
						Search.FullSearch(Connectivity.tfsProjectCollectionUri);
						break;

					case 3:
						Search.ListUsers(Connectivity.tfsProjectCollectionUri);
						break;

					case 4:
						return;

					default:
						break;
				}
			}
		}

		private static void Greet()
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("===============================================================");
			Console.WriteLine("Welcome to TFDT : Team Foundation Dev Tools ( License : GPLv2 )");
			Console.WriteLine("===============================================================");
			Console.ResetColor();
		}
	}
}
