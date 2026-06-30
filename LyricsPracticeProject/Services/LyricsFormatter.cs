using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricsPracticeProject.Services
{
    public class LyricsFormatter
    {
        public string Format(string lyrics)
        {
            if (string.IsNullOrWhiteSpace(lyrics))
            {
                return "Текст песни отсутствует.";
            }

            string normalizedText = lyrics.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
            string[] lines = normalizedText.Split('\n');

            List<string> result = new List<string>();

            foreach (string line in lines)
            {
                string currentLine = line.TrimEnd();

                if (IsSectionTitle(currentLine) &&
                    result.Count > 0 &&
                    result[result.Count - 1] != "")
                {
                    result.Add("");
                }

                result.Add(currentLine);
            }

            return string.Join(Environment.NewLine, result);
        }

        private bool IsSectionTitle(string line)
        {
            string trimmedLine = line.Trim();

            return trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]");
        }
    }
}