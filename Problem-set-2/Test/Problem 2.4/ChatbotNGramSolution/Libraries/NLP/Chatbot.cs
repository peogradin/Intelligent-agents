using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using NLP.NGrams;
using static System.Math;

namespace NLP
{
    public class Chatbot
    {
        private readonly NGramManager nGramManager;
        private Random random;
        private double alpha;
        private double beta;
        private double gamma;
        public Chatbot(NGramManager manager, double alpha, double beta, double gamma)
        {
            this.nGramManager = manager;
            this.random = new Random(42);
            this.alpha = alpha;
            this.beta = beta;
            this.gamma = gamma;
        }

        public Dictionary<string, double> JelinekMercerSmoothing(List<string> previousTokens)
        {
            
            int n = previousTokens.Count;
            Dictionary<string, double> probabilities = new Dictionary<string, double>();

            Dictionary<string, List<NGram>> unigramDictionary = nGramManager.GetUnigramDictionary();
            Dictionary<string, List<NGram>> bigramDictionary = nGramManager.GetBigramDictionary();
            Dictionary<string, List<NGram>> trigramDictionary = nGramManager.GetTrigramDictionary();

            bool isTrigram = n >= 2;
            bool isBigram = n >= 1;

            if (isTrigram)
            {
                string lastTwoWords = $"{previousTokens[n - 2]} {previousTokens[n - 1]}";
                if (!string.IsNullOrEmpty(lastTwoWords) && trigramDictionary.TryGetValue(lastTwoWords, out var trigramList))
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
                else
                {
                    isTrigram = false;
                }

            }

            string lastWord = previousTokens[n - 1];
            if (!string.IsNullOrEmpty(lastWord) && bigramDictionary.TryGetValue(lastWord, out var bigramList))
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
                    if (isTrigram)
                    {
                        probabilities[nextWord] += beta * bigramProbability;
                    }
                    else
                    {
                        probabilities[nextWord] += gamma * bigramProbability;
                    }
                }
            }
            else
            {
                isBigram = false;
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
                if (isTrigram)
                {
                    probabilities[nextWord] += (1 - alpha - beta) * unigramProbability;
                }
                else if (isBigram)
                {
                    probabilities[nextWord] += (1 - gamma) * unigramProbability;
                }
                else
                {
                    probabilities[nextWord] += unigramProbability;
                }
            }

            return probabilities;
        }

        public string GenerateSentence(bool lowTemperatureMode, int maxTokens = 100)
        {
            List<string> sentence = new List<string> { "<bos>" };
            string nextWord;

            for (int i = 0; i < maxTokens; i++)
            {
                Dictionary<string, double> probabilities = JelinekMercerSmoothing(sentence);

                //double totalProbability = probabilities.Sum(p => p.Value); //Debug
                //Console.WriteLine($"Total Probability (Before Sampling): {totalProbability:F6}"); //Debug
                //double totalProbability = probabilities.Sum(p => p.Value);
                //if (Math.Abs(totalProbability - 1.0) > 1e-5)
                //{
                //    Console.WriteLine($"Error: total probability is {totalProbability}, it should be = 1.0.");
                //}

                if (probabilities == null || probabilities.Count == 0)
                {
                    break;
                }

                if (lowTemperatureMode)
                {
                    var top10 = probabilities.OrderByDescending(p => p.Value).Take(10).ToDictionary(p => p.Key, p => p.Value);
                    double sum = top10.Sum(p => p.Value);

                    Dictionary<string, double> normalizedProbabilities = top10.ToDictionary(p => p.Key, p => p.Value / sum);
                    nextWord = SampleFromDistribution(normalizedProbabilities);
                }
                else
                {
                    nextWord = SampleFromDistribution(probabilities);
                }
                sentence.Add(nextWord);

                if (nextWord == "<eos>")
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

        public double ComputePerplexity(List<List<string>> tokenizedDataSet, Action<int> onProgressUpdate = null)
        {
            double logProbabilitySum = 0.0;
            int tokenCount = 0;
            int processedSentences = 0;
            int totalSentences = tokenizedDataSet.Count;

            foreach (var sentence in tokenizedDataSet)
            {
                if (sentence.Count == 0) continue;
                List<string> context = new List<string> { "<bos>" };

                foreach (string token in sentence)
                {
                    if (token == "<bos>") continue;

                    Dictionary<string, double> probabilities = JelinekMercerSmoothing(context);

                    //double totalProbability = probabilities.Sum(p => p.Value);
                    //if (Math.Abs(totalProbability - 1.0) > 1e-5)
                    //{
                    //    Console.WriteLine($"Error: total probability is {totalProbability}, it should be = 1.0. ´Context = {string.Join(" ", context)}");
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"Total Probability: {totalProbability:F6}"); //Debug
                    //}

                    if (probabilities.TryGetValue(token, out double probability) && probability > 0)
                    {
                        logProbabilitySum += Math.Log(probability);
                    }
                    else
                    {
                        logProbabilitySum += Math.Log(1e-10);
                        Console.WriteLine($"Error: couldn't find token {token}");
                    }
                    tokenCount++;
                    if (context.Count == 2)
                    {
                        context.RemoveAt(0);
                    }
                    context.Add(token);
                }

                processedSentences++;
                if (processedSentences % 500 == 0)
                {
                    onProgressUpdate?.Invoke((int)(processedSentences / (double)totalSentences * 100));
                }
            }

            if (tokenCount == 0) return double.PositiveInfinity;
            double perplexity = Math.Exp(-logProbabilitySum / tokenCount);
            return perplexity;
        }

    }
}

