using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.Tokenization
{
    public class TokenizerTests
    {
        public static void RunTests()
        {
            Tokenizer tokenizer = new Tokenizer();
            List<string> testSentences = new List<string>
            {
                "We were there all 12. We did ...",
                "The price is $15.99 and €3.50!",
                "Hello, Mr. Smith! How are you today?",
                "NASA launched Apollo 11 on 16.07.1969.",
                "I have 3.14 apples and 42.0 oranges.",
                "Dr. Johnson is here. He arrived at 10:30 a.m."
            };

            foreach (string sentence in testSentences)
            {
                List<Token> tokens = tokenizer.Tokenize(sentence);
                Console.WriteLine("Input: " + sentence);
                Console.Write("Tokens: ");
                foreach (var token in tokens)
                {
                    Console.Write("[" + token.Spelling + "] ");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
