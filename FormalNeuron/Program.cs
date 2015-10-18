using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            // and
            var andNeuron = new FormalNeuron(2);
            andNeuron.SetWeights(ConstOutput.CreateArray(1.0, 1.0));
            andNeuron.Threshold = ConstOutput.Create(1.5);

            while (true)
            {
                var input = Console.ReadLine().Split(' ').ToArray();
                var name = input[0];
                var prms = input.Skip(1).Select(v => v != "0").ToArray();

                if (name == "quit")
                {
                    break;
                }
                else if (name == "and")
                {
                    andNeuron.SetInputs(ConstOutput.CreateArray(prms));
                    Console.WriteLine(andNeuron.Output ? 1 : 0);
                }
                else
                {
                    Console.Error.WriteLine("unknown name: {0}", name);
                }
            }
        }
    }
}
