using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TotalDefenseConcordance
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 1)
            {
                Console.WriteLine("Please specify a valid file path to generate a concordance from. ");
            }

            string path = args[0];

            string textToParse = "";
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                textToParse = sr.ReadToEnd();
            }

            var concordanceHandler = new ConcordanceHandler(textToParse);
            var outputList = concordanceHandler.PrintConcordances();

            PrintOutputs(outputList);
            Console.ReadLine();
        }

        private static void PrintOutputs(List<Concordance> outputList)
        {
            foreach (var line in outputList)
            {
                var sentenceList = new StringBuilder();
                foreach (var lineNumber in line.Sentences)
                {
                    sentenceList.Append(" " + lineNumber.ToString());
                }
                Console.WriteLine(line.Word + " appeared " + line.Count + " times, and was in lines:" + sentenceList);
            }
        }
    }

    public class ConcordanceHandler
    {
        private Dictionary<string, Concordance> Concordance = new Dictionary<string, Concordance>();
        private string regexToSplitIntoSentences = @"(?<!\w\.\w.)(?<![A-Z][a-z]\.)(?<=\.|\?)\s";

        public ConcordanceHandler(string textToParse)
        {
            var sentences = Regex.Split(textToParse, regexToSplitIntoSentences);

            for (int i = 0; i < sentences.Length; i++)
            {
                var words = sentences[i].Split(new char[] { ' ', '\t' }).ToList();

                foreach (var word in words)
                {
                    var cleanWord = word.TrimEnd('.', ',', ':').ToLower();

                    if (cleanWord.Equals(Environment.NewLine) || string.IsNullOrEmpty(cleanWord) || string.IsNullOrWhiteSpace(cleanWord))
                        continue;

                    if (Concordance.ContainsKey(cleanWord))
                    {
                        var currentConcordance = Concordance[cleanWord];
                        currentConcordance.Count++;
                        if (!Concordance[cleanWord].Sentences.Contains(i + 1))
                            currentConcordance.Sentences.Add(i + 1);
                        Concordance[cleanWord] = currentConcordance;
                    }

                    else
                    {
                        Concordance.Add(cleanWord, new Concordance { Count = 1, Sentences = new List<int>(), Word = cleanWord });
                        Concordance[cleanWord].Sentences.Add(i + 1);
                    }
                }
            }
        }

        public List<Concordance> PrintConcordances()
        {
            List<Concordance> returnList = Concordance.Select(x => new Concordance { Word = x.Key, Count = x.Value.Count, Sentences = x.Value.Sentences }).OrderBy(x => x.Word).ToList();
            return returnList;
        }
    }
    public class Concordance
    {
        public int Count { get; set; }
        public List<int> Sentences { get; set; } = new List<int>();
        public string Word { get; set; }
    }
}

//5mins: stubbing out getting the test data in from a file and read to a string. Looked up a few things on SO because I had forgotten.

//5mins: Next, I will break by sentence, and have a collection of sentences. Looked up a regex to do the splitting without mistaking "e.g." as a sentence delimiter. 
//40 so far (4:47) Then, I will create a method which accepts them, in order, and keeps a word count and the enumeration of each sentence the word occurred in.
//Finally, I will test it and see if it matches the output in the e-mail that was sent to me. 