using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

                for (int i = 0; i < line.Sentences.Count; i++)
                {

                    if (i < line.Sentences.Count - 1)
                    {
                        sentenceList.Append(" " + line.Sentences[i].ToString() + ",");
                    }

                    if (i == line.Sentences.Count - 1)
                    {
                        sentenceList.Append(" " + line.Sentences[i].ToString() + ".");
                    }
                }

                Console.WriteLine("The word: "+ line.Word + " appeared " + line.Count + " times, and was in lines:" + sentenceList);
            }
        }
    }
}