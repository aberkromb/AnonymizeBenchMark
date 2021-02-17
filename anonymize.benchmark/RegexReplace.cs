using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Anonymize.Benchmark
{
    public class RegexReplace : IReplace
    {
        public string ReplaceJsonForbiddenVariables(string jsonStr)
        {
            var listReplacements = new List<(int, int, string)>();

            foreach (var param in Program.AnonymizePattern.Params)
            {
                // var pattern = $"(\"{param}\"+ *: *\"+).*(?=\")";
                var pattern = $"\"{param}\":\"(\\w*)\"";
                var matchCollection = Regex.Matches(jsonStr, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

                foreach (Match match in matchCollection)
                {
                    string replacement;
                    // значение в json специально выдилил в группу, чтобы проще было с ним работать
                    // потому далее по тексту везде обращаюсь к группе
                    if (match.Groups[1].Value == "Chocolate")
                    {
                        replacement = "BINGO!";
                    }
                    else
                    {
                        replacement = "************";
                    }

                    listReplacements.Add((match.Groups[1].Index, match.Groups[1].Length, replacement));
                }
            }

            return Replace(jsonStr, listReplacements.OrderBy(tuple => tuple.Item1));
        }

        private string Replace(string jsonStr, IOrderedEnumerable<(int, int, string)> listReplacements)
        {
            var sb = new StringBuilder();
            var currentIndex = 0;

            var jsonSpan = jsonStr.AsSpan();
            
            foreach (var (index, length, replace) in listReplacements)
            {
                sb.Append(jsonSpan.Slice(currentIndex, index - currentIndex));
                sb.Append(replace);
                currentIndex = index + length;
            }

            sb.Append(jsonSpan.Slice(currentIndex));

            return sb.ToString();
        }
    }
}