using System;
using System.Collections.Generic;
using System.Linq;

namespace NLP.TextClassification
{
    public class NaiveBayesianClassifier : TextClassifier
    {
        private Vocabulary vocabulary;
        private Dictionary<string, double> posteriorPositive;
        private Dictionary<string, double> posteriorNegative;
        private double priorPositive;
        private double priorNegative;
        private const double epsilon = 1e-9;
        private Dictionary<string, double> rawFreqPositive;
        private Dictionary<string, double> rawFreqNegative;


        public override void Initialize(Vocabulary vocabulary)
        {
            this.vocabulary = vocabulary;
            posteriorPositive = new Dictionary<string, double>();
            posteriorNegative = new Dictionary<string, double>();
            priorPositive = 0;
            priorNegative = 0;
            rawFreqPositive = new Dictionary<string, double>();
            rawFreqNegative = new Dictionary<string, double>();

        }

        public void Initialize(Vocabulary vocabulary, TextClassificationDataSet dataSet)
        {
            Initialize(vocabulary);
            ComputeProbabilities(dataSet);
        }

        public override int Classify(List<Token> tokenList)
        {
            double positiveLogLikelihood = Math.Log(priorPositive);
            double negativeLogLikelihood = Math.Log(priorNegative);

            foreach (var token in tokenList)
            {
                string word = token.Spelling.ToLower();
                if (posteriorPositive.ContainsKey(word))
                {
                    positiveLogLikelihood += Math.Log(Math.Max(posteriorPositive[word], epsilon));
                }

                if (posteriorNegative.ContainsKey(word))
                {
                    negativeLogLikelihood += Math.Log(Math.Max(posteriorNegative[word], epsilon));
                }

            }
            return positiveLogLikelihood >= negativeLogLikelihood ? 1 : 0;
        }

        public string Evaluate(TextClassificationDataSet dataSet)
        {
            int correct = 0;
            int total = dataSet.ItemList.Count;

            int tp = 0, fp = 0, tn = 0, fn = 0;

            foreach (var item in dataSet.ItemList)
            {
                int predicted = Classify(item.TokenList);
                if (predicted == item.ClassLabel)
                {
                    correct++;
                    if (item.ClassLabel == 1)
                    {
                        tp++;
                    }
                    else
                    {
                        tn++;
                    }
                }
                else
                {
                    if (item.ClassLabel == 1)
                    {
                        fn++;
                    }
                    else
                    {
                        fp++;
                    }
                }
            }

            double accuracy = (double)correct / total;
            double precision = tp + fp > 0 ? (double)tp / (tp + fp) : 0;
            double recall = tp + fn > 0 ? (double)tp / (tp + fn) : 0;
            double f1 = precision + recall > 0 ? 2 * precision * recall / (precision + recall) : 0;

            string results = $"Accuracy = {accuracy:F4}, Precision = {precision:F4}, Recall = {recall:F4}, F1 = {f1:F4}";

            return results;
        }


        public override TextClassifier Copy()
        {
            NaiveBayesianClassifier copiedClassifier = new NaiveBayesianClassifier();
            copiedClassifier.Initialize(this.vocabulary);
            copiedClassifier.posteriorPositive = new Dictionary<string, double>(this.posteriorPositive);
            copiedClassifier.posteriorNegative = new Dictionary<string, double>(this.posteriorNegative);
            copiedClassifier.priorPositive = this.priorPositive;
            copiedClassifier.priorNegative = this.priorNegative;

            return copiedClassifier;
        }

        public void ComputeProbabilities(TextClassificationDataSet dataSet)
        {
            var bagOfWords = vocabulary.GetVocabulary();

            int totalPositiveWords = bagOfWords.Values.Sum(td => td.Class1Count);
            int totalNegativeWords = bagOfWords.Values.Sum(td => td.Class0Count);
            int vocabularySize = bagOfWords.Count;

            int positiveReviews = dataSet.ItemList.Count(i => i.ClassLabel == 1);
            int negativeReviews = dataSet.ItemList.Count(i => i.ClassLabel == 0);
            int totalReviews = dataSet.ItemList.Count;

            priorPositive = (double)positiveReviews / totalReviews;
            priorNegative = (double)negativeReviews / totalReviews;


            foreach (var word in bagOfWords)
            {
                posteriorPositive[word.Key] = (double)(word.Value.Class1Count + 1) / (totalPositiveWords + vocabularySize);
                posteriorNegative[word.Key] = (double)(word.Value.Class0Count + 1) / (totalNegativeWords + vocabularySize);

                rawFreqPositive[word.Key] = (double)word.Value.Class1Count / totalPositiveWords;
                rawFreqNegative[word.Key] = (double)word.Value.Class0Count / totalNegativeWords;
            }

        }

        public double GetPosteriorPositive(string word)
        {
            word = word.ToLower();
            if (!posteriorPositive.ContainsKey(word) || !posteriorNegative.ContainsKey(word))
            {
                return 0;
            }

            double P_t_positive = rawFreqPositive[word];
            double P_t_negative = rawFreqNegative[word];
            double P_positive = priorPositive;
            double P_negative = priorNegative;

            double P_t = P_t_positive * P_positive + P_t_negative * P_negative;

            return P_t > 0 ? P_t_positive * P_positive / P_t : 0;

        }

        public double GetPosteriorNegative(string word)
        {
            word = word.ToLower();
            if (!posteriorPositive.ContainsKey(word) || !posteriorNegative.ContainsKey(word))
            {
                return 0;
            }

            double P_t_positive = rawFreqPositive[word];
            double P_t_negative = rawFreqNegative[word];
            double P_positive = priorPositive;
            double P_negative = priorNegative;

            double P_t = P_t_positive * P_positive + P_t_negative * P_negative;

            return P_t > 0 ? P_t_negative * P_negative / P_t : 0;

        }

        public double GetPriorPositive()
        {
            return priorPositive;
        }

        public double GetPriorNegative()
        {
            return priorNegative;
        }

    }
}
