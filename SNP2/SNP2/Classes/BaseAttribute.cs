using SNP2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    public abstract class BaseAttribute : IAttribute
    {
        public float Value;
        public int NumOfWords = 5;

        public float GetValue()
        {
            return Value;
        }
        public virtual void CalculateValue(Document doc)
        {
            return;
        }        

        public virtual void CalculateValue(Document doc1, Document doc2)
        {
            return;
        }
        
    }
}
