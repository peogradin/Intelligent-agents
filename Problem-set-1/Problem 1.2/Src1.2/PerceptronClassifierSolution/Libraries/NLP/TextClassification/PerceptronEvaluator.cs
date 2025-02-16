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

    }
}
