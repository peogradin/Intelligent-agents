using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NLP.Tokenization
{
    public class Tokenizer
    {

        private HashSet<string> abbreviations = new HashSet<string>
        {
            "e.g.", "i.e.", "mr.", "mrs.", "dr.", "etc.", "st.", "rd.", "ltd.", "co.", "inc.", "jan.", "feb.", "mar.", "apr.", "jun.", "jul.", "aug.", "sep.", "oct.", "nov.", "dec.", "a.m.", "p.m.", "u.s.", "u.k."
        };

        public event Action<int> OnProgressUpdate;

        public List<string> Tokenize(string text)
        {
            // Implement your tokenizer here (to handle abbreviations, numbers, special characters, and so on). 
            // You may wish to add more methods to keep the code well-structured

            // Remove the line below - needed only for the compiler.

            string pattern = @"(?<=[,!?;:])|(?=[,!?;:])|\s+";
            string[] rawTokens = Regex.Split(text, pattern);
            int processedTokens = 0;

            List<string> tokenStrings = new List<string>();

            foreach (string rawToken in rawTokens)
            {
                string token = rawToken.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(token) || token == "+" || token == "-" || token == "*" || token == "...")
                {
                    continue;
                }

                if (abbreviations.Contains(token))
                {
                    tokenStrings.Add(token);
                }
                else if (Regex.IsMatch(token, @"^\d+\.\d+$"))
                {
                    tokenStrings.Add(token);
                }
                else if (token.Contains("."))
                {

                    Match match = Regex.Match(token, @"^(.*?)(\.\.\.?|\.)$");
                    if (match.Success)
                    {
                        string wordPart = match.Groups[1].Value;
                        string dotsPart = match.Groups[2].Value;

                        if (!string.IsNullOrWhiteSpace(wordPart))
                        {
                            tokenStrings.Add(wordPart);
                        }
                        tokenStrings.Add(dotsPart);
                        continue;
                    }

                    string[] subTokens = token.Split('.');
                    foreach (string subToken in subTokens)
                    {
                        if (!string.IsNullOrWhiteSpace(subToken))
                        {
                            tokenStrings.Add(subToken);
                        }
                    }
                }
                else
                {
                    tokenStrings.Add(token);
                }

                processedTokens++;
                if (processedTokens % 500000 == 0)
                {
                    OnProgressUpdate?.Invoke(processedTokens);
                }

            }

            return tokenStrings;
        }
    }
}
