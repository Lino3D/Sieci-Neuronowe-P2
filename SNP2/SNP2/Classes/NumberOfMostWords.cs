using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    class NumberOfMostWords : BaseAttribute
    {
        

        public override void CalculateValue(Document doc)
        {
            foreach (var paragraphUniqueWords in doc.UniqueWords)
            {
                var mostCommonWords =
                    paragraphUniqueWords.OrderByDescending(x => x.Value).Select(x => Value).Take(NumOfWords).ToList();

                MinValue = mostCommonWords.Min();
                MaxValue = mostCommonWords.Max();

                Value = (Value - MaxValue)/(MaxValue - MinValue);
                Value = Math.Abs(mostCommonWords.Sum());
            }
        }

    }
}
