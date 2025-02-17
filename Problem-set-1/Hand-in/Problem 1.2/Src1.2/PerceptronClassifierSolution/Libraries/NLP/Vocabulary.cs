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

        private Dictionary<string, Token> vocabulary;
        public Vocabulary()
        {
            vocabulary = new Dictionary<string, Token>();
        }

        public void AddTokens(List<Token> tokens)
        {
            foreach (Token token in tokens)
            {
                string word = token.Spelling.ToLower();

                if (!vocabulary.ContainsKey(word))
                {
                    vocabulary.Add(word, token);
                }
            }
        }

        public Dictionary<string, Token> GetVocabulary()
        {
            return vocabulary;
        }

        public bool Contains(string word)
        {
            return vocabulary.ContainsKey(word.ToLower());
        }

        public List<Token> GetTokens()
        {
            return vocabulary.Values.ToList();
        }

    }
}
