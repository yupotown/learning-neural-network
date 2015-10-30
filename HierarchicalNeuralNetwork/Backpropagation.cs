using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalNeuralNetwork
{
    class Backpropagation
    {
        public Backpropagation(HierarchicalNetwork network, Func<double, double> diffTransferFunction, double learningRate, double stabilizationRate)
        {
            Network = network;
            DiffTransferFunction = diffTransferFunction;
            LearningRate = learningRate;
            StabilizationRate = stabilizationRate;
            Error = 0.0;

            // allocation
            var layers = Network.LayersCount;
            delta = new double[layers][];
            dw = new double[layers][][];
            dh = new double[layers][];
            delta[0] = new double[0];
            dw[0] = new double[0][];
            dh[0] = new double[0];
            var prevCnt = Network.GetNeuronsCount(0);
            for (var layer = 1; layer < layers; ++layer)
            {
                var cnt = Network.GetNeuronsCount(layer);
                delta[layer] = new double[cnt];
                dw[layer] = new double[cnt][];
                dh[layer] = new double[cnt];
                for (var i = 0; i < cnt; ++i)
                {
                    delta[layer][i] = 0.0;
                    dw[layer][i] = new double[prevCnt];
                    dh[layer][i] = 0.0;
                    for (var j = 0; j < prevCnt; ++j)
                    {
                        dw[layer][i][j] = 0.0;
                    }
                }
                prevCnt = cnt;
            }
        }

        /// <summary>
        /// 対象とする階層型ニューラルネットワーク
        /// </summary>
        public HierarchicalNetwork Network { get; private set; }

        /// <summary>
        /// 伝達関数の微分
        /// </summary>
        public Func<double, double> DiffTransferFunction { get; private set; }

        /// <summary>
        /// 学習係数
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// 安定化係数
        /// </summary>
        public double StabilizationRate { get; set; }

        /// <summary>
        /// 誤差関数の値
        /// </summary>
        public double Error { get; private set; }

        /// <summary>
        /// 1つの入力とそれに対する教師信号を用いて、1ステップの学習を行う。
        /// 連続で呼び出した場合、前回の重み修正量としきい値修正量が慣性項として利用される。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="supervisor"></param>
        public void Train(double[] input, double[] supervisor)
        {
            var outLayer = Network.LayersCount - 1;
            var inCnt = Network.GetNeuronsCount(0);
            var outCnt = Network.GetNeuronsCount(outLayer);

            // 出力を得る
            Network.SetInputs(input.Select(v => new ConstOutput(v)).ToArray());
            Network.Fire();

            // 誤差関数
            Error = 0.0;
            for (var i = 0; i < outCnt; ++i)
            {
                var d = supervisor[i] - Network[outLayer, i].Output;
                Error += d * d;
            }
            Error /= 2;

            // 出力層の学習信号
            for (var i = 0; i < outCnt; ++i)
            {
                var neuron = Network[outLayer, i];
                delta[outLayer][i] = df(neuron.InnerState + neuron.Threshold) * (supervisor[i] - neuron.Output);
            }

            // 隠れ層の学習信号
            for (var layer = outLayer - 1; layer >= 1; --layer)
            {
                var cnt = Network.GetNeuronsCount(layer);
                var nextCnt = Network.GetNeuronsCount(layer + 1);
                for (var i = 0; i < cnt; ++i)
                {
                    var neuron = Network[layer, i];
                    var sum = 0.0;
                    for (var j = 0; j < nextCnt; ++j)
                    {
                        sum += delta[layer + 1][j] * Network[layer + 1, j].Weights[i];
                    }
                    delta[layer][i] = df(neuron.InnerState + neuron.Threshold) * sum;
                }
            }

            // 重みとしきい値の修正
            for (var layer = outLayer; layer >= 1; --layer)
            {
                var cnt = Network.GetNeuronsCount(layer);
                var prevCnt = Network.GetNeuronsCount(layer - 1);
                for (var i = 0; i < cnt; ++i)
                {
                    var neuron = Network[layer, i];
                    for (var j = 0; j < prevCnt; ++j)
                    {
                        dw[layer][i][j] =
                            LearningRate * delta[layer][i] * Network[layer - 1, j].Output
                            + StabilizationRate * dw[layer][i][j];
                        neuron.Weights[j] += dw[layer][i][j];
                    }
                    dh[layer][i] =
                        LearningRate * delta[layer][i]
                        + StabilizationRate * dh[layer][i];
                    neuron.Threshold += dh[layer][i];
                }
            }
        }

        // 学習信号
        private double[][] delta;

        // 重みの修正量
        private double[][][] dw;

        // しきい値の修正量
        private double[][] dh;

        private Func<double, double> df
        {
            get { return DiffTransferFunction; }
        }
    }
}
