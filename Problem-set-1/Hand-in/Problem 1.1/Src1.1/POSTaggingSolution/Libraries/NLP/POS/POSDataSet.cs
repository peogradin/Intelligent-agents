using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.POS
{
    public class POSDataSet
    {
        private List<Sentence> sentenceList;

        public POSDataSet()
        {
            sentenceList = new List<Sentence>();
        }

        public List<Sentence> SentenceList
        {
            get { return sentenceList; }
            set { sentenceList = value; }
        }
        public void ConvertPOSTags (Dictionary<string, string> tagMapping)
        {
            foreach (Sentence sentence in SentenceList)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string brownTag = tokenData.Token.POSTag;
                    if (tagMapping.ContainsKey(brownTag))
                    {
                        tokenData.Token.POSTag = tagMapping[brownTag];
                    }
                    else
                    {
                        Console.WriteLine("Unknown Brown Tag: " + tokenData.Token.POSTag); //Debug
                        tokenData.Token.POSTag = "XX";
                    }
                }
            }
        }

        public static (POSDataSet trainingDataSet, POSDataSet testDataSet) Split(POSDataSet completeDataSet, double splitFraction)
        {
            POSDataSet trainingDataSet = new POSDataSet();
            POSDataSet testDataSet = new POSDataSet();

            int splitIndex = (int)(splitFraction*completeDataSet.SentenceList.Count);

            trainingDataSet.SentenceList = completeDataSet.SentenceList.Take(splitIndex).ToList();
            testDataSet.SentenceList = completeDataSet.SentenceList.Skip(splitIndex).ToList();

            return (trainingDataSet, testDataSet);
        }

        public Dictionary<string, (int Count, double Fraction)> CountPOSTags()
        {
            Dictionary<string, int> tagCounts = new Dictionary<string, int>();
            int totalCount = 0;

            foreach (Sentence sentence in SentenceList)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string tag = tokenData.Token.POSTag;
                    if (!tagCounts.ContainsKey(tag))
                    {
                        tagCounts[tag] = 0;
                    }
                    tagCounts[tag]++;
                    totalCount++;
                }
            }

            return tagCounts.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => (kvp.Value, (double)kvp.Value / totalCount));

        }

        public Dictionary<int, (int Count, double Fraction)> CountTagsToWordDistribution()
        {
            Dictionary<string, HashSet<string>> wordTags = new Dictionary<string, HashSet<string>>();

            foreach (Sentence sentence in SentenceList)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string word = tokenData.Token.Spelling;
                    string tag = tokenData.Token.POSTag;

                    if (!wordTags.ContainsKey(word))
                    {
                        wordTags[word] = new HashSet<string>();
                    }

                    wordTags[word].Add(tag);
                }
            }

            int totalCount = wordTags.Count;

            Dictionary<int, int> tagsToWordDistribution = new Dictionary<int, int>();

            foreach (var kvp in wordTags)
            {
                int uniqueTagCount = kvp.Value.Count;

                if (!tagsToWordDistribution.ContainsKey(uniqueTagCount))
                {
                    tagsToWordDistribution[uniqueTagCount] = 0;
                }

                tagsToWordDistribution[uniqueTagCount]++;
            }

            return tagsToWordDistribution.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => (kvp.Value, (double)kvp.Value / totalCount));
        }

    }
}
