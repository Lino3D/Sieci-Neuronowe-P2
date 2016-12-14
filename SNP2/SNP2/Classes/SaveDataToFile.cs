using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    public class SaveDataToFile
    {
        public void SaveDocument(string FileName, Document doc1, Document doc2, bool Plagiarised)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            int AttributeCount = doc1.Attirbutes.Count() + doc2.Attirbutes.Count();
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter($"{FileName}.dat",true))
            {
                //    file.WriteLine($"2 {AttributeCount} 1");
                file.WriteLine();
                foreach (var item in doc1.Attirbutes)
                {
                    file.Write(item.GetValue() + " ");
                }
                foreach (var item in doc2.Attirbutes)
                {
                    file.Write(item.GetValue() + " ");
                }
                file.WriteLine();
                file.Write(Plagiarised ? "1" : "0");
                
            }
        }
    }
}
