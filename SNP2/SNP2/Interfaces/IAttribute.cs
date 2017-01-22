using SNP2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Interfaces
{
    public interface IAttribute
    {
        void CalculateValue(Document doc1, Document doc2);
        void CalculateValue(Document doc);
        float GetValue();

        void SetIndex(int index);

    }
}
