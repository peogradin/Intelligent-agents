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
        private string dataSetString = null;
        private List<string> tokens;
        private Thread tokenizerThread;
        private Thread nGramThread;
        private NGramManager nGramManager = new NGramManager();
        private AutoCompleter autoCompleter;
        List<string> suggestions;

        public MainForm()
        {
            InitializeComponent();
        }

        // Write this method (and, of course, all other relevant methods, placed in
        // appropriately named classes, placed in a suitable folder.
        //
        // Note: To add a folder, right-click on the project in the Solution Explorer,
        // (e.g., NLP), then select Add - New Folder. 
        // Do NOT add folders externally (outside Visual Studio).
        //
        // Here, no class labels are needed. Instead you simply
        // need to read text and then tokenize it, after which you can generate
        // the n-grams (for n = 1,2, and 3).

        private string LoadDataSet()
        {
            string text = null;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    text = File.ReadAllText(openFileDialog.FileName).ToLower();
                    nGramsListBox.Items.Add($"Loaded dataset from {openFileDialog.FileName}");
                }
            }
            return text;
        }

        private void loadDataSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataSetString = LoadDataSet();
            if (dataSetString != null) { tokenizeButton.Enabled = true; }
            loadDataSetToolStripMenuItem.Enabled = false;
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
                tokens = tokenizer.Tokenize(dataSetString);
                ShowProgressSafe($"Tokenization complete. {tokens.Count} tokens found.", true);

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
                nGramManager.GenerateNGrams(tokens);
                ShowProgressSafe("NGram generation complete", true);
                ShowTopTrigrams();
                Invoke(new Action(() => startAutocompletionButton.Enabled = true));

            });

            nGramThread.Start();

        }

        private void startAutocompletionButton_Click(object sender, EventArgs e)
        {
            startAutocompletionButton.Enabled = false;
            nGramsListBox.Items.Clear();
            autoCompleter = new AutoCompleter(nGramManager);
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

            List<string> inputTokens = new Tokenizer().Tokenize(inputText);
            suggestions = autoCompleter.GetSuggestions(inputTokens);
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

        private void ShowTopTrigrams()
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
            .Take(5);

            ShowProgressSafe("Top 5 trigrams:");
            foreach (var trigram in topTrigrams)
            {
                ShowProgressSafe($"{trigram.Identifier} ({trigram.FrequencyPerMillionInstances:F2} per million)");
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
