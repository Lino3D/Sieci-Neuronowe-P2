using SNP2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    [Serializable]
    public class DocumentController
    {
        public List<Document> Docs;
        //  private List<Document> Docs2;


        public DocumentController()
        {
           
        }

        public void InitializeDocuments()
        {
            FolderResourceProvider provider = new FolderResourceProvider("./Resource /", "rofl");
            Docs = new List<Document>();
            //    Docs2 = new List<Document>();
            foreach (var docString in provider.FirstFolderDocuments)
            {
                Docs.Add(new Document(docString));
            }
            //foreach (var docString in provider.SecondFolderDocuments)
            //{
            //    Docs2.Add(new Document(docString));
            //}

            //       Docs1.ForEach(x => x.CalculateAttributes());
            //  Docs2.ForEach(x => x.CalculateAttributes());
            Docs.ForEach(x => x.CalculateParagraphAttributes());

            Docs.ForEach(x => x.CalculateMeanAttributes());
        }

        public void CalculateDocumentsDeviation()
        {
            List<float> distances = new List<float>();
            foreach (var document in Docs)
            {
                distances.Add(CalculateMaxDistance(document));
            }

            string domek ="Value of most deviated: " + distances.OrderByDescending(x => x).FirstOrDefault().ToString();
        }

        private float CalculateMaxDistance(Document doc)
        {
            List<float> ResultList = new List<float>();
            foreach (var paragraph in doc.ParagraphAttributesList)
            {
                ResultList.Add(CalculateParagraphDistance(paragraph, doc.MeanAttributes));                
            }


            // Standard Deviation
            doc.StandardDeviation = ResultList.Select(x => x * x).Sum()/ResultList.Count();

            var ret = ResultList.OrderByDescending(x => x).FirstOrDefault();
            doc.MostDeviationValue = ret;
            doc.MostDeviatedParagraphNumber = ResultList.IndexOf(ret);
            return ret;
        }                
        

        private float CalculateParagraphDistance(List<IAttribute> atts, Dictionary<Type, float> dict)
        {
            float result = 0;
            float outValue;
            foreach (var item in atts)
            {
                if (!dict.TryGetValue(item.GetType(), out outValue))
                {
                    throw new Exception();
                }
                result += (item.GetValue() - outValue) * (item.GetValue() - outValue);
            }
            return  (float) Math.Sqrt(result) ;
        }
    }
}
