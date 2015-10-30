using NeuralNetwork;
using NeuralNetwork.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkGraphTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // 階層型ニューラルネットワークを構成
            var nn = new HierarchicalNetwork(2, 2, 1);
            nn[1, 0].Weights[0] = 10.0;
            nn[1, 0].Weights[1] = 10.0;
            nn[1, 0].Threshold = -5.0;
            nn[1, 1].Weights[0] = 10.0;
            nn[1, 1].Weights[1] = 10.0;
            nn[1, 1].Threshold = -15.0;
            nn[2, 0].Weights[0] = 10.0;
            nn[2, 0].Weights[1] = -10.0;
            nn[2, 0].Threshold = -5.0;

            // 発火
            nn.SetInputs(new ConstOutput(0.0), new ConstOutput(1.0));
            nn.Fire();

            // グラフを出力
            var gen = new GraphGenerator();
            using (var writer = new StreamWriter("graph.gv"))
            {
                gen.Generate(writer, nn);
            }
        }
    }
}
