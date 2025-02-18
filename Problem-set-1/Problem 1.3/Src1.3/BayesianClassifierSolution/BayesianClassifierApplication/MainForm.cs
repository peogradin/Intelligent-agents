using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // For the Path class; see LoadDataSet()
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP;
using NLP.TextClassification;
using NLP.Tokenization;

namespace BayesianClassifierApplication
{
    public partial class MainForm : Form
    {
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";

        private TextClassificationDataSet trainingSet = null;
        private TextClassificationDataSet testSet = null;

        private Vocabulary vocabulary = null;
        private NaiveBayesianClassifier classifier = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private TextClassificationDataSet LoadDataSet()
        {
            TextClassificationDataSet dataSet = null;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    dataSet = new TextClassificationDataSet();
                    StreamReader dataReader = new StreamReader(openFileDialog.FileName);
                    while (!dataReader.EndOfStream)
                    {
                        string line = dataReader.ReadLine();
                        List<string> lineSplit = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        TextClassificationDataItem item = new TextClassificationDataItem();
                        item.Text = lineSplit[0].ToLower();
                        item.ClassLabel = int.Parse(lineSplit[1]);
                        dataSet.ItemList.Add(item);
                    }
                    dataReader.Close();
                    int count0 = dataSet.ItemList.Count(i => i.ClassLabel == 0);
                    int count1 = dataSet.ItemList.Count(i => i.ClassLabel == 1);
                    string fileName = Path.GetFileName(openFileDialog.FileName); // File name without the file path.
                    progressListBox.Items.Add("Loaded data file \"" + fileName + "\" with " + count0.ToString() +
                        " negative reviews and " + count1.ToString() + " positive reviews.");
                }
            }
            return dataSet;
        }

        private void loadTrainingSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trainingSet = LoadDataSet();
            if ((trainingSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTrainingSetToolStripMenuItem.Enabled = false; // To avoid accidentally reloading the training set instead of the validation set...
        }

        private void loadTestSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testSet = LoadDataSet();
            if ((trainingSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTestSetToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tokenizeButton_Click(object sender, EventArgs e)
        {
            tokenizeButton.Enabled = false;

            Tokenizer tokenizer = new Tokenizer();
            foreach (TextClassificationDataItem item in trainingSet.ItemList)
            {
                item.TokenList = tokenizer.Tokenize(item.Text);
            }
            int totalTokensTraining = trainingSet.ItemList.Sum(item => item.TokenList.Count);
            progressListBox.Items.Add("Tokenized training set with " + totalTokensTraining + " tokens.");

            foreach (TextClassificationDataItem item in testSet.ItemList)
            {
                item.TokenList = tokenizer.Tokenize(item.Text);
            }
            int totalTokensTest = testSet.ItemList.Sum(item => item.TokenList.Count);
            progressListBox.Items.Add("Tokenized test set with " + totalTokensTest + " tokens.");

            generateVocabularyButton.Enabled = true;
        }

        private void generateVocabularyButtonButton_Click(object sender, EventArgs e)
        {
            generateVocabularyButton.Enabled = false;
            vocabulary = new Vocabulary();

            foreach (var item in trainingSet.ItemList)
            {
                vocabulary.AddTokens(item.TokenList, item.ClassLabel);
            }

            progressListBox.Items.Add("");
            progressListBox.Items.Add("Generated vocabulary with " + vocabulary.GetTokens().Count + " unique tokens.");
            progressListBox.Items.Add("");

            generateClassifierButton.Enabled = true;
        }

        private void generateClassifierButton_Click(object sender, EventArgs e)
        {
            generateClassifierButton.Enabled = false;

            classifier = new NaiveBayesianClassifier();
            classifier.Initialize(vocabulary, trainingSet);

            progressListBox.Items.Clear();
            progressListBox.Items.Add("Naive Bayesian Classifier generated.");
            progressListBox.Items.Add("");

            double priorPositive = classifier.GetPriorPositive();
            double priorNegative = classifier.GetPriorNegative();

            progressListBox.Items.Add("Prior probabilities:");
            progressListBox.Items.Add("P(c0) = " + priorNegative.ToString("F4"));
            progressListBox.Items.Add("P(c1) = " + priorPositive.ToString("F4"));
            progressListBox.Items.Add("");


            evaluateButton.Enabled = true;

        }

        private void evaluateButton_Click(object sender, EventArgs e)
        {
            evaluateButton.Enabled = false;
            string evaluationResultTraining = classifier.Evaluate(trainingSet);
            progressListBox.Items.Clear();
            progressListBox.Items.Add("Evaluation on training set:");
            progressListBox.Items.Add(evaluationResultTraining);

            string evaluationResultTest = classifier.Evaluate(testSet);
            progressListBox.Items.Add("");
            progressListBox.Items.Add("Evaluation on test set:");
            progressListBox.Items.Add(evaluationResultTest);

            var wordsOfInterest = new List<string> { "friendly", "perfectly", "horrible", "poor" };

            
            progressListBox.Items.Add("");
            progressListBox.Items.Add("Probabilities P(class | word):");
            progressListBox.Items.Add("");
            progressListBox.Items.Add(string.Format("{0,-15} {1,-20} {2,-20}", "Word", "P(c0 | word)", "P(c1 | word)"));

            foreach (var word in wordsOfInterest)
            {
                double probPositive = classifier.GetPosteriorPositive(word);
                double probNegative = classifier.GetPosteriorNegative(word);
                progressListBox.Items.Add(string.Format("{0,-15} {1,-20:F4} {2,-20:F4}", word, probNegative, probPositive));
            }

        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            trainingSet = null;
            testSet = null;
            vocabulary = null;
            classifier = null;
            loadTrainingSetToolStripMenuItem.Enabled = true;
            loadTestSetToolStripMenuItem.Enabled = true;
            tokenizeButton.Enabled = false;
            generateVocabularyButton.Enabled = false;
            generateClassifierButton.Enabled = false;
            evaluateButton.Enabled = false;
            progressListBox.Items.Clear();
        }

    }
}
