﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP;
using NLP.POS;
using NLP.POS.Taggers;

namespace POSTaggingApplication
{
    public partial class MainForm : Form
    {
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";
        private POSDataSet completeDataSet = null;
        private POSDataSet trainingDataSet = null;
        private POSDataSet testDataSet = null;
        private List<TokenData> vocabulary = null;

        private Dictionary<string, string> tagMapping = new Dictionary<string, string>();
        private UnigramTagger unigramTagger;

        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Note: 
        // The Brown corpus is available on the course Canvas page.
        // It can also be obtained at http://www.sls.hawaii.edu/bley-vroman/brown_corpus.html
        // in the file "browntag_nolines.txt: Corpus in one file, tagged, no line numbers, each sentence is one line"
        private void loadPOSCorpusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                int tokenCount = 0;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamReader fileReader = new StreamReader(openFileDialog.FileName);
                    completeDataSet = new POSDataSet();
                    while (!fileReader.EndOfStream)
                    {
                        string line = fileReader.ReadLine();
                        if (line != "")
                        {
                            List<string> lineSplit = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            List<TokenData> tokenDataList = new List<TokenData>();
                            Sentence sentence = new Sentence();
                            foreach (string lineSplitItem in lineSplit)
                            {
                                List<string> spellingAndTag = lineSplitItem.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                Token token = new Token();
                                if (spellingAndTag.Count == 2) // Needed in order to ignore the very last line that just contains "_.".
                                {
                                    token.Spelling = spellingAndTag[0].ToLower().Trim(); // Convert all words to lowercase.
                                    token.POSTag = spellingAndTag[1].Trim();
                                }
                                TokenData tokenData = new TokenData(token);
                                if (token.POSTag.Length == 1 || token.POSTag[1] != '|') // A somewhat ugly fix, needed to remove some junk from the data ...
                                {
                                    tokenDataList.Add(tokenData);
                                    tokenCount++;
                                }
                            }
                            sentence.TokenDataList = tokenDataList;
                            completeDataSet.SentenceList.Add(sentence);
                        }
                    }
                    fileReader.Close();
                    resultsListBox.Items.Add("Loaded the Brown corpus with " + completeDataSet.SentenceList.Count.ToString()
                        + " sentences and " + tokenCount.ToString() + " tokens.");
                }
            }
        }

        private List<TokenData> GenerateVocabulary(POSDataSet dataSet)
        {
            List<TokenData> tmpTokenDataList = new List<TokenData>();
            foreach (Sentence sentence in dataSet.SentenceList)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    tmpTokenDataList.Add(tokenData);
                }
            }
            // Sort in alphabetical order, then by tag (also in alphabetical order)...
            // This takes a few seconds to run: It would have been more elegant (and easy) to put the
            // computation in a separate thread, but I didn't bother to do that here, as it would make
            // the code slightly more complex. Here, it is OK that the application freezes for a few
            // seconds while it is sorting the data.
            tmpTokenDataList = tmpTokenDataList.OrderBy(t => t.Token.Spelling).ThenBy(t => t.Token.POSTag).ToList();
            // ... then merge
            List<TokenData> tokenDataList = MergeTokens(tmpTokenDataList);
            return tokenDataList;
        }

        private List<TokenData> MergeTokens(List<TokenData> unmergedDataSet)
        {
            List<TokenData> mergedDataSet = new List<TokenData>();
            if (unmergedDataSet.Count > 0)
            {
                int index = 0;
                Token currentToken = new Token();
                currentToken.Spelling = unmergedDataSet[index].Token.Spelling;
                currentToken.POSTag = unmergedDataSet[index].Token.POSTag;
                TokenData currentTokenData = new TokenData(currentToken);
                index++;
                while (index < unmergedDataSet.Count)
                {
                    Token nextToken = unmergedDataSet[index].Token;
                    if ((nextToken.Spelling == currentToken.Spelling) && (nextToken.POSTag == currentToken.POSTag))
                    {
                        currentTokenData.Count += 1;
                    }
                    else
                    {
                        mergedDataSet.Add(currentTokenData);
                        currentToken = new Token();
                        currentToken.Spelling = unmergedDataSet[index].Token.Spelling;
                        currentToken.POSTag = unmergedDataSet[index].Token.POSTag;
                        currentTokenData = new TokenData(currentToken);
                    }
                    index++;
                }
                mergedDataSet.Add(currentTokenData); // Add the final element as well ...
            }
            return mergedDataSet;
        }


        private void loadTagConversionDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Write code here: This method should load the tag conversion file. You must
            // represent the mappings somehow, so that the various tags in Brown set
            // can be mapped to the universal tag set. For example, one can maintain
            // a list with rows containing two elements (e.g. List<string[2]>, or List<List<string>>, where
            // the inner list should then contain two elements): The Brown tag and the corresponding
            // Universal tag, e.g.,
            //
            // VBZ -> VERB (verb in 3rd person present tense)
            // VB  -> VERB (verb in infinitive form)
            // NN  -> NOUN (noun in singular form)
            // NNS -> NOUN (noun in plural form) 
            // 
            // ...and so on. 
            // An even more elegant (and more re-usable) way is to define a class called, for example, POSTagConverter,
            // with a Convert method that takes a tag as input and outputs the converted tags.
            // Note that, since the data sets are not very large, you don't need to care much about the speed of the code.
            // Thus when searching for an input tag, you can use the Find() method instead of, say, a binary search 
            // or a Dictionary (but it's up to you).
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tagMapping.Clear();
                    StreamReader fileReader = new StreamReader(openFileDialog.FileName);    
                    while (!fileReader.EndOfStream)
                    {
                        string line = fileReader.ReadLine();
                        // Process data here ...
                        List<string> lineSplitList = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        
                        foreach (string lineSplit in lineSplitList)
                        {
                            List<string> tagPair = lineSplit.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();


                            if (tagPair.Count == 2)
                            {
                                string brownTag = tagPair[0].Trim();
                                string universalTag = tagPair[1].Trim();

                                if (!tagMapping.ContainsKey(brownTag))
                                {
                                    tagMapping[brownTag] = universalTag;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Skipping malformed line: " + lineSplit); //Debug
                            }
                        }

                    }
                    resultsListBox.Items.Add("Loaded tag conversion data with " + tagMapping.Count + " Brown to Universal tag pairs.");
                }
            }

            // Keep these lines: They will activate the conversion button, provided that the
            // Brown data set has been loaded first:
            if (completeDataSet != null)
            {
                if (completeDataSet.SentenceList.Count > 0)
                {
                    convertPOSTagsButton.Enabled = true;
                }
            }
        }

        private void convertPOSTagsButton_Click(object sender, EventArgs e)
        {
            // Write code here, such that the Brown tags are mapped to the 
            // Universal tags (for the complete data set), using the representation described above 
            // After running this method, all the tokens should be assigned
            // one of the 12 Universal tags.
            // 
            // Method call: 
            // completeDataSet.ConvertPOSTags(... <suitable input, namely the tag conversion data> ...); // this you have to write ...

            completeDataSet.ConvertPOSTags(tagMapping);

            // Next, build the vocabulary, using the 12 universal tags (this method you get for free! :) )
            // NOTE: (Only) in this problem (for simplicity) the vocabulary is a simple List<TokenData> rather
            // than an instance of the Vocabulary class (which defines a Dictionary<string, Token>)
            vocabulary = GenerateVocabulary(completeDataSet);



            // Keep this line: It will activate the split button.
            splitDataSetButton.Enabled = true;
        }

        private void splitDataSetButton_Click(object sender, EventArgs e)
        {
            // Split the data set into a training set and a test set (a validation
            // set is not needed here, since no optimization is carried out - the
            // unigram tagger is as it is - no optimization required or possible).
            // The result should be 
            //
            //  trainingDataSet (containing, by default, 80% of the sentences)
            //
            //  testDataSet (contaning the remaining 20% of the sentences)
            //
            double splitFraction;
            Boolean splitFractionOK = double.TryParse(splitFractionTextBox.Text, out splitFraction);
            if (splitFractionOK && splitFraction > 0 && splitFraction <= 1)
            {
                // NOTE: The most elegant way to do this is to write a static method in the POSDataSet class,
                // such as, POSDataSet.Split(POSDataSet completeDataSet, double splitFraction).
                // One should always strive to put methods *where they naturally belong*. In this case,
                // the split method belongs with the POSDataSet. One can also, of course,
                // just write the code here (in this method), instantiating the trainingDataSet and
                // the testSet, and then just adding sentences, but the most elegant way is
                // to define a method in the POSDataSet class. You can read about static
                // methods on MSDN or StackOverflow, for example

                (trainingDataSet, testDataSet) = POSDataSet.Split(completeDataSet, splitFraction);
                resultsListBox.Items.Add("Dataset split with " + trainingDataSet.SentenceList.Count + " training sentences, " +
                    testDataSet.SentenceList.Count + " test sentences.");

                // Keep these lines: It will activate the statistics generation button and the unigram tagger generation button,
                // once the data set has been split.
                generateStatisticsButton.Enabled = true;
                generateUnigramTaggerButton.Enabled = true;
            }
            else
            {
                MessageBox.Show("Incorrectly specified split fraction", "Error", MessageBoxButtons.OK);
            }
        }

        private void generateStatisticsButton_Click(object sender, EventArgs e)
        {
            resultsListBox.Items.Clear(); // Keep this line.

            // Write code here for carrying out all the steps described in 
            // Assignment 1.1a, using the (note!) training set

            // Put different parts of the assignment (different subtasks) in
            // separate methods, in order to keep the code neat and tidy.

            // All the results should be printed *neatly* to the screen, in the
            // associated listBox (resultsListBox), after first clearing it.
            // To add things to the result listbox, one uses the command:
            // resultListBox.Items.Add(...);
            // where ... is a text string. Include empty lines with
            // resultListBox.Items.Add(" ");
            // where appropriate (e.g., between different
            // subtasks, to make the output more readable).

            var tagCounts = trainingDataSet.CountPOSTags();

            resultsListBox.Items.Add("Tag frequency statistics:");
            resultsListBox.Items.Add("# Tags\t Count\t %");

            foreach ( var kvp in tagCounts )
            {
                resultsListBox.Items.Add($"{kvp.Key}:\t {kvp.Value.Count}\t {(kvp.Value.Fraction*100):F2}%");
            }

            resultsListBox.Items.Add(" ");

            var tagsToWordDistribution = trainingDataSet.CountTagsToWordDistribution();

            resultsListBox.Items.Add("Distribution of number of tags per word: ");
            resultsListBox.Items.Add("# Tags\t Count\t %");

            foreach (var kvp in tagsToWordDistribution)
            {
                resultsListBox.Items.Add($"{kvp.Key}\t {kvp.Value.Count}\t {(kvp.Value.Fraction * 100):F2}%");
            }
        }

        private void generateUnigramTaggerButton_Click(object sender, EventArgs e)
        {
            // Write code here for generating a unigram tagger, again using the *training* set;
            // Here, you *should* Define a class Unigram tagger derived from (inheriting) the base class
            // POSTagger in the NLP library included in this solution.

            // For the actual tagging (once the unigram tagger has been generated)
            // you must override the Tag() method in the base class (POSTagger)).

            // In the Unigram tagger, it might be a good idea to use a Dictionary<string, TokenData>
            // for quickly finding tokens (the TokenData) with a given spelling (the string).

            // Note that, for most POS taggers, it matters whether or not a word is
            // (say) the first word or the last word of a sentence, but not for the
            // unigram tagger, so it is easy to write the Tag() method - it need not
            // take into account the position of the word in the sentence.
            resultsListBox.Items.Clear();

            unigramTagger = new UnigramTagger(trainingDataSet);
            resultsListBox.Items.Add("Unigram tagger has been generated.");

            // Keep this line: It will activate the evaluation button for the unigram tagger
            runUnigramTaggerButton.Enabled = true;
        }

        private void runUnigramTaggerButton_Click(object sender, EventArgs e)
        {
            resultsListBox.Items.Clear(); // Keep this line.

            // Write code here for running the unigram tagger over the test set.
            // All the results should be printed *neatly* to the screen, in the
            // associated listBox (resultsListBox), after first clearing it.

            // Note again that, for most POS taggers, it matters whether or not a word is
            // (say) the first word or the last word of a sentence, but not for the
            // unigram tagger. Thus, when you run the unigram tagger the "sentence"
            // that goes into the Tag() method can simply be the entire list of
            // tokens in the test set.

            int correctTags = 0;
            int totalTags = 0;

            foreach (Sentence sentence in testDataSet.SentenceList)
            {
                List<string> predictedTags = unigramTagger.Tag(sentence);

                for (int i = 0;  i < predictedTags.Count; i++)
                {
                    string predictedTag = predictedTags[i];
                    string actualTag = sentence.TokenDataList[i].Token.POSTag;

                    if (predictedTag == "UNKNOWN")
                    {
                        continue; //Word is unknown
                    }

                    if (predictedTag == actualTag)
                    {
                        correctTags++;
                    }

                    totalTags++;

                }
            }

            double accuracy = (double) correctTags / totalTags;

            resultsListBox.Items.Add("--- Unigram Tagging Results ---");
            resultsListBox.Items.Add("Number of Correctly Tagged Words: " + correctTags);
            resultsListBox.Items.Add("Total Number of Tags: " + totalTags);
            resultsListBox.Items.Add("Accuracy: " +  accuracy);
            

        }
    }
}
