using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using MultiAdmin.ConsoleTools;
using MultiAdmin.Utility;

namespace MultiAdmin.Config
{
	public class Config
	{
		public string[] rawData = { };

		public Config(string path)
		{
			ReadConfigFile(path);
		}

		private string internalConfigPath;

		public string ConfigPath
		{
			get => internalConfigPath;
			private set
			{
				try
				{
					internalConfigPath = Utils.GetFullPathSafe(value);
				}
				catch (Exception e)
				{
					internalConfigPath = value;
					Program.LogDebugException(nameof(ConfigPath), e);
				}
			}
		}

		public void ReadConfigFile(string configPath)
		{
			if (string.IsNullOrEmpty(configPath)) return;

			ConfigPath = configPath;

			try
			{
				rawData = File.Exists(ConfigPath) ? File.ReadAllLines(ConfigPath, Encoding.UTF8) : new string[] { };
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(ReadConfigFile), e);

				new ColoredMessage[] {new ColoredMessage($"Error while reading config (Path = {ConfigPath ?? "Null"}):", ConsoleColor.Red.ToColor()), new ColoredMessage(e.ToString(), ConsoleColor.Red.ToColor())}.WriteLines();
			}
		}

		public void ReadConfigFile()
		{
			ReadConfigFile(ConfigPath);
		}

		public bool Contains(string key)
		{
			return rawData != null && rawData.Any(entry => entry.StartsWith($"{key}:", StringComparison.CurrentCultureIgnoreCase));
		}

		private static string CleanValue(string value)
		{
			if (string.IsNullOrEmpty(value)) return value;

			var newValue = value.Trim();

			try
			{
				if (newValue.StartsWith("\"") && newValue.EndsWith("\""))
					return newValue.Substring(1, newValue.Length - 2);
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(CleanValue), e);
			}

			return newValue;
		}

		public string GetString(string key, string def = null)
		{
			try
			{
				foreach (var line in rawData)
				{
					if (!line.ToLower().StartsWith(key.ToLower() + ":")) continue;

					try
					{
						return CleanValue(line.Substring(key.Length + 1));
					}
					catch (Exception e)
					{
						Program.LogDebugException(nameof(GetString), e);
					}
				}
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(GetString), e);
			}

			return def;
		}

		public string[] GetStringArray(string key, string[] def = null)
		{
			try
			{
				foreach (var line in rawData)
				{
					if (!line.ToLower().StartsWith(key.ToLower() + ":")) continue;

					try
					{
						return line.Substring(key.Length + 1).Split(',').Select(CleanValue).ToArray();
					}
					catch (Exception e)
					{
						Program.LogDebugException(nameof(GetStringArray), e);
					}
				}
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(GetStringArray), e);
			}

			return def;
		}

		public int GetInt(string key, int def = 0)
		{
			try
			{
				var value = GetString(key);

				if (!string.IsNullOrEmpty(value) && int.TryParse(value, out var parseValue))
					return parseValue;
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(GetInt), e);
			}

			return def;
		}

		public uint GetUInt(string key, uint def = 0)
		{
			try
			{
				var value = GetString(key);

				if (!string.IsNullOrEmpty(value) && uint.TryParse(value, out var parseValue))
					return parseValue;
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(GetUInt), e);
			}

			return def;
		}

		public float GetFloat(string key, float def = 0)
		{
			try
			{
				var value = GetString(key);

				if (!string.IsNullOrEmpty(value) && float.TryParse(value, out var parsedValue))
					return parsedValue;
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(GetFloat), e);
			}

			return def;
		}

		public double GetDouble(string key, double def = 0)
		{
			try
			{
				var value = GetString(key);

				if (!string.IsNullOrEmpty(value) && double.TryParse(value, out var parsedValue))
					return parsedValue;
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(GetDouble), e);
			}

			return def;
		}

		public decimal GetDecimal(string key, decimal def = 0)
		{
			try
			{
				var value = GetString(key);

				if (!string.IsNullOrEmpty(value) && decimal.TryParse(value, out var parsedValue))
					return parsedValue;
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(GetDecimal), e);
			}

			return def;
		}

		public bool GetBool(string key, bool def = false)
		{
			try
			{
				var value = GetString(key);

				if (!string.IsNullOrEmpty(value) && bool.TryParse(value, out var parsedValue))
					return parsedValue;
			}
			catch (Exception e)
			{
				Program.LogDebugException(nameof(GetBool), e);
			}

			return def;
		}
	}
}
