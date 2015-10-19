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

            // not
            var notNeuron = new FormalNeuron(1);
            notNeuron.SetInputs(inputs);
            notNeuron.SetWeights(ConstOutput.CreateArray(-1.0));
            notNeuron.Threshold = ConstOutput.Create(-0.5);

            // and
            var andNeuron = new FormalNeuron(2);
            andNeuron.SetInputs(inputs);
            andNeuron.SetWeights(ConstOutput.CreateArray(1.0, 1.0));
            andNeuron.Threshold = ConstOutput.Create(1.5);

            // nand
            var nandNeuron = new FormalNeuron(2);
            nandNeuron.SetInputs(inputs);
            nandNeuron.SetWeights(ConstOutput.CreateArray(-1.0, -1.0));
            nandNeuron.Threshold = ConstOutput.Create(-1.5);

            // or
            var orNeuron = new FormalNeuron(2);
            orNeuron.SetInputs(inputs);
            orNeuron.SetWeights(ConstOutput.CreateArray(1.0, 1.0));
            orNeuron.Threshold = ConstOutput.Create(0.5);

            // nor
            var norNeuron = new FormalNeuron(2);
            norNeuron.SetInputs(inputs);
            norNeuron.SetWeights(ConstOutput.CreateArray(-1.0, -1.0));
            norNeuron.Threshold = ConstOutput.Create(-0.5);

            // xor
            var xorNeuron = new FormalNeuron(2);
            var xorAndNeuron = new FormalNeuron(2);
            var xorOrNeuron = new FormalNeuron(2);
            xorAndNeuron.SetInputs(inputs);
            xorAndNeuron.SetWeights(ConstOutput.CreateArray(1.0, 1.0));
            xorAndNeuron.Threshold = ConstOutput.Create(1.5);
            xorOrNeuron.SetInputs(inputs);
            xorOrNeuron.SetWeights(ConstOutput.CreateArray(1.0, 1.0));
            xorOrNeuron.Threshold = ConstOutput.Create(0.5);
            xorNeuron.SetInputs(xorAndNeuron, xorOrNeuron);
            xorNeuron.SetWeights(ConstOutput.CreateArray(-1.0, 1.0));
            xorNeuron.Threshold = ConstOutput.Create(0.5);

            // names
            var names = new Dictionary<string, ILazyOutput<bool>>();
            names.Add("not", notNeuron);
            names.Add("and", andNeuron);
            names.Add("nand", nandNeuron);
            names.Add("or", orNeuron);
            names.Add("nor", norNeuron);
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
