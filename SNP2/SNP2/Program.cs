using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FANNCSharp;
using FANNCSharp.Float;

namespace SNP2
{
    class Program
    {
        static void Main(string[] args)
        {
            uint[] layers = { 2, 3, 1 };
            NeuralNet net = new NeuralNet(NetworkType.LAYER, layers);
            
            net.RandomizeWeights(0, 1);
            net.TrainOnFile("data.dat", 100, 1, (float)0.0001);
            net.Save("NN.net");
            Console.ReadLine();
        }
    }
}
