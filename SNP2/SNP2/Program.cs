using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FANNCSharp;
using FANNCSharp.Float;
using static SNP2.ProgramController;

namespace SNP2
{
    class Program
    {


        static void Main(string[] args)
        {
            ProgramController controller = new ProgramController();
            controller.InitializeSimpleNN();

            int MinDistance = 3;
            controller.InitializeKMeansTest(new Mixed(), false, MinDistance);
            controller.InitializeKMeansTest(new Zwierzakowo(), false, MinDistance);
            controller.InitializeKMeansTest(new Plagiarized(), false, MinDistance);
            controller.InitializeKMeansTest(new Balladyna(), false, MinDistance);

            Console.ReadLine();
        }




    }
}
