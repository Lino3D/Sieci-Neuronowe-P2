using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    public class MultipleUsageOfIdenticalWords : BaseAttribute
    {

        public override void CalculateValue(Document doc1, Document doc2)
        {
            var mostCommonWords1 = doc1.UniqueWords.OrderByDescending(x => x.Key.Count()).Take(NumOfWords).Select(x => x.Key);
            var mostCommonWords2 = doc2.UniqueWords.OrderByDescending(x => x.Key.Count()).Take(NumOfWords).Select(x => x.Key);

            float identicalCount = mostCommonWords1.Select(x => mostCommonWords2.Contains(x)).Count();

            Value = identicalCount/((doc1.Text.Length + doc2.Text.Length));
        }
    }
}
