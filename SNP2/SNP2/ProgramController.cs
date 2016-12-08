using FANNCSharp;
using FANNCSharp.Double;
using SNP2.Clustering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SNP2
{
    public class ProgramController
    {
        string WorkText1, WorkText2;
        List<Cluster> ClustersText1;
        List<Cluster> ClustersText2;
        int MinDistance = 7;

        public void InitializeSimpleNN()
        {
            NeuralNetwork nn = new NeuralNetwork();
            nn.InitializeNN();
        }

        public void InitializeKMeansTest(IResourceProvider resProvider, bool printcomparison, int min = 5)
        {
            MinDistance = min;
            ReadTexts(resProvider);
            ClustersText1 = Algorithm.InitializeClusters(WorkText1);
            ClustersText1 = Algorithm.Cluster(ClustersText1, MinDistance);
            ClustersText1 = Algorithm.KMeans(ClustersText1);

            ClustersText2 = Algorithm.InitializeClusters(WorkText2);
            ClustersText2 = Algorithm.Cluster(ClustersText2, MinDistance);
            ClustersText2 = Algorithm.KMeans(ClustersText2);

            Console.WriteLine("Provider type: " + resProvider.GetType().Name);
            Console.WriteLine("Number of clusters in 1st text: "+ClustersText1.Count());
            Console.WriteLine("Number of clusters in 2nd text: " + ClustersText2.Count());

            int presentInBoth = 0;
            foreach (var item in ClustersText1)
            {
                if (ClustersText2.FirstOrDefault(x => x.Centroid == item.Centroid) != null)
                    presentInBoth++;
            }
            var value1 = ((double)presentInBoth / (double)ClustersText1.Count) * 100;
            var value2 = ((double)presentInBoth / (double)ClustersText2.Count) * 100;

            Console.WriteLine("Percentage of centroids present in both clusters, " +
                $"according to Text1: {String.Format("{0:0.00}", value1) }% and to Text2: {String.Format("{0:0.00}", value2)}%");

            if (!printcomparison)
                return;
            int lesserCount = ClustersText1.Count < ClustersText2.Count ? ClustersText1.Count : ClustersText2.Count;

            for (int i = 0; i < lesserCount; i++)
            {
                Console.WriteLine($"Head 1: {ClustersText1.ElementAt(i).Centroid} \t\t Head 2: {ClustersText2.ElementAt(i).Centroid}");
            }
            Console.WriteLine("_________________________________________________________________________");

        }


        public void ReadTexts(IResourceProvider resProvider)
        {
            //try
            //{  
            using (StreamReader sr = new StreamReader(resProvider.ResourceFile1))
            {
                WorkText1 = sr.ReadToEnd();
            }
            using (StreamReader sr = new StreamReader(resProvider.ResourceFile2))
            {
                WorkText2 = sr.ReadToEnd();
            }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("The file could not be read:");
            //    Console.WriteLine(e.Message);
            //}
        }

        public class Plagiarized : IResourceProvider
        {
            public string ResourceFile1
            {
                get
                {
                    return "./Resource/text1.txt";
                }
            
            }

            public string ResourceFile2
            {
                get
                {
                    return "./Resource/text2.txt";
                }
                
            }
        }

        public class Zwierzakowo : IResourceProvider
        {
            public string ResourceFile1
            {
                get
                {
                    return "./Resource/zwierzakowo1.txt";
                }

            }

            public string ResourceFile2
            {
                get
                {
                    return "./Resource/zwierzakowo2.txt";
                }

            }
        }

        public class Mixed : IResourceProvider
        {
            public string ResourceFile1
            {
                get
                {
                    return "./Resource/text2.txt";
                }

            }

            public string ResourceFile2
            {
                get
                {
                    return "./Resource/zwierzakowo2.txt";
                }

            }
        }

        public class Balladyna : IResourceProvider
        {
            public string ResourceFile1
            {
                get
                {
                    return "./Resource/balladyna1.txt";
                }

            }

            public string ResourceFile2
            {
                get
                {
                    return "./Resource/balladyna2.txt";
                }

            }
        }
    }
}
