using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalNeuralNetwork
{
    class HierarchicalNetwork
    {
        public HierarchicalNetwork(params int[] neuronsCount)
        {
            ncnt = new int[neuronsCount.Length];
            Array.Copy(neuronsCount, ncnt, ncnt.Length);

            // neurons
            neurons = new Neuron[ncnt.Length][];
            for (var i = 0; i < ncnt.Length; ++i)
            {
                neurons[i] = new Neuron[ncnt[i]];
                for (var j = 0; j < ncnt[i]; ++j)
                {
                    if (i == 0)
                    {
                        neurons[i][j] = new Neuron(1);
                        neurons[i][j].SetWeights(1.0);
                        neurons[i][j].Threshold = 0.0;
                        neurons[i][j].TransferFunction = (v => v);
                    }
                    else
                    {
                        neurons[i][j] = new Neuron(ncnt[i - 1]);
                        for (var k = 0; k < ncnt[i - 1]; ++k)
                        {
                            neurons[i][j].Inputs[k] = neurons[i - 1][k];
                            neurons[i][j].Weights[k] = (rnd.NextDouble() - 0.5) / 5.0;
                        }
                        neurons[i][j].Threshold = (rnd.NextDouble() - 0.5) / 5.0;
                    }
                }
            }
        }

        /// <summary>
        /// 第 layer 層の index 番目のニューロン
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Neuron this[int layer, int index]
        {
            get
            {
                return neurons[layer][index];
            }
            set
            {
                if (layer >= 1)
                {
                    if (value.InputCount < ncnt[layer - 1])
                    {
                        throw new ArgumentException();
                    }
                    for (var i = 0; i < ncnt[layer - 1]; ++i)
                    {
                        value.Inputs[i] = neurons[layer - 1][i];
                    }
                }
                neurons[layer][index] = value;
            }
        }

        /// <summary>
        /// 階層数
        /// </summary>
        public int LayersCount
        {
            get
            {
                return ncnt.Length;
            }
        }

        /// <summary>
        /// 各階層のニューロンの数を取得する。
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public int GetNeuronsCount(int layer)
        {
            return ncnt[layer];
        }

        /// <summary>
        /// 入力の次元
        /// </summary>
        public int InputDimention { get { return GetNeuronsCount(0); } }

        /// <summary>
        /// 出力の次元
        /// </summary>
        public int OutputDimention { get { return GetNeuronsCount(LayersCount - 1); } }

        /// <summary>
        /// 入力層に与える入力を設定する。
        /// </summary>
        /// <param name="inputs"></param>
        public void SetInputs(params IOutput[] inputs)
        {
            for (var i = 0; i < ncnt[0]; ++i)
            {
                neurons[0][i].Inputs[0] = inputs[i];
            }
        }

        /// <summary>
        /// 出力層の出力を取得する。
        /// </summary>
        /// <returns></returns>
        public double[] GetOutputs()
        {
            var lastLayer = ncnt.Length - 1;
            var res = new double[ncnt[lastLayer]];
            for (var i = 0; i < ncnt[ncnt.Length - 1]; ++i)
            {
                res[i] = neurons[lastLayer][i].Output;
            }
            return res;
        }

        /// <summary>
        /// 発火する。
        /// </summary>
        public void Fire()
        {
            for (var layer = 0; layer < ncnt.Length; ++layer)
            {
                for (var i = 0; i < ncnt[layer]; ++i)
                {
                    neurons[layer][i].Fire();
                }
            }
        }

        private int[] ncnt;
        private Neuron[][] neurons;
        private Random rnd = new Random();
    }
}
