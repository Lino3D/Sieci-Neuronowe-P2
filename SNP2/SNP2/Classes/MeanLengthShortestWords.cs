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
            var MostCommonWords = doc.UniqueWords.OrderBy(x => x.Key.Count()).Take(NumOfWords);
            int sum = MostCommonWords.Sum(x => x.Key.Count());
            Value = (float) sum / NumOfWords;
        }
    }
}
