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
                        tokenData.Token.POSTag = "X";
                    }
                }
            }
        }
    }
}
