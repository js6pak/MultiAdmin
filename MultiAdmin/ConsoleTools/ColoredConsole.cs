using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Pastel;

namespace MultiAdmin.ConsoleTools
{
	public static class ColoredConsole
	{
		public static readonly object WriteLock = new object();

		public static void Write(string text, Color? textColor = null, Color? backgroundColor = null)
		{
			lock (WriteLock)
			{
				if (text == null) return;

				if (textColor.HasValue)
					text = text.Pastel(textColor.Value);

				if (backgroundColor.HasValue)
					text = text.PastelBg(backgroundColor.Value);

				Console.Write(text);
			}
		}

		public static void WriteLine(string text, Color? textColor = null, Color? backgroundColor = null)
		{
			lock (WriteLock)
			{
				Write(text, textColor, backgroundColor);

				Console.WriteLine();
			}
		}

		public static void Write(params ColoredMessage[] message)
		{
			lock (WriteLock)
			{
				foreach (var coloredMessage in message)
				{
					if (coloredMessage != null)
						Write(coloredMessage.text, coloredMessage.textColor, coloredMessage.backgroundColor);
				}
			}
		}

		public static void WriteLine(params ColoredMessage[] message)
		{
			lock (WriteLock)
			{
				Write(message);

				Console.WriteLine();
			}
		}

		public static void WriteLines(params ColoredMessage[] message)
		{
			lock (WriteLock)
			{
				foreach (var coloredMessage in message) WriteLine(coloredMessage);
			}
		}
	}

	public class ColoredMessage : ICloneable
	{
		public string text;
		public Color? textColor;
		public Color? backgroundColor;

		public int Length => text?.Length ?? 0;

		public ColoredMessage(string text, Color? textColor = null, Color? backgroundColor = null)
		{
			this.text = text;
			this.textColor = textColor;
			this.backgroundColor = backgroundColor;
		}

		public bool Equals(ColoredMessage other)
		{
			return string.Equals(text, other.text) && textColor == other.textColor && backgroundColor == other.backgroundColor;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((ColoredMessage)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = text != null ? text.GetHashCode() : 0;
				hashCode = (hashCode * 397) ^ textColor.GetHashCode();
				hashCode = (hashCode * 397) ^ backgroundColor.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(ColoredMessage firstMessage, ColoredMessage secondMessage)
		{
			if (ReferenceEquals(firstMessage, secondMessage))
				return true;

			if (ReferenceEquals(firstMessage, null) || ReferenceEquals(secondMessage, null))
				return false;

			return firstMessage.Equals(secondMessage);
		}

		public static bool operator !=(ColoredMessage firstMessage, ColoredMessage secondMessage)
		{
			return !(firstMessage == secondMessage);
		}

		public override string ToString()
		{
			return text;
		}

		public ColoredMessage Clone()
		{
			return new ColoredMessage(text?.Clone() as string, textColor, backgroundColor);
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public void Write(bool clearConsoleLine = false)
		{
			lock (ColoredConsole.WriteLock)
			{
				ColoredConsole.Write(clearConsoleLine ? ConsoleUtils.ClearConsoleLine(this) : this);
			}
		}

		public void WriteLine(bool clearConsoleLine = false)
		{
			lock (ColoredConsole.WriteLock)
			{
				ColoredConsole.WriteLine(clearConsoleLine ? ConsoleUtils.ClearConsoleLine(this) : this);
			}
		}
	}

	public static class ColoredMessageEnumerableExtensions
	{
		private static string JoinTextIgnoreNull(IEnumerable<object> objects)
		{
			var builder = new StringBuilder(string.Empty);

			foreach (var o in objects)
			{
				if (o != null)
					builder.Append(o);
			}

			return builder.ToString();
		}

		public static string GetText(this IEnumerable<ColoredMessage> message)
		{
			return JoinTextIgnoreNull(message);
		}

		public static void Write(this IEnumerable<ColoredMessage> message, bool clearConsoleLine = false)
		{
			lock (ColoredConsole.WriteLock)
			{
				ColoredConsole.Write(clearConsoleLine
					? ConsoleUtils.ClearConsoleLine(message.ToArray())
					: message.ToArray());
			}
		}

		public static void WriteLine(this IEnumerable<ColoredMessage> message, bool clearConsoleLine = false)
		{
			lock (ColoredConsole.WriteLock)
			{
				ColoredConsole.WriteLine(clearConsoleLine
					? ConsoleUtils.ClearConsoleLine(message.ToArray())
					: message.ToArray());
			}
		}

		public static void WriteLines(this IEnumerable<ColoredMessage> message, bool clearConsoleLine = false)
		{
			lock (ColoredConsole.WriteLock)
			{
				ColoredConsole.WriteLines(clearConsoleLine
					? ConsoleUtils.ClearConsoleLine(message.ToArray())
					: message.ToArray());
			}
		}
	}

	public static class ColorExtensions
	{
		public static Color ToColor(this ConsoleColor c)
		{
			int[] cColors = {
					0x0c0c0c, //Black = 0
					0x0037da, //DarkBlue = 1
					0x13a10e, //DarkGreen = 2
					0x3a96dd, //DarkCyan = 3
					0xc50f1f, //DarkRed = 4
					0x881798, //DarkMagenta = 5
					0xc19c00, //DarkYellow = 6
					0xcccccc, //Gray = 7
					0x767676, //DarkGray = 8
					0x3b78ff, //Blue = 9
					0x16c60c, //Green = 10
					0x61d6d6, //Cyan = 11
					0xe74856, //Red = 12
					0xb4009e, //Magenta = 13
					0xf9f1a5, //Yellow = 14
					0xf2f2f2  //White = 15
				};
			return Color.FromArgb(cColors[(int)c]);
		}
	}
}
