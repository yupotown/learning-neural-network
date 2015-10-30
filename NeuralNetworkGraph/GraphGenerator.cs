using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Graph
{
    public class GraphGenerator
    {
        public GraphGenerator()
        {
        }

        public void Generate(StreamWriter writer, HierarchicalNetwork nn, string name = "hierarchical_nn")
        {
            writer.WriteLine("digraph {0} {{", name);
            writer.WriteLine("\tgraph [rankdir=LR, ranksep=1.0];");

            // subgraph in
            writer.WriteLine("\tsubgraph in {");
            writer.WriteLine("\t\tcolor=gray;");
            writer.WriteLine("\t\tnode[style=invis, fixedsize=true, width=0, height=0];");
            writer.WriteLine("\t\t{0};", string.Join(", ",
                Enumerable.Range(0, nn.InputDimention)
                .Select(i => string.Format("i{0}", i + 1))
                ));
            writer.WriteLine("\t}");

            // clusters
            for (var i = 0; i < nn.LayersCount; ++i)
            {
                writer.WriteLine("\tsubgraph cluster{0} {{", i + 1);
                writer.WriteLine("\t\tcolor=gray;");
                if (i == 0)
                {
                    writer.WriteLine("\t\tnode [label=\"\"]");
                    writer.WriteLine("\t\t{0};", string.Join(", ",
                        Enumerable.Range(0, nn.GetNeuronsCount(i))
                        .Select(j => string.Format("n{0}{1}", i + 1, j + 1))
                        ));
                }
                else
                {
                    for (var j = 0; j < nn.GetNeuronsCount(i);  ++j)
                    {
                        writer.WriteLine("\t\tn{0}{1} [label=\"{2:0.00}\"];", i + 1, j + 1, -nn[i, j].Threshold);
                    }
                }
                writer.WriteLine("\t}");
            }

            // subgraph out
            writer.WriteLine("\tsubgraph out {");
            writer.WriteLine("\t\tnode[style=invis, fixedsize=true, width=0, height=0]");
            writer.WriteLine("\t\t{0};", string.Join(", ",
                Enumerable.Range(0, nn.OutputDimention)
                .Select(i => string.Format("o{0}", i + 1))
                ));
            writer.WriteLine("\t}");

            // i? -> n1?
            for (var i = 0; i < nn.InputDimention; ++i)
            {
                writer.WriteLine("\ti{0} -> n1{0} [arrowsize=0.6];", i + 1);
            }

            // n?? -> n??
            for (var i = 0; i < nn.LayersCount - 1; ++i)
            {
                for (var j = 0; j < nn.GetNeuronsCount(i); ++j)
                {
                    for (var k = 0; k < nn.GetNeuronsCount(i + 1); ++k)
                    {
                        if (k == 0)
                        {
                            writer.WriteLine("\tn{0}{1} -> n{2}{3} [taillabel=\"{4:0.00}\", headlabel=\"{5:0.00}\", arrowsize=0.6];",
                                i + 1, j + 1, i + 2, k + 1, nn[i, j].Output, nn[i + 1, k].Weights[j]);
                        }
                        else
                        {
                            writer.WriteLine("\tn{0}{1} -> n{2}{3} [headlabel=\"{4:0.00}\", arrowsize=0.6];",
                                i + 1, j + 1, i + 2, k + 1, nn[i + 1, k].Weights[j]);
                        }
                    }
                }
            }

            // n?? -> o?
            for (var i = 0; i < nn.OutputDimention; ++i)
            {
                writer.WriteLine("\tn{0}{1} -> o{1} [taillabel=\"{2:0.00}\", arrowsize=0.6];",
                    nn.LayersCount, i + 1, nn[nn.LayersCount - 1, i].Output);
            }

            writer.WriteLine("}");
        }
    }
}
