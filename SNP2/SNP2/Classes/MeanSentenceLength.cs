using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    [Serializable]
    public class MeanSentenceLength : BaseAttribute
    {
        public override void CalculateValue(Document doc)
        {
            //foreach (var paragraphUniqueWords in doc.UniqueWords)
            //var paragraphUniqueWords = doc.UniqueWords[ParagraphIndex];
            //{
            //    var mostCommonWords = paragraphUniqueWords.OrderBy(x => x.Value).Take(5).Select(x => x.Value).ToList();
            //    float sum = 0;
            //    MinValue = mostCommonWords.Min();
            //    MaxValue = mostCommonWords.Max();

            //    foreach (var VARIABLE in mostCommonWords)
            //    {
            //        if (MaxValue != MinValue)
            //            sum += (VARIABLE - MaxValue)/(MaxValue - MinValue);
            //    }

            //    Value = Math.Abs(sum/NumOfWords);
            //}

            var thisParagraph = doc.Paragraphs[ParagraphIndex];

            var sentences = thisParagraph.Split('.');

            var firstOrDefault = sentences.OrderBy(x => x.Length).FirstOrDefault();
            if (firstOrDefault != null)
                MinValue = firstOrDefault.Length;
            var orDefault = sentences.OrderByDescending(x => x.Length).FirstOrDefault();
            if (orDefault != null)
                MaxValue = orDefault.Length;

            float sum = 0 ;

            if(MaxValue!=MinValue)
            foreach (var sentence in sentences)
            {
                sum += (sentence.Length - MaxValue)/(MaxValue - MinValue);
            }
            Value = Math.Abs(sum/NumOfWords);
        }
    }
}
