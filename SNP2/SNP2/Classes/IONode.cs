using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2.Classes
{
    public class IONode
    {
        public double [] input;
        public double [] output;

        public IONode(List<float> _input, float expectedOutput)
        {
            input = _input.Select(x=> (double) x).ToArray();
            output = new double[] { (double)expectedOutput };
        }
    }
}
