using SNP2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    [Serializable]
    public class MeanWordLength : BaseAttribute
    {
        public override void CalculateValue(Document doc)
        {
      
           float mean = 0;
            MinValue = doc.TextWords.Select(x => x.Count()).Min();
            MaxValue = doc.TextWords.Select(x => x.Count()).Max();
            foreach (var word in doc.TextWords)
            {
                if (MaxValue != MinValue)
                    mean += (word.Count - MaxValue) / (MaxValue - MinValue);
            }

            mean = (mean - MaxValue) / (MaxValue - MinValue);


            Value = Math.Abs(mean / doc.TextWords.Count);
        }
    }
}
