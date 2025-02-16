using System;
using System.Collections.Generic;
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

        public event Action<int, double, double> EpochCompleted;

        public event Action<string> ProgressUpdated;

        public PerceptronOptimizer(PerceptronClassifier classifier, TextClassificationDataSet trainingSet, TextClassificationDataSet validationSet)
        {
            this.classifier = classifier;
            this.trainingSet = trainingSet;
            this.validationSet = validationSet;
            this.learningRate = 0.1;
            this.epoch = 0;
            this.bestValidationAccuracy = 0.0;
            random = new Random(42);
            this.stopRequest = false;
        }

        private void ReportProgress(string message)
        {
            ProgressUpdated?.Invoke(message);
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
            }

            double trainingAccuracy = evaluator.Evaluate(trainingSet);
            double validationAccuracy = evaluator.Evaluate(validationSet);

            if (validationAccuracy > bestValidationAccuracy)
            {
                bestValidationAccuracy = validationAccuracy;
                classifier.SetBest();
            }

            EpochCompleted?.Invoke(epoch, trainingAccuracy, validationAccuracy);

        }
    }
}
