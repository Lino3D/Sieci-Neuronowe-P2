﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    public class MeanLengthMostUsed :BaseAttribute
    {
        public override void CalculateValue(Document doc)
        {
            var MostCommonWords = doc.UniqueWords.OrderByDescending(x => x.Value).Take(NumOfWords);
            int sum = MostCommonWords.Sum(x => x.Value);
            Value = (float)sum / NumOfWords;
        }
    }
}