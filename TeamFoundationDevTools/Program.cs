using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Diagnostics;
using System.IO;


// tfsProjectCollectionUri = GetRegisteredTfsConnectionUrl();

// if (tfsProjectCollectionUri == null || tfsProjectCollectionUri.ToString().Length == 0)
// {
// 	Console.WriteLine("");
// }


namespace TeamFoundationDevTools
{
	class Program
	{
		static Uri tfsProjectCollectionUri = null;

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
			if (menus == null)
			{
				menus = new Dictionary<int, string>();
				menus[0] = "Connect / Change Connection";
				menus[1] = "Search";
				menus[2] = "Preferences";
				menus[3] = "Exit";
			}


			int selection = -1;
			tempString = string.Format("Your Selection (0-{0}) : ", menus.Count - 1);

			while (selection != menus.Where(kvp => kvp.Value == "Exit").First().Key)
			{
				ResetScreen();

				// Print Menu
				foreach (KeyValuePair<int, string> menu_item in menus)
					Console.WriteLine("{0}.\t {1}", menu_item.Key, menu_item.Value);


				selection = Utils.GetValidIntChoice(-1, 0, menus.Count - 1, ref tempString);
				ResetScreen();

				switch (selection)
				{
					case 0:
						tfsProjectCollectionUri = Connectivity.GetRegisteredTfsConnectionUri();
						break;

					case 1:
					case 2:
						
					case 3:
						
						return;
					default:
						break;
				}
			}
		}


		private static void Greet()
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("===========================================");
			Console.WriteLine("Welcome to TFDT : Team Foundation Dev Tools");
			Console.WriteLine("===========================================");
			Console.ResetColor();
		}



		
	}
}
