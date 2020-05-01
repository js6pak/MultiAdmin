using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using MultiAdmin.ConsoleTools;

namespace MultiAdmin
{
	/// <summary>
	/// https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html
	/// </summary>
	public class UnityRichTextParser
	{
		public static Dictionary<string, string> Colors { get; } = new Dictionary<string, string>
		{
			{"aqua", "#00ffff"},
			{"black", "#000000"},
			{"blue", "#0000ff"},
			{"brown", "#a52a2a"},
			{"cyan", "#00ffff"},
			{"darkblue", "#0000a0"},
			{"fuchsia", "#ff00ff"},
			{"green", "#008000"},
			{"grey", "#808080"},
			{"lightblue", "#add8e6"},
			{"lime", "#00ff00"},
			{"magenta", "#ff00ff"},
			{"maroon", "#800000"},
			{"navy", "#000080"},
			{"olive", "#808000"},
			{"orange", "#ffa500"},
			{"purple", "#800080"},
			{"red", "#ff0000"},
			{"silver", "#c0c0c0"},
			{"teal", "#008080"},
			{"white", "#ffffff"},
			{"yellow", "#ffff00"}
		};

		private static Match GetMatch(string text, int index, Regex regex)
		{
			return regex.Match(text.Substring(index));
		}

		public static ColoredMessage[] Parse(string text, Color? color = null)
		{
			var messages = new List<ColoredMessage>();

			try
			{
				var skip = 0;
				var colorStack = new Stack<Color?>();
				colorStack.Push(color);
				var message = new ColoredMessage(string.Empty, colorStack.Peek());

				for (var i = 0; i < text.Length; i++)
				{
					if (skip > 0)
					{
						skip--;
						continue;
					}

					var ch = text[i];
					Match match;
					if ((match = GetMatch(text, i,
						new Regex("^<(color|size|b|i|material|quad)=?([^>]*)>", RegexOptions.IgnoreCase))).Success)
					{
						skip += match.Length - 1;

						var tag = match.Groups[1].Value.ToLower();
						if (tag == "color")
						{
							var value = match.Groups[2].Value.ToLower();

							value = Colors.ContainsKey(value) ? Colors[value] : value;

							messages.Add(message);
							message = new ColoredMessage(string.Empty,
								int.TryParse(value.Replace("#", string.Empty), NumberStyles.HexNumber, null,
									out var parsedColor)
									? Color.FromArgb(parsedColor)
									: colorStack.TryPeek(out var result)
										? result
										: color
							);
							colorStack.Push(message.textColor);
						}
					}
					else if ((match = GetMatch(text, i,
						new Regex("^</(color|size|b|i|material|quad)>", RegexOptions.IgnoreCase))).Success)
					{
						skip += match.Length - 1;
						var tag = match.Groups[1].Value.ToLower();
						if (tag == "color")
						{
							messages.Add(message);
							colorStack.TryPop(out _);
							message = new ColoredMessage(string.Empty,
								colorStack.TryPeek(out var result) ? result : color);
						}
					}
					else
					{
						message.text += ch;

						if (i == text.Length - 1)
						{
							messages.Add(message);
						}
					}
				}
			}
			catch (Exception)
			{
				messages.Clear();
				messages.Add(new ColoredMessage(text, color));
			}

			return messages.ToArray();
		}
	}
}
