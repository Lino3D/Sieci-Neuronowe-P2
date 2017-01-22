using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    public class MeanLengthLeastUsed : BaseAttribute
    {
        public override void CalculateValue(Document doc)
        {
            //foreach (var paragraphUniqueWords in doc.UniqueWords)
            var paragraphUniqueWords = doc.UniqueWords[ParagraphIndex];
            {
                var mostCommonWords = paragraphUniqueWords.OrderBy(x => x.Value).Take(5).Select(x => x.Value).ToList();
                float sum = 0;
                MinValue = mostCommonWords.Min();
                MaxValue = mostCommonWords.Max();

                foreach (var VARIABLE in mostCommonWords)
                {
                    if (MaxValue != MinValue)
                        sum += (VARIABLE - MaxValue)/(MaxValue - MinValue);
                }

                Value = Math.Abs(sum/NumOfWords);
            }
        }

    }
}
