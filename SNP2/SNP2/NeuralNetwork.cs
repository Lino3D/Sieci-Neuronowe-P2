using FANNCSharp;
using FANNCSharp.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2
{
    public class NeuralNetwork
    {
        bool Bias = false;
        int LayerCount = 4;
        int HiddenNeuronsCount = 6;
        object ActivationFunction;
        int MaxIterations;
        double LearningRate = 0.00001;
        double Momentum = 0.00001;
        double MinError = 0.0001;
        public void InitializeNN()
        {
            // self organizing map
            uint[] layers = { 2, 3, 1 };
            NeuralNet net = new NeuralNet(NetworkType.LAYER, layers);

            net.RandomizeWeights(0, 1);
            net.TrainOnFile("data.dat", 1000, 1, (float)MinError);
            net.Save("NN.net");
        }
    }
}
