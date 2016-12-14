using SNP2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    public class MeanWordLength : BaseAttribute
    {
        public override void CalculateValue(Document doc)
        {
            int mean = 0;
            foreach (var word in doc.TextWords)
            {
                mean += word.Length;
            }
            
            Value = (float) mean / doc.TextWords.Length;
        }
    }
}
