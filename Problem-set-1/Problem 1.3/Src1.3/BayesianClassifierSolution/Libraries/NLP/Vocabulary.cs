using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class Vocabulary
    {
        // Write this class - it should contain a data structure
        // that holds all the words in the vocabulary
        // Use a Dictionary<string, Token> for this purpose.

        // NOTE: In Problem1.1, the Vocabulary class is not used, 
        // instead (only for that problem) the vocabulary is represented
        // as a simple List<Token>.

        private Dictionary<string, TokenData> vocabulary;
        public Vocabulary()
        {
            vocabulary = new Dictionary<string, TokenData>();
        }

        public void AddTokens(List<Token> tokens, int classLabel)
        {
            foreach (Token token in tokens)
            {
                string word = token.Spelling.ToLower();

                if (!vocabulary.ContainsKey(word))
                {
                    vocabulary[word] = new TokenData(token);
                }
                vocabulary[word].Count++;
                if (classLabel == 0)
                {
                    vocabulary[word].Class0Count++;
                }
                else
                {
                    vocabulary[word].Class1Count++;
                }

            }
        }

        public Dictionary<string, TokenData> GetVocabulary()
        {
            return vocabulary;
        }

        public bool Contains(string word)
        {
            return vocabulary.ContainsKey(word.ToLower());
        }

        public List<TokenData> GetTokens()
        {
            return vocabulary.Values.ToList();
        }

    }
}

