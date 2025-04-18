﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NLP.Tokenization
{
    public class Tokenizer
    {

        
        public List<List<string>> Tokenize(string text)
        {
            List<List<string>> sentences = new List<List<string>>();

            string[] lines = text.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {

                List<string> tokens = Regex.Split(line.Trim(), @"\s+")
                                           .Where(t => !string.IsNullOrWhiteSpace(t))
                                           .ToList();

                if (tokens.Count > 0)
                {
                    sentences.Add(tokens);
                }
            }

            return sentences;
        }
    }
}
