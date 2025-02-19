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

        public List<Token> Tokenize(string text)
        {
            // Implement your tokenizer here (to handle abbreviations, numbers, special characters, and so on). 
            // You may wish to add more methods to keep the code well-structured

            // Remove the line below - needed only for the compiler.

            List<Token> tokens = new List<Token>();
            string pattern = @"(?<=\s)|(?=\s)|(?<=[,!?;:\p{Sc}€$£])|(?=[,!?;:\p{Sc}€$£])";
            string[] rawTokens = Regex.Split(text, pattern);

            List<string> finalTokens = new List<string>();

            foreach (string rawToken in rawTokens)
            {
                string token = rawToken.Trim().ToLower();

                if (string.IsNullOrEmpty(token))
                {
                    continue;
                }

                if (token.Contains("."))
                {
                    if (abbreviations.Contains(token)) // Check for abbreviations
                    {
                        finalTokens.Add(token);
                    }

                    else if (Regex.IsMatch(token, @"^\d+(\.\d+)+$")) // Check for numbers
                    {
                        finalTokens.Add(token);
                    }

                    else // Split tokens by periods
                    {
                        string[] splitTokens = Regex.Split(token, @"(?=\.)|(?<=\.)");
                        foreach (string splitToken in splitTokens)
                        {
                            if (!string.IsNullOrWhiteSpace(splitToken))
                            {
                                finalTokens.Add(splitToken);
                            }
                        }

                    }
                }


                else
                {
                    finalTokens.Add(token
                        .Replace("$", "dollar")
                        .Replace("€", "euro")
                        .Replace("£", "pound")
                        );
                }

            }

            foreach (string token in finalTokens)
            {
                tokens.Add(new Token { Spelling = token });
            }

            return tokens;
        }
    }
}
