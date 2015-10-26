using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalNeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<double, double> f = ((double x) => 1 / (1 + Math.Exp(-x)));
            Func<double, double> df = (x => f(x) * (1 - f(x)));

            // ネットワークの情報読み込み
            HierarchicalNetwork nn;
            var examples = new List<Tuple<double[], double[]>>();

            Console.Error.Write("path >");
            var path = Console.ReadLine();
            using (var reader = new StreamReader(path))
            {
                nn = new HierarchicalNetwork(reader.ReadLine().Split(' ').Select(v => int.Parse(v)).ToArray());
                var exNum = int.Parse(reader.ReadLine());
                for (var i = 0; i < exNum; ++i)
                {
                    var io = reader.ReadLine().Split('|').ToList();
                    examples.Add(Tuple.Create(
                        io[0].Split(' ').Select(v => double.Parse(v)).ToArray(),
                        io[1].Split(' ').Select(v => double.Parse(v)).ToArray()
                        ));
                }
            }

            // 学習
            var bp = new Backpropagation(nn, df, 0.8, 0.75);
            var rnd = new Random();
            for (var i = 0; i < 5000; ++i)
            {
                var ex = examples.OrderBy(v => Guid.NewGuid()).ToArray(); // shuffle
                for (var j = 0; j < 4; ++j)
                {
                    bp.Train(ex[j].Item1, ex[j].Item2);
                }
            }

            while (true)
            {
                Console.Error.Write("input >");
                var inputs = Console.ReadLine().Split(' ').Select(v => new ConstOutput(double.Parse(v))).ToArray();
                nn.SetInputs(inputs);
                nn.Fire();
                foreach (var o in nn.GetOutputs())
                {
                    Console.WriteLine("{0:0.0########################################################}", o);
                }
            }
        }
    }
}
