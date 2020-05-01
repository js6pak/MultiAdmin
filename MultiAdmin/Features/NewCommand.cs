using System.Diagnostics;
using MultiAdmin.Features.Attributes;
using MultiAdmin.Utility;

namespace MultiAdmin.Features
{
	[Feature]
	internal class NewCommand : Feature, ICommand, IEventServerFull
	{
		private string onFullServerId;
		private Process onFullServerInstance;

		public NewCommand(Server server) : base(server)
		{
		}

		public void OnCall(string[] args)
		{
			if (args.IsEmpty())
			{
				Server.Write("Error: Missing Server ID!");
			}
			else
			{
				var serverId = string.Join(" ", args);

				if (string.IsNullOrEmpty(serverId)) return;

				Server.Write($"Launching new server with Server ID: \"{serverId}\"...");

				Program.StartServer(new Server(serverId));
			}
		}

		public string GetCommand()
		{
			return "NEW";
		}

		public bool PassToGame()
		{
			return false;
		}

		public string GetCommandDescription()
		{
			return "Starts a new server with the given Server ID";
		}

		public string GetUsage()
		{
			return "<SERVER ID>";
		}

		public override void Init()
		{
		}

		public override void OnConfigReload()
		{
			onFullServerId = Server.ServerConfig.StartConfigOnFull.Value;
		}

		public override string GetFeatureDescription()
		{
			return "Adds a command to start a new server given a config folder";
		}

		public override string GetFeatureName()
		{
			return "New";
		}

		public void OnServerFull()
		{
			if (string.IsNullOrEmpty(onFullServerId)) return;

			// If a server instance has been started
			if (onFullServerInstance != null)
			{
				onFullServerInstance.Refresh();

				if (!onFullServerInstance.HasExited) return;
			}

			Server.Write($"Launching new server with Server ID: \"{onFullServerId}\" due to this server being full...");

			onFullServerInstance = Program.StartServer(new Server(onFullServerId));
		}
	}
}
