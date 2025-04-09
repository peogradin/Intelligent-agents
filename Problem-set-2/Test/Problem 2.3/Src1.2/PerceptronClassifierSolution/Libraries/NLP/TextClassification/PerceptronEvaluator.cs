using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.TextClassification
{
    public class PerceptronEvaluator
    {
        private PerceptronClassifier classifier;

        public PerceptronEvaluator(PerceptronClassifier classifier)
        {
            this.classifier = classifier;
        }

        public double Evaluate(TextClassificationDataSet dataSet)
        {
            if (dataSet.ItemList.Count == 0)
            {
                return 0.0;
            }

            int correctCount = 0;
            foreach (var item in dataSet.ItemList)
            {
                int predictedClass = classifier.Classify(item.TokenList);
                int trueClass = item.ClassLabel;
                if (predictedClass == trueClass)
                {
                    correctCount++;
                }
            }
            return (double)correctCount / dataSet.ItemList.Count;
        }

        public (double accuracy, double precision, double recall, double f1) EvaluateExtended(TextClassificationDataSet dataSet)
        {
            if (dataSet.ItemList.Count == 0)
            {
                return (0.0, 0.0, 0.0, 0.0);
            }

            int TP = 0, FP = 0, TN = 0, FN = 0;

            foreach (var item in dataSet.ItemList)
            {
                int predictedClass = classifier.Classify(item.TokenList);
                int trueClass = item.ClassLabel;

                if (predictedClass == 1 && trueClass == 1)
                {
                    TP++;
                }
                else if (predictedClass == 1 && trueClass == 0)
                {
                    FP++; 
                }
                else if (predictedClass == 0 && trueClass == 0)
                {
                    TN++; 
                }
                else if (predictedClass == 0 && trueClass == 1)
                {
                    FN++;
                }
            }

            double accuracy = (double)(TP + TN) / dataSet.ItemList.Count;
            double precision = TP + FP == 0 ? 0 : (double)TP / (TP + FP);
            double recall = TP + FN == 0 ? 0 : (double)TP / (TP + FN);
            double f1 = (precision + recall) == 0 ? 0 : 2 * (precision * recall) / (precision + recall);

            return (accuracy, precision, recall, f1);
        }

    }
}
