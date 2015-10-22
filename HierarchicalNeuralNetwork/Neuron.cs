using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalNeuralNetwork
{
    class Neuron : IOutput
    {
        public Neuron(int inputCount)
        {
            Weights = new double[0];
            Inputs = new IOutput[0];
            InputCount = inputCount;
            Threshold = 0.0;
            TransferFunction = ((double v) => 1 / (1 + Math.Exp(-v)));
            InnerState = 0.0;
            Output = 0.0;
        }

        /// <summary>
        /// 発火する。
        /// </summary>
        public void Fire()
        {
            var sum = 0.0;
            for (var i = 0; i < Inputs.Length; ++i)
            {
                sum += Weights[i] * Inputs[i].Output;
            }
            InnerState = sum;
            Output = TransferFunction(InnerState + Threshold);
        }

        /// <summary>
        /// 入力の個数
        /// </summary>
        public int InputCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return Weights.Length;
            }
            set
            {
                Contract.Requires(value >= 0);

                var newWeights = new double[value];
                var newInputs = new IOutput[value];
                for (var i = 0; i < value; ++i)
                {
                    if (i < Weights.Length)
                    {
                        newWeights[i] = Weights[i];
                        newInputs[i] = Inputs[i];
                    }
                    else
                    {
                        newWeights[i] = 1.0;
                        newInputs[i] = new ConstOutput(0.0);
                    }
                }
                Weights = newWeights;
                Inputs = newInputs;
            }
        }

        /// <summary>
        /// 重み
        /// </summary>
        public double[] Weights { get; private set; }

        /// <summary>
        /// 入力
        /// </summary>
        public IOutput[] Inputs { get; private set; }

        /// <summary>
        /// しきい値
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// 伝達関数
        /// </summary>
        public Func<double, double> TransferFunction { get; set; }

        /// <summary>
        /// 内部状態(入力と重みの積の総和)
        /// </summary>
        public double InnerState { get; private set; }

        /// <summary>
        /// 出力
        /// </summary>
        public double Output { get; private set; }

        /// <summary>
        /// 重みを設定する。
        /// </summary>
        /// <param name="weights"></param>
        public void SetWeights(params double[] weights)
        {
            Contract.Requires(weights.Length >= Weights.Length);

            Array.Copy(weights, Weights, Weights.Length);
        }

        /// <summary>
        /// 入力を設定する。
        /// </summary>
        /// <param name="inputs"></param>
        public void SetInputs(params IOutput[] inputs)
        {
            Contract.Requires(inputs.Length >= Inputs.Length);

            Array.Copy(inputs, Inputs, Inputs.Length);
        }
    }
}
