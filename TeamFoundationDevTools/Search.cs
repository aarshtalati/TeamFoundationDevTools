using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.IO;

namespace TeamFoundationDevTools
{
    static class Search
    {

        static int results = 0;
        static CultureInfo locale = CultureInfo.CurrentCulture;

        static string fileName = null;
        internal static string GetFileName()
        {
            return Preferences.outputFilePath + "\\" + Preferences.GetFileName();
        }

        static string
            fileNameWildCard = null,
            filePathContains = null;

        // Syntactic sugar for optional parameter ( if yer usin' Vs 2k8 ! )
        internal static void FullSearch() { FullSearch(null); }

        internal static void FullSearch(Uri TpcAddress)
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

            StringBuilder sbContent = new StringBuilder();
            sbContent.AppendLine("===================");
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

            Item slnFile;
            string
                line,
                slnFileContent,
                project_progress,
                fileName = GetFileName(),
                projectNameWithTfsVersion;

            foreach (var project in projects)
            {
                line = null;
                slnFile = null;
                slnFileContent = null;
                project_progress = null;
                projectNameWithTfsVersion = null;

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

                    slnFile = versionControl.GetItems(project.ServerItem + "/*" + project.Name + "*.sln", VersionSpec.Latest, RecursionType.Full).Items.FirstOrDefault();

                    if (slnFile == null)
                    {
                        slnFile = versionControl.GetItems(project.ServerItem + "/*.sln", VersionSpec.Latest, RecursionType.Full).Items.FirstOrDefault();
                    }

                    if (slnFile != null)
                    {
                        // Get file string
                        using (Stream stream = slnFile.DownloadFile())
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                using (StreamReader streamReader = new StreamReader(new MemoryStream(memoryStream.ToArray())))
                                {
                                    line = "";
                                    while ((line = streamReader.ReadLine()) != null)
                                    {
                                        line = line.Trim();
                                        if (line.StartsWith("# Visual Studio ", StringComparison.OrdinalIgnoreCase))
                                        {
                                            projectNameWithTfsVersion = string.Format("{0} ({1})", project.Name, line);
                                            break;
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    project_progress = string.Format("{0}\t {1} \t {2}", items.Length, ("MATCH FOUND in :").PadLeft(25, '.'), projectNameWithTfsVersion ?? project.Name);
                    sbContent.AppendLine();
                    sbContent.AppendLine(project_progress);
                    Console.Write("\n" + project_progress);
                    Console.ResetColor();

                    results += items.Length;

                    sbContent.AppendLine();

                    if (items.Count() > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" ==> Please wait : spitting out details in txt file");
                        Console.ResetColor();
                    }

                    foreach (var item in items)
                    {
                        var history = versionControl.QueryHistory(item.ServerItem, VersionSpec.Latest, 0, RecursionType.OneLevel, null, null, null, 1, true, true);
                        Changeset h = null;

                        var enumer = history.GetEnumerator();
                        int count = 0;

                        while (count < 1 && enumer.MoveNext())
                        {
                            h = (Changeset)enumer.Current;
                            count++;
                        }

                        string
                            changesetId = null,
                            commiter = null,
                            committedOn = null,
                            comment = null;

                        if (h == null)
                            changesetId = commiter = committedOn = comment = "( ? )";

                        else
                        {
                            changesetId = h.ChangesetId.ToString();
                            commiter = (h.Committer ?? "").ToUpper();
                            committedOn = (h.CreationDate.ToString("MM/dd/yy HH:mm:ss tt") ?? "").ToUpper();
                            comment = h.Comment ?? "";
                        }

                        sbContent.AppendFormat("\t{0} {1} {2} {3} {4}" + Environment.NewLine,
                            changesetId.PadRight(10),
                            commiter.PadRight(60),
                            committedOn.PadRight(25),
                            comment.Length > 100 ? comment.Substring(0, 100).PadRight(105) : comment.PadRight(105),
                            item.ServerItem); ;
                    }

                    if (items.Count() > 0)
                    {
                        Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
                        Console.WriteLine("                                                   ");
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
