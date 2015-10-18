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
            // input
            var inputs = ConstOutput.CreateArray(false, false);

            // and
            var andNeuron = new FormalNeuron(2);
            andNeuron.SetInputs(inputs);
            andNeuron.SetWeights(ConstOutput.CreateArray(1.0, 1.0));
            andNeuron.Threshold = ConstOutput.Create(1.5);

            // xor
            var xorNeuron = new FormalNeuron(2);
            var xorNeuron1 = new FormalNeuron(2);
            var xorNeuronThreshold = FunctionNeuron.Create((bool v) => v ? 2.5 : 0.5);
            xorNeuron.SetInputs(inputs);
            xorNeuron.SetWeights(ConstOutput.CreateArray(1.0, 1.0));
            xorNeuron.Threshold = xorNeuronThreshold;
            xorNeuronThreshold.Input = xorNeuron1;
            xorNeuron1.SetInputs(inputs);
            xorNeuron1.SetWeights(ConstOutput.CreateArray(1.0, 1.0));
            xorNeuron1.Threshold = ConstOutput.Create(1.5);

            // names
            var names = new Dictionary<string, ILazyOutput<bool>>();
            names.Add("and", andNeuron);
            names.Add("xor", xorNeuron);

            while (true)
            {
                var line = Console.ReadLine().Split(' ').ToList();
                var name = line[0];
                var prms = line.Skip(1).Select(v => v != "0").ToList();
                for (var i = 0; i < prms.Count; ++i)
                {
                    inputs[i].Set(prms[i]);
                }

                if (name == "quit")
                {
                    break;
                }
                else if (names.ContainsKey(name))
                {
                    Console.WriteLine(names[name].Output ? 1 : 0);
                }
                else
                {
                    Console.Error.WriteLine("unknown name: {0}", name);
                }
            }
        }
    }
}
