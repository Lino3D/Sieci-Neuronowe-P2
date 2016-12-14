using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SNP2.Clustering
{
   public static class Algorithm
	{
		public static List<Cluster> InitializeClusters (string document)
		{

			char[] delimiters = new char[] { '\r', '\n', ' ', '\t' };
			string[] w = document.Split(delimiters,
					 StringSplitOptions.RemoveEmptyEntries);
			int id=0;
			List<Cluster> clusters = new List<Cluster>();

			foreach (string s in w)
			{
			  
				List<string> tmp= new List<string>();
				tmp.Add(s);
				Cluster c = new Cluster(id, s.Length, tmp);
				clusters.Add(c);
				id++;
			   
			}
			return clusters;

		}

	   public static List<Cluster> Cluster(List<Cluster> clusters, int constant)
		{
	  
		   int distance=0;
		   while(clusters.Count!=1)
		   {
			   for (int i = 0; i < clusters.Count; i++)
			   {
				   int MinLength = Int32.MaxValue;
				   int MinCluster = 0;
				   string str1 = clusters[i].Contents.First();

				   for (int j = 0; j < clusters.Count; j++)
				   {
					   if (i != j)
					   {
						   distance = CalculateLevenshtein(clusters[j].Contents.First(), str1);
						   if (distance < MinLength)
						   {
							   MinLength = distance;
							   MinCluster = j;
						   }
						  

					   }
				   }
				   clusters[i] = MergeClusters(clusters[i], clusters[MinCluster]);
				   clusters.RemoveAt(MinCluster);
				   //jesli najmniejszy znaleziony dystans miedzy clusterami jest wiekszy niz nasz constant, to przerywamy liczenie.
				   if (MinLength > constant)
					   return clusters;
			   }
		   }

		   //Calculate Centroids
			return clusters; 
		}
	   public static Cluster MergeClusters(Cluster c1, Cluster c2)
	   {
		   foreach ( string t in c2.Contents)
				c1.Contents.Add( t );
		   return c1;
	   }


	   public static List<Cluster> KMeans(List<Cluster> clusters)
	   {
		   //Variable is a centroid for each Cluster in Clusters
		   int[] centroid = new int[clusters.Count()];

		   List<Cluster> newClusters = new List<Cluster>();
		   List<string> wordContainer = new List<string>();

		   FindCentroid(clusters);
		   GiveNewIds(clusters);
		   bool IsTheSame = false;
		   int counter = 0;



		   while (!IsTheSame)
		   {
			   counter = 0;
			   foreach (var cluster in clusters)
				   if (cluster.Centroid == cluster.Contents[0])
				   {
				   
					   counter++;
				   }
			   if( counter == clusters.Count())
				   IsTheSame = true;
			   if (!IsTheSame)
			   {
				   newClusters = new List<Cluster>();
				   wordContainer = new List<string>();
				   int id = 0;
				   foreach (var cluster in clusters)
				   {
					   Cluster c = new Cluster(id, cluster.Centroid);
					   id++;
					   newClusters.Add(c);
					   for (int i = 0; i < cluster.Contents.Count(); i++)
						   if (cluster.Contents[i] != cluster.Centroid)
							   wordContainer.Add(cluster.Contents[i]);
				   }

				   CalculateClosests(newClusters, wordContainer);
				   clusters.Clear();
				   clusters = newClusters;
				   FindCentroid(clusters);
			   }

		   }


		   return clusters;
	   }

	   public static void CalculateClosests(List<Cluster> clusters, List<string> words)
	   {
		   while (words.Count() != 0)
		   {
			   foreach (var c in clusters)
			   {
				   if (words.Count() != 0)
				   {
					   int MinLen = int.MaxValue;
					   int index = 0;
					   for (int i = 0; i < words.Count(); i++)
					   {
						   if (MinLen > CalculateLevenshtein(c.Contents[0], words[i]))
						   {
							   MinLen = CalculateLevenshtein(c.Contents[0], words[i]);
							   index = i;
						   }

					   }

					   c.AddToCluster(words[index]);
					   words.RemoveAt(index);
				   }
			   }
		   }
	   }


	   public static void GiveNewIds(List<Cluster>clusters)
	   {
		   int i = 1;
		   foreach(Cluster c in clusters)
		   {
			   c.SetId(i);
			   i++;
		   }
	   }

	   public static void FindCentroid(List<Cluster> clusters)
	   {
		   int sum = 0;
		   int value;
		   foreach (var item in clusters)
		   {
			   item.CalculateVectorSpace();
			   sum = 0;
			   for (int i = 0; i < item.Vector.Count(); i++)
			   {
				   sum += item.Vector[i];
				   item.mean = sum / item.Vector.Count();
			   }

			   //Find the word close to the mean value [centroid]
			   value = (int)item.mean;
			   int original = value;
			   bool lastadded = true;
			   int addition = 1;

			   for (int i = 0; i < clusters.Count; i++)
			   {
				   for (int j = 0; j < clusters[i].Contents.Count(); j++)
				   {
					   string tmp = clusters[i].Contents[j];
					   tmp = Regex.Replace(tmp, "[^0-9a-zA-Z]+", "");
					   clusters[i].Contents[j] = tmp;

				   }

			   }
			   while (true)
			   {
				   int tmp2 = -1;

				   for (int i = 0; i < item.Vector.Count(); i++)
				   {
					   if (item.Vector[i] == value)
					   {
						   tmp2 = i;
						   break;
					   }
				   }
				   if (tmp2 != -1)
				   {
					   item.Centroid = item.Contents.ElementAt(tmp2);
					   break;
				   }
				   if (lastadded)
				   {
					   value = original + addition;
					   lastadded = false;
				   }
				   else
				   {
					   value = original - addition;
					   lastadded = true;
					   addition++;
				   }
			   }
		   }
	   }

	   public static int CalculateLevenshtein(string a, string b)
		{
			int n = a.Length;
			int m = b.Length;
			int[,] d = new int[n + 1, m + 1];

			// Step 1
			if (n == 0)
				return m;
			if (m == 0)
				return n;
			// Step 2
			for (int i = 0; i <= n; d[i, 0] = i++);
			for (int j = 0; j <= m; d[0, j] = j++);

			// Step 3
			for (int i = 1; i <= n; i++)
			{
				//Step 4
				for (int j = 1; j <= m; j++)
				{
					// Step 5
					int cost = (b[j - 1] == a[i - 1]) ? 0 : 1;

					// Step 6
					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}
			// Step 7
			return d[n, m];
		}
	}
}
