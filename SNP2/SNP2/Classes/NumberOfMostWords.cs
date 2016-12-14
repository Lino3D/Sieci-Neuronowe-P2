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
            var MostCommonWords = doc.UniqueWords.OrderByDescending(x => x.Value).Take(NumOfWords);
            Value = MostCommonWords.Sum(x => x.Value);
        }

    }
}
