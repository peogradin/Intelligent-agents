using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLP.NGrams;

namespace NLP
{
    public class Chatbot
    {
        private readonly NGramManager nGramManager;
        private Random random;
        public Chatbot(NGramManager manager)
        {
            this.nGramManager = manager;
            this.random = new Random(42);
        }

        public Dictionary<string, double> JelinekMercerSmoothing(List<string> previousTokens, double alpha = 0.25, double beta = 0.5, double gamma = 0.5)
        {
            
            int n = previousTokens.Count;
            Dictionary<string, double> probabilities = new Dictionary<string, double>();

            Dictionary<string, List<NGram>> unigramDictionary = nGramManager.GetUnigramDictionary();
            Dictionary<string, List<NGram>> bigramDictionary = nGramManager.GetBigramDictionary();
            Dictionary<string, List<NGram>> trigramDictionary = nGramManager.GetTrigramDictionary();

            if (n >= 2)
            {
                string lastTwoWords = $"{previousTokens[n - 2]} {previousTokens[n - 1]}";
                if (trigramDictionary.TryGetValue(lastTwoWords, out var trigramList))
                {
                    double totalTrigramProbability = trigramList.Sum(t => t.FrequencyPerMillionInstances);
                    foreach (var trigram in trigramList)
                    {
                        string nextWord = trigram.TokenList.Last();
                        double trigramProbability = trigram.FrequencyPerMillionInstances / totalTrigramProbability;

                        if (!probabilities.ContainsKey(nextWord))
                        {
                            probabilities[nextWord] = 0.0;
                        }

                        probabilities[nextWord] += alpha * trigramProbability;
                    }
                }
            }

            string lastWord = previousTokens[n - 1];
            if (bigramDictionary.TryGetValue(lastWord, out var bigramList))
            {
                double totalBigramProbability = bigramList.Sum(t => t.FrequencyPerMillionInstances);
                foreach (var bigram in bigramList)
                {
                    string nextWord = bigram.TokenList.Last();
                    double bigramProbability = bigram.FrequencyPerMillionInstances / totalBigramProbability;
                    if (!probabilities.ContainsKey(nextWord))
                    {
                        probabilities[nextWord] = 0.0;
                    }
                    if (n == 1)
                    {
                        probabilities[nextWord] += (gamma) * bigramProbability;
                    }
                    else
                    {
                        probabilities[nextWord] += beta * bigramProbability;
                    }
                }
            }
            
            
            double totalUnigramProbability = unigramDictionary.Sum(t => t.Value[0].FrequencyPerMillionInstances);
            foreach (var unigram in unigramDictionary)
            {
                string nextWord = unigram.Key;
                double unigramProbability = unigram.Value[0].FrequencyPerMillionInstances / totalUnigramProbability;
                if (!probabilities.ContainsKey(nextWord))
                {
                    probabilities[nextWord] = 0.0;
                }
                if (n >= 2)
                {
                    probabilities[nextWord] += (1 - alpha - beta) * unigramProbability;
                }
                else
                {
                    probabilities[nextWord] += (1 - gamma) * unigramProbability;
                }
            }

            return probabilities;
        }

        public string GenerateSentence(bool lowTemperatureMode = false)
        {
            List<string> sentence = new List<string> { "<bos>" };
            
            while (true)
            {
                Dictionary<string, double> probabilities = JelinekMercerSmoothing(sentence);

                if (lowTemperatureMode)
                {
                    var top10 = probabilities.OrderByDescending(p => p.Value).Take(10).ToDictionary(p => p.Key, p => p.Value);
                    double sum = top10.Sum(p => p.Value);

                    Dictionary<string, double> normalizedProbabilities = top10.ToDictionary(p => p.Key, p => p.Value / sum);
                    string nextToken = SampleFromDistribution(normalizedProbabilities);
                    sentence.Add(nextToken);
                }
                else
                {
                    string nextToken = SampleFromDistribution(probabilities);
                    sentence.Add(nextToken);
                }
                if (sentence.Last() == "<eos>")
                {
                    break;
                }
            }

            return string.Join(" ", sentence);
        }

        private string SampleFromDistribution(Dictionary<string, double> probabilities)
        {
            double randomValue = random.NextDouble();
            double cumulative = 0.0;
            foreach (var pair in probabilities)
            {
                cumulative += pair.Value;
                if (randomValue < cumulative)
                {
                    return pair.Key;
                }
            }
            return null;
        }
    }
}

