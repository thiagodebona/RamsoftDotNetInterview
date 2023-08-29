using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dotnet.MiniJira.Live.Dashboard.App.Helpers
{
    public class ConsoleWriter
    {
        public void Write(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }

            var color_match = Regex.Match(msg, @"{[FB]C=[a-z]+}|{\/[FB]C}", RegexOptions.IgnoreCase);
            if (color_match.Success)
            {
                var initial_background_color = Console.BackgroundColor;
                var initial_foreground_color = Console.ForegroundColor;
                var background_color_history = new List<ConsoleColor>();
                var foreground_color_history = new List<ConsoleColor>();

                var current_index = 0;

                while (color_match.Success)
                {
                    if (color_match.Index - current_index > 0)
                    {
                        Console.Write(msg.Substring(current_index, color_match.Index - current_index));
                    }

                    if (color_match.Value.StartsWith("{BC=", StringComparison.OrdinalIgnoreCase)) // set background color
                    {
                        var background_color_name = GetColorNameFromMatch(color_match);
                        Console.BackgroundColor = GetParsedColorAndAddToHistory(background_color_name, background_color_history, initial_background_color);
                    }
                    else if (color_match.Value.Equals("{/BC}", StringComparison.OrdinalIgnoreCase)) // revert background color
                    {
                        Console.BackgroundColor = GetLastColorAndRemoveFromHistory(background_color_history, initial_background_color);
                    }
                    else if (color_match.Value.StartsWith("{FC=", StringComparison.OrdinalIgnoreCase)) // set foreground color
                    {
                        var foreground_color_name = GetColorNameFromMatch(color_match);
                        Console.ForegroundColor = GetParsedColorAndAddToHistory(foreground_color_name, foreground_color_history, initial_foreground_color);
                    }
                    else if (color_match.Value.Equals("{/FC}", StringComparison.OrdinalIgnoreCase)) // revert foreground color
                    {
                        Console.ForegroundColor = GetLastColorAndRemoveFromHistory(foreground_color_history, initial_foreground_color);
                    }

                    current_index = color_match.Index + color_match.Length;
                    color_match = color_match.NextMatch();
                }

                Console.Write(msg.Substring(current_index));

                Console.BackgroundColor = initial_background_color;
                Console.ForegroundColor = initial_foreground_color;
            }
            else
            {
                Console.Write(msg);
            }
        }

        public void WriteLine(string msg)
        {
            Write(msg);
            Console.WriteLine();
        }

        private string GetColorNameFromMatch(Match match)
        {
            return match.Value.Substring(4, match.Value.IndexOf("}") - 4);
        }

        private ConsoleColor GetParsedColorAndAddToHistory(string colorName, List<ConsoleColor> colorHistory, ConsoleColor defaultColor)
        {
            var new_color = Enum.TryParse<ConsoleColor>(colorName, true, out var parsed_color) ? parsed_color : defaultColor;
            colorHistory.Add(new_color);

            return new_color;
        }

        private ConsoleColor GetLastColorAndRemoveFromHistory(List<ConsoleColor> colorHistory, ConsoleColor defaultColor)
        {
            if (colorHistory.Any())
            {
                colorHistory.RemoveAt(colorHistory.Count - 1);
            }

            return colorHistory.Any() ? colorHistory.Last() : defaultColor;
        }
    }
}
