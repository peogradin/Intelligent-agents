using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.TextClassification
{
    public class PerceptronOptimizer
    {
        private TextClassificationDataSet trainingSet;
        private TextClassificationDataSet validationSet;
        private double learningRate;
        private int epoch;
        private double bestValidationAccuracy;
        private Random random;
        private bool stopRequest;

        private PerceptronClassifier classifier;
        private List<double> trainingAccuracies = new List<double>();
        private List<double> validationAccuracies = new List<double>();

        public event Action<int, double, double, double> EpochCompleted;

        public PerceptronOptimizer(PerceptronClassifier classifier, TextClassificationDataSet trainingSet, TextClassificationDataSet validationSet)
        {
            this.classifier = classifier;
            this.trainingSet = trainingSet;
            this.validationSet = validationSet;
            this.learningRate = 0.1;
            this.epoch = 0;
            this.bestValidationAccuracy = 0.0;
            random = new Random(0);
            this.stopRequest = false;
        }

        public string Epoch()
        {
            return epoch.ToString();
        }

        public void Train()
        {
            stopRequest = false;

            while (!stopRequest)
            {
                if (stopRequest)
                {
                    break;
                }
                TrainOneEpoch();
            }
        }

        public void Stop()
        {
            stopRequest = true;
            Console.WriteLine("Stop request received.");
        }

        private void TrainOneEpoch()
        {
            var evaluator = new PerceptronEvaluator(classifier);
            epoch++;
            var shuffledTrainingSet = trainingSet.ItemList.OrderBy(x => random.Next()).ToList();

            foreach (var item in shuffledTrainingSet)
            {
                int predictedClass = classifier.Classify(item.TokenList);
                int trueClass = item.ClassLabel;
                int error = trueClass - predictedClass;
                if (error != 0)
                {
                    foreach (var token in item.TokenList)
                    {
                        if (classifier.WeightDictionary.ContainsKey(token.Spelling))
                        {
                            classifier.WeightDictionary[token.Spelling] += learningRate * error;
                        }
                    }
                }
                //classifier.Bias += learningRate * error;
            }

            double trainingAccuracy = evaluator.Evaluate(trainingSet);
            double validationAccuracy = evaluator.Evaluate(validationSet);

            trainingAccuracies.Add(trainingAccuracy);
            validationAccuracies.Add(validationAccuracy);

            if (validationAccuracy > bestValidationAccuracy)
            {
                bestValidationAccuracy = validationAccuracy;
                classifier.SetBest();
            }

            EpochCompleted?.Invoke(epoch, trainingAccuracy, validationAccuracy, bestValidationAccuracy);

        }

        public void SaveTrainingData(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("Epoch,TrainingAccuracy,ValidationAccuracy");
                for (int i = 0; i < trainingAccuracies.Count; i++)
                {
                    writer.WriteLine($"{i+1}, {trainingAccuracies[i]}, {validationAccuracies[i]}");
                }
            }
        }
    }
}
