using FANNCSharp;
using FANNCSharp.Double;
using SNP2.Classes;
using SNP2.Clustering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SNP2.Interfaces;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Runtime.Serialization;

namespace SNP2
{
    public class ProgramController
    {
        string WorkText1, WorkText2;
        List<Cluster> ClustersText1;
        List<Cluster> ClustersText2;
        int MinDistance = 7;
        DocumentController docControl;
        List<IONode> nodes;
        float PlagiarismThreshold = 0.5f;

        public void InitializeDocument()
        {
          
            bool Okay = true;
            FileStream fs2 = new FileStream(@"C:\MyTemp\nodes.dat", FileMode.Open);
            try
            {
                BinaryFormatter formatter2 = new BinaryFormatter();
                nodes = (List<IONode>)formatter2.Deserialize(fs2);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                Okay = false;
            }
            finally
            {
                fs2.Close();
            }
            if (Okay == false)
            {
                docControl = new DocumentController();
                docControl.InitializeDocuments();
                docControl.CalculateDocumentsDeviation();
                CreateNodes();

                FileStream fs = new FileStream(@"C:\MyTemp\nodes.dat", FileMode.Create);
                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, nodes);
                }
                catch (Exception e)
                {

                }
                finally
                {
                    fs.Close();
                }
            }




          

        }

        public void CreateNodes()
        {
            nodes = new List<IONode>();
            float isPlagiarised;
            List<float> Input;
            int numOfPredictedPlagiarised = 0;
            foreach (var item in docControl.Docs)
            {
                if (item.MostDeviationValue > PlagiarismThreshold)
                {
                    isPlagiarised = 1.0f;
                    numOfPredictedPlagiarised++;
                }
                else
                    isPlagiarised = 0.0f;
                Input = new List<float>();
                Input.AddRange(item.MeanAttributes.Select(x => x.Value));
                Input.AddRange(item.ParagraphAttributesList.ElementAt(item.MostDeviatedParagraphNumber).Select(x => x.GetValue()).ToList());
                IONode node = new IONode(Input, isPlagiarised);
                nodes.Add(node);
            }
            string domek = "% przewidzianych przez nas jako plagiaryzmy: " + (((float)numOfPredictedPlagiarised / (float)docControl.Docs.Count()) * 100) + "%";
        }

        public void InitializeNNForDocuments()
        {
            // self organizing map
            uint[] layers = { (uint)nodes.FirstOrDefault().input.Count(), 5, 1 };
            NeuralNet net = new NeuralNet(NetworkType.LAYER, layers);
            net.RandomizeWeights(0, 1);
            TrainingData td = new TrainingData();

            td.SetTrainData(nodes.Select(x => x.input).ToArray(), nodes.Select(x => x.output).ToArray());

            net.TrainOnData(td, 10000, 1, (float)0.0000001);

            //        net.Save("NN.net");


            var error = net.Test(nodes.FirstOrDefault().input, nodes.FirstOrDefault().output);
            Console.WriteLine(error[0].ToString());
        }

        public void InitializeSimpleNN()
        {
            // result shoudl be 0, ZERO, like not divide by it, im tired...
            double[] testarr = new double[] { 1, 12.2, 11.2, 1.2, 4.361111, 5, 56, 1, 13.4, 25, 1.2, 4.17817, 5, 125 };
            // self organizing map
            uint[] layers = { 14, 3, 1 };
            NeuralNet net = new NeuralNet(NetworkType.LAYER, layers);


            net.RandomizeWeights(0, 1);
            net.TrainOnFile("test.dat", 10000, 1, (float)0.0001);
            net.Save("NN.net");


            var error = net.Test(testarr, new double[] { 0 });
            Console.WriteLine(error[0].ToString());
        }


        #region NotNow!
        public void TryStartSimpleNN()
        {

        }

        public void InitializeDocuments(IResourceProvider resProvider)
        {
            ReadTexts(resProvider);
            Document doc1 = new Document(WorkText1);
            Document doc2 = new Document(WorkText2);

            doc1.CalculateAttributes();
            doc2.CalculateAttributes();

            doc1.CalculateAttributes(doc2);
            doc2.CalculateAttributes(doc1);


            SaveDataToFile save = new SaveDataToFile();
            save.SaveDocument("test", doc1, doc2, false);



            string dupa = "";

        }

        public void InitializeFolder(IResourceProvider resProvider)
        {

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
            Console.WriteLine("Number of clusters in 1st text: " + ClustersText1.Count());
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
                              $"according to Text1: {String.Format("{0:0.00}", value1)}% and to Text2: {String.Format("{0:0.00}", value2)}%");

            if (!printcomparison)
                return;
            int lesserCount = ClustersText1.Count < ClustersText2.Count ? ClustersText1.Count : ClustersText2.Count;

            for (int i = 0; i < lesserCount; i++)
            {
                Console.WriteLine(
                    $"Head 1: {ClustersText1.ElementAt(i).Centroid} \t\t Head 2: {ClustersText2.ElementAt(i).Centroid}");
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

        #endregion

    }
}

#region Old
//        public class Plagiarized : IResourceProvider
//        {
//            public string ResourceFile1
//            {
//                get
//                {
//                    return "./Resource/text1.txt";
//                }

//            }

//            public string ResourceFile2
//            {
//                get
//                {
//                    return "./Resource/text2.txt";
//                }

//            }
//        }

//        public class Zwierzakowo : IResourceProvider
//        {
//            public string ResourceFile1
//            {
//                get
//                {
//                    return "./Resource/zwierzakowo1.txt";
//                }

//            }

//            public string ResourceFile2
//            {
//                get
//                {
//                    return "./Resource/zwierzakowo2.txt";
//                }

//            }
//        }

//        public class Mixed : IResourceProvider
//        {
//            public string ResourceFile1
//            {
//                get
//                {
//                    return "./Resource/Doc1.txt";
//                }

//            }

//            public string ResourceFile2
//            {
//                get
//                {
//                    return "./Resource/Doc2.txt";
//                }

//            }
//        }

//        public class Balladyna : IResourceProvider
//        {
//            public string ResourceFile1
//            {
//                get
//                {
//                    return "./Resource/balladyna1.txt";
//                }

//            }

//            public string ResourceFile2
//            {
//                get
//                {
//                    return "./Resource/balladyna2.txt";
//                }

//            }
//        }
//    }
//}
#endregion