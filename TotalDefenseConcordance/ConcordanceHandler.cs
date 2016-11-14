using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TotalDefenseConcordance
{
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

}
