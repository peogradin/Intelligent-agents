using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.NGrams
{
    public class NGramManager
    {
        private Dictionary<string, List<NGram>> unigramDictionary;
        private Dictionary<string, List<NGram>> bigramDictionary;
        private Dictionary<string, List<NGram>> trigramDictionary;

        public event Action<int> OnProgressUpdate;

        public NGramManager()
        {
            unigramDictionary = new Dictionary<string, List<NGram>>();
            bigramDictionary = new Dictionary<string, List<NGram>>();
            trigramDictionary = new Dictionary<string, List<NGram>>();
        }

        public void GenerateNGrams(List<List<string>> tokenizedDataSet)
        {
            Dictionary<string, int> unigramCounts = new Dictionary<string, int>();
            Dictionary<string, int> bigramCounts = new Dictionary<string, int>();
            Dictionary<string, int> trigramCounts = new Dictionary<string, int>();

            int totalUnigrams = 0, totalBigrams = 0, totalTrigrams = 0;
            int processedTokens = 0;

            foreach (var sentence in  tokenizedDataSet)
            {
                int sentenceLength = sentence.Count;
                for (int i = 0; i < sentenceLength; i++)
                {
                    string token = sentence[i];

                    if (token != "<bos>")
                    {
                        if (!unigramCounts.ContainsKey(token))
                        {
                            unigramCounts[token] = 0;
                        }
                        unigramCounts[token]++;
                        totalUnigrams++;
                    }

                    if (i < sentenceLength - 1)
                    {
                        string bigram = $"{token} {sentence[i + 1]}";
                        if (!bigramCounts.ContainsKey(bigram))
                        {
                            bigramCounts[bigram] = 0;
                        }
                        bigramCounts[bigram]++;
                        totalBigrams++;
                    }
                    if (i < sentenceLength - 2)
                    {
                        string trigram = $"{token} {sentence[i + 1]} {sentence[i + 2]}";
                        if (!trigramCounts.ContainsKey(trigram))
                        {
                            trigramCounts[trigram] = 0;
                        }
                        trigramCounts[trigram]++;
                        totalTrigrams++;
                    }
                    totalUnigrams++;
                    processedTokens++;
                    if (processedTokens % 1000000 == 0)
                    {
                        OnProgressUpdate?.Invoke(processedTokens);
                    }
                }
            }            

            unigramDictionary = CreateNGramDictionary(unigramCounts, totalUnigrams, 1);
            bigramDictionary = CreateNGramDictionary(bigramCounts, totalBigrams, 2);
            trigramDictionary = CreateNGramDictionary(trigramCounts, totalTrigrams, 3);

        }

        private Dictionary<string, List<NGram>> CreateNGramDictionary(Dictionary<string, int> ngramCounts, int totalNGrams, int n)
        {

            Dictionary<string, List<NGram>> ngramDictionary = new Dictionary<string, List<NGram>>();
            foreach (var pair in ngramCounts)
            {
                var ngram = new NGram(pair.Key)
                {
                    FrequencyPerMillionInstances = (double)pair.Value / totalNGrams * 1_000_000
                };

                string[] words = pair.Key.Split(' ');
                string key = n == 1 ? words[0] : string.Join(" ", words.Take(n - 1));

                if (!ngramDictionary.ContainsKey(key))
                    ngramDictionary[key] = new List<NGram>();
                ngramDictionary[key].Add(ngram);
                
            }
            
            return ngramDictionary;
        }

        public Dictionary<string, List<NGram>> GetUnigramDictionary() => unigramDictionary;
        public Dictionary<string, List<NGram>> GetBigramDictionary() => bigramDictionary;
        public Dictionary<string, List<NGram>> GetTrigramDictionary() => trigramDictionary;


    }
}
