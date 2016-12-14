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
            var MostCommonWords1 = doc1.UniqueWords.OrderByDescending(x => x.Key.Count()).Take(NumOfWords).Select(x => x.Key);
            var MostCommonWords2 = doc2.UniqueWords.OrderByDescending(x => x.Key.Count()).Take(NumOfWords).Select(x => x.Key);

            int IdenticalCount = MostCommonWords1.Select(x => MostCommonWords2.Contains(x)).Count();

            Value = IdenticalCount;
        }
    }
}
