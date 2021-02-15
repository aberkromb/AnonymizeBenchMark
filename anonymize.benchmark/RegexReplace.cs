using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Anonymize.Benchmark
{
    public class RegexReplace : IReplace
    {
        public string ReplaceJsonForbiddenVariables(string jsonStr)
        {
            foreach (var param in Program.AnonymizePattern.Params)
            {
                var pattern = $"(?<=\"{param}\"+ *: *\"+).*(?=\")";
                var matchCollection = Regex.Matches(jsonStr, pattern, RegexOptions.IgnoreCase);
                
                foreach (Match match in matchCollection.Reverse())
                {
                    string replacement;
                    if (match.Value == "Chocolate")
                    {
                        replacement = "BINGO!";
                    }
                    else
                    {
                        replacement = "************";
                    }
                    
                    jsonStr = Replace(jsonStr, match.Index, match.Length, replacement);
                }
            }

            return jsonStr;
        }
        
        private string Replace(string s, int index, int length, string replacement)
        {
            var builder = new StringBuilder();
            builder.Append(s.Substring(0,index));
            builder.Append(replacement);
            builder.Append(s.Substring(index + length));
            return builder.ToString();
        }
    }
}