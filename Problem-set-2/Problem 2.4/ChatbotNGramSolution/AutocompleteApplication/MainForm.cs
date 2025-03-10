using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using NLP;
using NLP.NGrams;
using NLP.TextClassification;
using NLP.Tokenization;
using System.Diagnostics;

namespace AutocompleteApplication
{
    public partial class MainForm : Form
    {

        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";
        private string trainingDataSetString = null;
        private string validationDataSetString = null;
        private string testDataSetString = null;
        private List<List<string>> tokenizedTrainingData;
        private List<List<string>> tokenizedValidationData;
        private List<List<string>> tokenizedTestData;
        private Thread tokenizerThread;
        private Thread nGramThread;
        private NGramManager nGramManager = new NGramManager();
        private Chatbot autoCompleter;
        List<string> suggestions;

        public MainForm()
        {
            InitializeComponent();
        }

        private string LoadDataSet(string dataSetName)
        {
            string text = null;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    text = File.ReadAllText(openFileDialog.FileName).ToLower();
                    nGramsListBox.Items.Add($"Loaded {dataSetName}.");
                }
            }
            return text;
        }

        private void loadTrainingDataSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trainingDataSetString = LoadDataSet("training data set");
            if (trainingDataSetString != null) { loadValidationDataSetToolStripMenuItem.Enabled = true; }
            loadTrainingDataSetToolStripMenuItem.Enabled = false;
        }

        private void loadValidationDataSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            validationDataSetString = LoadDataSet("validation data set");
            if (validationDataSetString != null) { loadTestDataSetToolStripMenuItem.Enabled = true; }
            loadValidationDataSetToolStripMenuItem.Enabled = false;
        }

        private void loadTestDataSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testDataSetString = LoadDataSet("test data set");
            if (testDataSetString != null) { tokenizeButton.Enabled = true; }
            loadTestDataSetToolStripMenuItem.Enabled = false;
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tokenizeButton_Click(object sender, EventArgs e)
        {
            tokenizeButton.Enabled = false;
            ShowProgressSafe($"Tokenizing...");
            Tokenizer tokenizer = new Tokenizer();

            tokenizer.OnProgressUpdate += (processedTokens) =>
            {
                ShowProgressSafe($"Processed {processedTokens} tokens");
            };

            tokenizerThread = new Thread(() =>
            {
                tokenizedTrainingData = tokenizer.Tokenize(trainingDataSetString);
                tokenizedValidationData = tokenizer.Tokenize(validationDataSetString);
                tokenizedTestData = tokenizer.Tokenize(testDataSetString);
                ShowProgressSafe($"Tokenization complete.", true);
                ShowProgressSafe($"Training: {tokenizedTrainingData.Count} sentences");
                ShowProgressSafe($"Validation: {tokenizedValidationData.Count} sentences");
                ShowProgressSafe($"Test: {tokenizedTestData.Count} sentences");

                Invoke(new Action(() => generateNGramsButton.Enabled = true));

            });
            tokenizerThread.Start();

        }

        private void generateNGramsButton_Click(object sender, EventArgs e)
        {
            generateNGramsButton.Enabled = false;

            ShowProgressSafe($"Generating NGrams");

            nGramManager.OnProgressUpdate += (progress) =>
            {
                ShowProgressSafe($"NGram generation: {progress} tokens processed");
            };
            nGramThread = new Thread(() =>
            {
                nGramManager.GenerateNGrams(tokenizedTrainingData);
                int totalUnigrams = nGramManager.GetUnigramDictionary().SelectMany(kv => kv.Value).Count();
                ShowProgressSafe("NGram generation complete", true);
                ShowProgressSafe($"Total unigrams (unique tokens): {totalUnigrams}");
                ShowProgressSafe("");
                ShowTopTrigrams(10);
                ShowProgressSafe("");
                ShowTopBigrams(10);
                ShowProgressSafe("");
                ShowTopUnigrams(10);
                Invoke(new Action(() => startAutocompletionButton.Enabled = true));

            });

            nGramThread.Start();

        }

        private void startAutocompletionButton_Click(object sender, EventArgs e)
        {
            startAutocompletionButton.Enabled = false;
            nGramsListBox.Items.Clear();
            autoCompleter = new Chatbot(nGramManager);
            sentenceTextBox.Enabled = true;
            sentenceTextBox.Focus();
        }


        private void sentenceTextBox_TextChanged(object sender, EventArgs e)
        {

            string inputText = sentenceTextBox.Text;
            if (!inputText.EndsWith(" ") || autoCompleter == null)
            {
                suggestionListBox.Visible = false;
                return;
            }

            //List<string> inputTokens = new Tokenizer().Tokenize(inputText);
            //suggestions = autoCompleter.GetSuggestions(inputTokens);
            suggestionListBox.Items.Clear();
            suggestionListBox.Items.AddRange(suggestions.ToArray());

            suggestionListBox.Visible = suggestions.Count > 0;
        }

        private void sentenceTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"Suggestions count: {suggestionListBox.Items.Count}");
            if (e.KeyCode == Keys.Tab)
            {
                if (suggestionListBox.Items.Count > 0)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;

                    string punctuation = ".,!?;:";

                    string selectedWord = suggestionListBox.Items[0].ToString();

                    if (punctuation.Contains(selectedWord))
                    {
                        sentenceTextBox.Text = sentenceTextBox.Text.TrimEnd() + selectedWord;
                    }
                    else
                    {
                        sentenceTextBox.Text = sentenceTextBox.Text.TrimEnd() + " " + selectedWord;
                    }

                    sentenceTextBox.SelectionStart = sentenceTextBox.Text.Length;
                    suggestionListBox.Visible = false;
                    sentenceTextBox.Focus();
                }
            }
        }


        private void suggestionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suggestionListBox.SelectedItem != null)
            {
                string punctuation = ".,!?;:";

                string selectedWord = suggestionListBox.SelectedItem.ToString();

                if (punctuation.Contains(selectedWord))
                {
                    sentenceTextBox.Text = sentenceTextBox.Text.TrimEnd() + selectedWord;
                }
                else
                {
                    sentenceTextBox.Text = sentenceTextBox.Text.TrimEnd() + " " + selectedWord;
                }
                sentenceTextBox.SelectionStart = sentenceTextBox.Text.Length;
                suggestionListBox.Visible = false;
                sentenceTextBox.Focus();
            }
        }

        private void ShowTopTrigrams(int amount)
        {
            var trigramDictionary = nGramManager.GetTrigramDictionary();
            if (trigramDictionary.Count == 0)
            {
                ShowProgressSafe("No trigrams found.");
                return;
            }
            var topTrigrams = trigramDictionary
            .SelectMany(kv => kv.Value)
            .OrderByDescending(ngram => ngram.FrequencyPerMillionInstances)
            .Take(amount);

            ShowProgressSafe($"Top {amount} trigrams:");
            foreach (var trigram in topTrigrams)
            {
                ShowProgressSafe($"{trigram.Identifier} ({trigram.FrequencyPerMillionInstances:F2} per million)");
            }
        }

        private void ShowTopBigrams(int amount)
        {
            var bigramDictionary = nGramManager.GetBigramDictionary();
            if (bigramDictionary.Count == 0)
            {
                ShowProgressSafe("No bigrams found.");
                return;
            }
            var topBigrams = bigramDictionary
            .SelectMany(kv => kv.Value)
            .OrderByDescending(ngram => ngram.FrequencyPerMillionInstances)
            .Take(amount);
            ShowProgressSafe($"Top {amount} bigrams:");
            foreach (var bigram in topBigrams)
            {
                ShowProgressSafe($"{bigram.Identifier} ({bigram.FrequencyPerMillionInstances:F2} per million)");
            }
        }

        private void ShowTopUnigrams(int amount)
        {
            var unigramDictionary = nGramManager.GetUnigramDictionary();
            if (unigramDictionary.Count == 0)
            {
                ShowProgressSafe("No unigrams found.");
                return;
            }
            var topUnigrams = unigramDictionary
            .SelectMany(kv => kv.Value)
            .OrderByDescending(ngram => ngram.FrequencyPerMillionInstances)
            .Take(amount);
            ShowProgressSafe($"Top {amount} unigrams:");
            foreach (var unigram in topUnigrams)
            {
                ShowProgressSafe($"{unigram.Identifier} ({unigram.FrequencyPerMillionInstances:F2} per million)");
            }
        }

        private void ShowProgressSafe(string message, bool clearBefore = false)
        {
            if (InvokeRequired)
            {

                Invoke(new MethodInvoker(() =>
                {
                    if (clearBefore)
                    {
                        nGramsListBox.Items.Clear();
                    }
                    nGramsListBox.Items.Add(message);
                }));

            }
            else
            {
                if (clearBefore)
                {
                    nGramsListBox.Items.Clear();
                }
                nGramsListBox.Items.Add(message);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                if (suggestionListBox.Visible && suggestionListBox.Items.Count > 0)
                {

                    this.sentenceTextBox_KeyDown(this, new KeyEventArgs(Keys.Tab));
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
