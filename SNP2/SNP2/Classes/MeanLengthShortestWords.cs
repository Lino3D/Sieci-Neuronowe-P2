using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    public class MeanLengthShortestWords : BaseAttribute
    {
        public override void CalculateValue(Document doc)
        {
            var paragraphUniqueWords = doc.UniqueWords[ParagraphIndex];
            {
                var mostCommonWords =
                    paragraphUniqueWords.OrderBy(x => x.Key.Count()).Select(x => x.Key).Take(NumOfWords).ToList();
                float sum = 0;

                MinValue = mostCommonWords.Select(x => x.Count()).Min();
                MaxValue = mostCommonWords.Select(x => x.Count()).Max();

                foreach (var VARIABLE in mostCommonWords)
                {
                    if (MaxValue != MinValue)
                        sum += (VARIABLE.Count() - MaxValue)/(MaxValue - MinValue);
                }

                Value = Math.Abs(sum/NumOfWords);
            }
        }
    }
}
