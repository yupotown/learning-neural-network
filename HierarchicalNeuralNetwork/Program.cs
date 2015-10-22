﻿using System;
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
            var nn = new HierarchicalNetwork(2, 2, 1);

            // 学習
            var bp = new Backpropagation(nn, df, 0.8, 0.75);
            var examples = new List<Tuple<double[], double[]>>()
            {
                Tuple.Create(new double[]{0.0, 0.0}, new double[]{0.0}),
                Tuple.Create(new double[]{0.0, 1.0}, new double[]{1.0}),
                Tuple.Create(new double[]{1.0, 0.0}, new double[]{1.0}),
                Tuple.Create(new double[]{1.0, 1.0}, new double[]{0.0}),
            };
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
                var inputs = Console.ReadLine().Split(' ').Select(v => new ConstOutput(double.Parse(v))).ToArray();
                nn.SetInputs(inputs);
                nn.Fire();
                Console.WriteLine("{0:0.0########################################################}", nn[2, 0].Output);
            }
        }
    }
}
