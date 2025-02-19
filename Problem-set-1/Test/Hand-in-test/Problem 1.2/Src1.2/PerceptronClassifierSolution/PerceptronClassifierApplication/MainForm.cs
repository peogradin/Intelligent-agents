using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP;
using NLP.TextClassification;
using NLP.Tokenization;
using System.Globalization;

namespace PerceptronClassifierApplication
{
    public partial class MainForm : Form
    {
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";

        private PerceptronClassifier classifier = null;
        private Vocabulary vocabulary = null;
        private TextClassificationDataSet trainingSet = null;
        private TextClassificationDataSet validationSet = null;
        private TextClassificationDataSet testSet = null;

        private Thread optimizerThread;
        private PerceptronOptimizer perceptronOptimizer;
        private bool isSaveData = false;

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
            if ((trainingSet != null) && (validationSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTrainingSetToolStripMenuItem.Enabled = false; // To avoid accidentally reloading the training set instead of the validation set...
        }

        private void loadValidationSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            validationSet = LoadDataSet();
            if ((trainingSet != null) && (validationSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadValidationSetToolStripMenuItem.Enabled = false;
        }

        private void loadTestSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testSet = LoadDataSet();
            if ((trainingSet != null) && (validationSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTestSetToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GenerateVocabulary(TextClassificationDataSet dataSet)
        {
            // Write a method that generates the vocabulary. Note that this
            // should ONLY be done for the training set!


            // You must generate an instance of the Vocabulary class,
            // which you must also implement (a skeleton is available
            // in the NLP library)

            vocabulary = new Vocabulary();

            foreach (TextClassificationDataItem item in dataSet.ItemList)
            {
                vocabulary.AddTokens(item.TokenList);
            }

            progressListBox.Items.Add("");
            progressListBox.Items.Add("Generated vocabulary with " + vocabulary.GetTokens().Count + " unique tokens.");
            progressListBox.Items.Add("");

        }

        private void tokenizeButton_Click(object sender, EventArgs e)
        {
            // Write code here for tokenizing the text. That is,
            // implement the Tokenize() method in the Tokenizer class.

            Tokenizer tokenizer = new Tokenizer();

            // First tokenize the training set:

            // Add code here... - should take the raw Text for each
            // TextClassificationDataItem and generate the TokenList
            // (also placed in the TextClassificationDataItem).

            progressListBox.Items.Clear();
            foreach (TextClassificationDataItem item in trainingSet.ItemList)
            {
                item.TokenList = tokenizer.Tokenize(item.Text);
            }

            int totalTokensTraining = trainingSet.ItemList.Sum(item => item.TokenList.Count);
            progressListBox.Items.Add("Tokenized training set with " + totalTokensTraining + " tokens.");

            // Then build the vocabulary from the training set:
            GenerateVocabulary(trainingSet);

            // Next, tokenize the validation set:

            // Add code here ..

            foreach (TextClassificationDataItem item in validationSet.ItemList)
            {
                item.TokenList = tokenizer.Tokenize(item.Text);
            }
            int totalTokensValidation = validationSet.ItemList.Sum(item => item.TokenList.Count);
            progressListBox.Items.Add("Tokenized validation set with " + totalTokensValidation + " tokens.");

            // Finally, tokenize the test set:

            // Add code here:

            foreach (TextClassificationDataItem item in testSet.ItemList)
            {
                item.TokenList = tokenizer.Tokenize(item.Text);
            }

            int totalTokensTest = testSet.ItemList.Sum(item => item.TokenList.Count);
            progressListBox.Items.Add("Tokenized test set with " + totalTokensTest + " tokens.");

            //
            initializeOptimizerButton.Enabled = true;
        }


        private void initializeOptimizerButton_Click(object sender, EventArgs e)
        {
            // Write code here for initializing a perceptron optimizer, which
            // you must also write (i.e. a class called PerceptronOptimizer).
            // Moreover, as mentioned in the assignment text,
            // it might be a good idea to define an evaluator class (e.g. PerceptronEvaluator)
            // You should place both classes in the TextClassification folder in the NLP library.

            classifier = new PerceptronClassifier();
            classifier.Initialize(vocabulary);

            perceptronOptimizer = new PerceptronOptimizer(classifier, trainingSet, validationSet);
            perceptronOptimizer.EpochCompleted += (epoch, trainingAccuracy, validationAccuracy, bestValidationAccuracy) =>
            {
                ShowProgressSafe($"Epoch {epoch}: ", clearBefore: true);
                ShowProgressSafe($"Training accuracy = {trainingAccuracy:F4},\t Validation accuracy: {validationAccuracy:F4},\t Best validation accuracy: {bestValidationAccuracy:F4}");
            };


            ShowProgressSafe("Initialized the perceptron classifier and optimizer.", clearBefore: true);

            startOptimizerButton.Enabled = true;
            initializeOptimizerButton.Enabled = false;
        }

        private void startOptimizerButton_Click(object sender, EventArgs e)
        {
            startOptimizerButton.Enabled = false;
            initializeOptimizerButton.Enabled = false;

            // Start the optimizer here.

            // For every epoch, the optimizer should (after the epoch has been completed)
            // trigger an event that prints the current accuracy (over the training set
            // and the validation set) of the perceptron classifier (in a thread-safe
            // manner, and with proper (clear) formatting). Do *not* involve
            // the test set here.

            stopOptimizerButton.Enabled = true;

            optimizerThread = new Thread(() =>
            {
                perceptronOptimizer.Train();
            });

            optimizerThread.Start();

            ShowProgressSafe("Started the optimizer.");

        }

        private void stopOptimizerButton_Click(object sender, EventArgs e)
        {
            stopOptimizerButton.Enabled = false;

            // Stop the optimizer here.

            // For simplicity (even though one may perhaps resume the optimizer), at this
            // point, evaluate the best classifier (= best validation performance) over
            // the *test* set, and print the accuracy to the screen (in a thread-safe
            // manner, and with proper (clear) formatting).

            perceptronOptimizer.Stop();
            ShowProgressSafe("Stopping the optimizer...");

            Task.Run(() =>
            {
                if (optimizerThread != null && optimizerThread.IsAlive)
                {
                    optimizerThread.Join();
                    ShowProgressSafe("Stopped the optimizer.", clearBefore: true);
                }
                else
                {
                    ShowProgressSafe("The optimizer was already stopped.", clearBefore: true);
                }

                this.Invoke(new MethodInvoker(() =>
                {
                    classifier.LoadBest();
                    var evaluator = new PerceptronEvaluator(classifier);
                    ShowProgressSafe("Evaluating the best classifier after " + perceptronOptimizer.Epoch() + " epochs.");
                    double trainingAccuracy = evaluator.Evaluate(trainingSet);
                    double validationAccuracy = evaluator.Evaluate(validationSet);
                    double testAccuracy = evaluator.Evaluate(testSet);
                    ShowProgressSafe($"Test accuracy: {testAccuracy:F4},\t Validation accuracy: {validationAccuracy:F4},\t Training accuracy: {trainingAccuracy:F4}");

                    var (topWords, bottomWords) = classifier.GetTopAndBottomWords(isSaveData, 10);

                    int columnWidth = 15;
                    ShowProgressSafe("");
                    ShowProgressSafe("Top words");
                    foreach (var (word, weight) in topWords)
                    {
                        ShowProgressSafe(word.PadRight(columnWidth) + weight.ToString("F4"));
                    }
                    ShowProgressSafe("");
                    ShowProgressSafe("Bottom words");
                    foreach (var (word, weight) in bottomWords)
                    {
                        ShowProgressSafe(word.PadRight(columnWidth) + weight.ToString("F4"));
                    }
                    ShowProgressSafe("");

                    if (isSaveData)
                    {
                        perceptronOptimizer.SaveTrainingData("trainingData.csv");
                        ShowProgressSafe("\nSaved training data to trainingData.csv.");
                        classifier.SaveClassifierExamples(testSet, "ClassifierExamples.csv");
                        ShowProgressSafe("Saved classifier examples to file.");
                    }

                    startOptimizerButton.Enabled = true;
                    initializeOptimizerButton.Enabled = true; // To allow for re-initialization

                }));
            });

            stopOptimizerButton.Enabled = true; // A bit ugly, should wait for the
            // optimizer to actually stop, but that's OK, it will stop quickly.

        }

        private void ShowProgressSafe(string message, bool clearBefore = false)
        {
            if (InvokeRequired)
            {

                Invoke(new MethodInvoker(() =>
                {
                    if (clearBefore)
                    {
                        progressListBox.Items.Clear();
                    }
                    progressListBox.Items.Add(message);
                }));

            }
            else
            {
                if (clearBefore)
                {
                    progressListBox.Items.Clear();
                }
                progressListBox.Items.Add(message);
            }
        }
    }
}
