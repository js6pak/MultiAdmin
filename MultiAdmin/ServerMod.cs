using System;
using System.Collections.Generic;
using System.Text;

namespace MultiAdmin
{
	public enum ServerModType
	{
		PluginLoaderSL,

		[Obsolete("EXILED support is untested and experimental")]
		EXILED,

		[Obsolete("ServerMod2 is abandoned")]
		ServerMod2
	}

	public class ServerMod
	{
		public ServerModType Type { get; set; }
		public Version Version { get; set; }

		public ServerMod(ServerModType type, Version version = null)
		{
			Type = type;
			Version = version ?? new Version();
		}

		public override string ToString()
		{
			return $"{Enum.GetName(typeof(ServerModType), Type)} {(Version?.ToString() ?? "unknown")}";
		}
	}
}
