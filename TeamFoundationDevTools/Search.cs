using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TeamFoundationDevTools
{
	static class Search
	{

		static int results = 0;
		static CultureInfo locale = CultureInfo.CurrentCulture;
		static string fileName = Preferences.outputFilePath + "\\" + Preferences.fileName;

		static string
			fileNameWildCard = null,
			filePathContains = null;

		internal static void FullSearch(Uri TpcAddress = null)
		{
			Console.WriteLine("Search Parameters :");

			Preferences.PreferencesHome(PreferenceMenu.View);

			Console.Write(("File Name ( Wild Card ) :").PadRight(51));
			fileNameWildCard = Console.ReadLine();

			if (!fileNameWildCard.Contains('*') || fileNameWildCard.Contains('?'))
				fileNameWildCard = "*" + fileNameWildCard + "*";

			Console.Write(("File Path Contains ( Ignore-Case ) :").PadRight(51));
			filePathContains = Console.ReadLine();

			if (TpcAddress == null || fileNameWildCard == null || fileNameWildCard.Length == 0)
				return;

			var tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(TpcAddress);
			tpc.Authenticate();
			tpc.EnsureAuthenticated();

			var versionControl = tpc.GetService<VersionControlServer>();
			var projects = versionControl.GetAllTeamProjects(true);

			StringBuilder sbContent = new StringBuilder("===================");
			sbContent.AppendLine("*** Search Criteria : ***");
			sbContent.AppendLine("===================");


			sbContent.AppendLine();
			sbContent.AppendLine("Server :" + TpcAddress.OriginalString);
			sbContent.AppendLine("Pattern :" + fileNameWildCard);

			if (filePathContains != null || filePathContains.Length != 0)
				sbContent.AppendLine("File Path Filter : " + filePathContains);

			sbContent.AppendLine("\n\n\n\n");

			sbContent.AppendLine("===================");
			sbContent.AppendLine("*** Results : ***");
			sbContent.AppendLine("===================");

			sbContent.AppendLine(Environment.NewLine + Environment.NewLine);

			string project_progress;
			foreach (var project in projects)
			{
				project_progress = null;
				var items = versionControl.GetItems(project.ServerItem + "/" + fileNameWildCard, RecursionType.Full).Items;

				if (items.Length > 0)
				{

					if (filePathContains != null && filePathContains.Length != 0)
					{
						//items = items.Where(p => p.ServerItem.Contains(filePathContains)).ToArray();
						items = items.Where(p => locale.CompareInfo.IndexOf(p.ServerItem, filePathContains, CompareOptions.IgnoreCase) > 0).ToArray();
					}

					if (items.Length == 0)
					{
						Console.ForegroundColor = ConsoleColor.DarkCyan;
						project_progress = string.Format(">>\t {0} \t {1}", ("path filter in :").PadLeft(25, '.'), project.Name);
						sbContent.AppendLine();
						sbContent.AppendLine(project_progress);
						Console.WriteLine(project_progress);
						Console.ResetColor();
						continue;
					}

					Console.ForegroundColor = ConsoleColor.Green;
					project_progress = string.Format("{0}\t {1} \t {2}", items.Length, ("MATCH FOUND in :").PadLeft(25, '.'), project.Name);
					sbContent.AppendLine();
					sbContent.AppendLine(project_progress);
					Console.WriteLine(project_progress);
					Console.ResetColor();

					results += items.Length;

					sbContent.AppendLine();

					foreach (var item in items)
					{
						var history = (IEnumerable<Changeset>)(versionControl.QueryHistory(item.ServerItem, VersionSpec.Latest, 0, RecursionType.OneLevel, null, null, null, int.MaxValue, true, false, true));
						var h = history.FirstOrDefault();

						string
							changesetId = null,
							commiter = null,
							committedOn = null,
							checkinNote = null;

						if (h == null)
							changesetId = commiter = committedOn = checkinNote = "( ? )";

						else
						{
							changesetId = h.ChangesetId.ToString();
							commiter = (h.CommitterDisplayName ?? "").ToUpper();
							committedOn = (h.CreationDate.ToString("MM/dd/yy HH:mm:ss tt") ?? "").ToUpper();

							if (h.CheckinNote.Values.Count() > 0)
							{
								checkinNote = (h.CheckinNote.Values[0].Value ?? "").ToUpper();
							}
						}


						sbContent.AppendFormat("\t\t\t {0}\t\t{1}\t\t{2}\t\t{3}\t\t{4}" + Environment.NewLine, changesetId, commiter, committedOn, checkinNote, item.ServerItem);
					}
					sbContent.AppendLine();

					Utils.DumpData(ref sbContent, ref fileName);

				}
				else
				{
					project_progress = string.Format("-\t {0} \t {1}", ("------- in :").PadLeft(25, '.'), project.Name);
					sbContent.AppendLine();
					sbContent.AppendLine(project_progress);
					Console.WriteLine(project_progress);

					Utils.DumpData(ref sbContent, ref fileName);
				}
			}


			sbContent.AppendLine(string.Format("{0} Found", results));
			Utils.DumpData(ref sbContent, ref fileName);

			Console.WriteLine("\n\n\nDone. {0} Found.", results);
			Console.WriteLine("Output File : " + fileName);
			Console.ReadKey();
		}

	}
}
