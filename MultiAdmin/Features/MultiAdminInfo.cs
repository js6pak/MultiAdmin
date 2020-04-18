using System;
using System.Reflection;
using MultiAdmin.Features.Attributes;

namespace MultiAdmin.Features
{
	[Feature]
	internal class MultiAdminInfo : Feature, IEventServerPreStart, ICommand
	{
		public MultiAdminInfo(Server server) : base(server)
		{
		}

		public void OnCall(string[] args)
		{
			PrintInfo();
		}

		public string GetCommand()
		{
			return "INFO";
		}

		public bool PassToGame()
		{
			return false;
		}

		public string GetCommandDescription()
		{
			return GetFeatureDescription();
		}

		public string GetUsage()
		{
			return string.Empty;
		}

		public void OnServerPreStart()
		{
			PrintInfo();
		}

		public override void Init()
		{
		}

		public override void OnConfigReload()
		{
		}

		public void PrintInfo()
		{
			Server.Write($"{Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title} (https://github.com/js6pak/MultiAdmin/)\nReleased under MIT License and forked by js6pak", ConsoleColor.DarkMagenta);
		}

		public override string GetFeatureDescription()
		{
			return $"Prints {nameof(MultiAdmin)} license and version information";
		}

		public override string GetFeatureName()
		{
			return "MultiAdminInfo";
		}
	}
}
