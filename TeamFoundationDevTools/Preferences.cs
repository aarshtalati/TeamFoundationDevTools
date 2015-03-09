using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamFoundationDevTools
{
	static class Preferences
	{
		static string outputFilePath = System.Environment.SpecialFolder.MyDocuments.ToString();
		static string fileName = string.Format("TFSSearch_{0}.txt", DateTime.Now.ToString("__yyyy_MM_dd_HH_mm_ss_fff"));
	}
}
