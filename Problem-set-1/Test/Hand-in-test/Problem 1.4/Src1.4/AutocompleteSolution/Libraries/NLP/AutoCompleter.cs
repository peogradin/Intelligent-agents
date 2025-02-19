using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLP.NGrams;

namespace NLP
{
    public class AutoCompleter
    {
        private readonly NGramManager nGramManager;

        public AutoCompleter(NGramManager manager)
        {
            this.nGramManager = manager;
        }

        public List<string> GetSuggestions(List<string> tokens)
        {

            if (tokens.Count == 0) return new List<string>();

            if (tokens.Count >= 2)
            {

                string lastTwoWords = $"{tokens[tokens.Count - 2]} {tokens[tokens.Count - 1]}";
                if (nGramManager.GetTrigramDictionary().TryGetValue(lastTwoWords, out var trigramList))
                {
                    return trigramList
                        .OrderByDescending(ngram => ngram.FrequencyPerMillionInstances)
                        .Select(ngram => ngram.TokenList.Last())
                        .Distinct()
                        .ToList();
                }
            }
            else if (tokens.Count == 1)
            {

                string searchKey = $". {tokens[0]}";
                if (nGramManager.GetTrigramDictionary().TryGetValue(searchKey, out var trigramList))
                {
                    return trigramList
                        .OrderByDescending(ngram => ngram.FrequencyPerMillionInstances)
                        .Select(ngram => ngram.TokenList.Last())
                        .Distinct()
                        .ToList();
                }
            }

            string lastWord = tokens.Last();
            if (nGramManager.GetBigramDictionary().TryGetValue(lastWord, out var bigramList))
            {
                return bigramList
                    .OrderByDescending(ngram => ngram.FrequencyPerMillionInstances)
                    .Select(ngram => ngram.TokenList.Last())
                    .Distinct()
                    .ToList();
            }


            return new List<string>();
        }
    }
}

