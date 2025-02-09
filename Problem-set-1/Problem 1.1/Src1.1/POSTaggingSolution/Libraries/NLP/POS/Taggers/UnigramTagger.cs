using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.POS.Taggers
{
    public class UnigramTagger : POSTagger
    {
        private Dictionary<string, TokenData> unigramModel;

        public UnigramTagger(POSDataSet trainingDataSet)
        {
            unigramModel = new Dictionary<string, TokenData>();

            Dictionary<string, Dictionary<string, int>> wordTagCount = new Dictionary<string, Dictionary<string, int>>();

            foreach (Sentence sentence in trainingDataSet.SentenceList)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string word = tokenData.Token.Spelling;
                    string tag = tokenData.Token.POSTag;

                    if (!wordTagCount.ContainsKey(word))
                    {
                        wordTagCount[word] = new Dictionary<string, int>();
                    }
                    if (!wordTagCount[word].ContainsKey(tag))
                    {
                        wordTagCount[word][tag] = 0;
                    }

                    wordTagCount[word][tag]++;

                }
            }

            foreach (var wordEntry in wordTagCount)
            {
                string word = wordEntry.Key;
                string mostCommonTag = wordEntry.Value.OrderByDescending(kvp => kvp.Value).First().Key;

                Token token = new Token { Spelling = word, POSTag = mostCommonTag };
                unigramModel[word] = new TokenData(token);
            }
        }

        public override List<string> Tag(Sentence sentence)
        {
            List<string> tags = new List<string>();
            string tag;

            foreach (var tokenData in sentence.TokenDataList)
            {
                string word = tokenData.Token.Spelling;

                if (unigramModel.TryGetValue(word, out TokenData assignedTokenData))
                {
                    tag = assignedTokenData.Token.POSTag;
                }
                else
                {
                    tag = "UNKNOWN";
                }
                tags.Add(tag);
            }

            return tags;
        }

    }
}
