using SNP2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SNP2.Classes
{
    public class Document
    {
        public string Text = "";
        public List<IAttribute> Attirbutes = new List<IAttribute>();
        public string[] TextWords;
        public Dictionary<string, int> UniqueWords = new Dictionary<string, int>();

        public void CalculateAttributes()
        {
            foreach (var item in Attirbutes)
            {
                item.CalculateValue(this);
            }
        }

        public void CalculateAttributes(Document SecondDocument)
        {
            foreach (var item in Attirbutes)
            {
                item.CalculateValue(this, SecondDocument);
            }
        }

        public Document(string _Text)
        {
            Text = _Text;
            var tmp = RemoveSpecialCharacters(Text);
            var editedText = tmp.ToLowerInvariant();
            TextWords = editedText.Split(null);
            SeparateWords();
            InitialzieAttributes();
        }
        public static string RemoveSpecialCharacters(string input)
        {
            Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }

        private void SeparateWords()
        {
            int count;
            foreach (var item in TextWords)
            {
                if (!UniqueWords.ContainsKey(item))
                {
                    count = TextWords.Where(x => x == item).Count();
                    UniqueWords.Add(item, count);
                }
            }
        }

        private void InitialzieAttributes()
        {
            Attirbutes.Add(new MeanLengthLeastUsed());
            Attirbutes.Add(new MeanLengthLongestWords());
            Attirbutes.Add(new MeanLengthMostUsed());
            Attirbutes.Add(new MeanLengthShortestWords());
            Attirbutes.Add(new MeanWordLength());
            Attirbutes.Add(new MultipleUsageOfIdenticalWords());
            Attirbutes.Add(new NumberOfMostWords());
        }

    }
}
