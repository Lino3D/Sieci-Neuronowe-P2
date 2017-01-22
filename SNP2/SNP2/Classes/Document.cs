using SNP2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace SNP2.Classes
{
    public class Document
    {
        public string Text = "";
        
        public List<IAttribute> Attirbutes = new List<IAttribute>();
        //public List<string> TextWords;
        public List<Dictionary<string, int>> UniqueWords = new List<Dictionary<string, int>>();

        public List<List<IAttribute>> ParagraphAttributesList = new List<List<IAttribute>>();

        public List<List<string>> TextWords = new List<List<string>>();

        public string[] Paragraphs;

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
            var paragraphMarker = "";
             Paragraphs = _Text.Split(new string[] { "\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            Paragraphs = Paragraphs.Where(x => x.Length > 10).ToArray();
            Text = _Text;
            //var tmp = RemoveSpecialCharacters(Text);
            //var editedText = tmp.ToLowerInvariant();
            //TextWords = editedText.Split(null);
            //SeparateWords();


            foreach (var paragraph in Paragraphs)
            {
                var tmp = RemoveSpecialCharacters(paragraph);
                var editedText = tmp.ToLowerInvariant();
                var tmpList = new List<string>();
                foreach (var word in editedText.Split(null))
                {
                    tmpList.Add(word);
                }
                TextWords.Add(tmpList);      
            }
            SeparateWordsInParagraphs();
            InitialzieAttributes();
            InitializeParagraphAttributes();
        }
        public static string RemoveSpecialCharacters(string input)
        {
            Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }

        private void SeparateWords()
        {
            //int count;
            //foreach (var item in TextWords)
            //{
            //    if (!UniqueWords.ContainsKey(item))
            //    {
            //        count = TextWords.Where(x => x == item).Count();
            //        UniqueWords.Add(item, count);
            //    }
            //}
        }

        private void SeparateWordsInParagraphs()
        {
            int count;
            foreach (var list in TextWords)
            {
                foreach (var item in list)
                {
                    foreach (var dictionary in UniqueWords)
                    {
                        if (!dictionary.ContainsKey(item))
                        {
                            count = list.Count(x => x == item);
                            dictionary.Add(item, count);
                        }
                    }
                   
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

        private void InitializeParagraphAttributes()
        {
            foreach (var pargraph in Paragraphs)
            {
                ParagraphAttributesList.Add(Attirbutes);
            }
        }

    }
}
