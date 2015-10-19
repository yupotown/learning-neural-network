using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class FormalNeuron : ILazyOutput<bool>
    {
        public FormalNeuron(int inputCount)
        {
            Weights = new ILazyOutput<double>[inputCount];
            Inputs = new ILazyOutput<bool>[inputCount];
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

                var newWeights = new ILazyOutput<double>[value];
                var newInputs = new ILazyOutput<bool>[value];
                for (var i = 0; i < value; ++i)
                {
                    if (i < Weights.Length)
                    {
                        newWeights[i] = Weights[i];
                        newInputs[i] = Inputs[i];
                    }
                    else
                    {
                        newWeights[i] = new ConstOutput<double>(1.0);
                        newInputs[i] = new ConstOutput<bool>(false);
                    }
                }
            }
        }

        /// <summary>
        /// 入力の重み
        /// </summary>
        public ILazyOutput<double>[] Weights { get; private set; }

        /// <summary>
        /// 入力
        /// </summary>
        public ILazyOutput<bool>[] Inputs { get; private set; }

        /// <summary>
        /// 閾値
        /// </summary>
        public ILazyOutput<double> Threshold { get; set; }

        /// <summary>
        /// 出力
        /// </summary>
        public bool Output
        {
            get
            {
                Debug.Assert(Weights.Length == Inputs.Length);

                var res = 0.0;
                for (var i = 0; i < Inputs.Length; ++i)
                {
                    res += Inputs[i].Output ? Weights[i].Output : 0.0;
                }
                return res >= Threshold.Output;
            }
        }

        /// <summary>
        /// 入力の重みを設定する。
        /// </summary>
        /// <param name="weights"></param>
        public void SetWeights(params ILazyOutput<double>[] weights)
        {
            Contract.Requires(weights.Length >= InputCount);

            Array.Copy(weights, Weights, Weights.Length);
        }

        /// <summary>
        /// 入力を設定する。
        /// </summary>
        /// <param name="inputs"></param>
        public void SetInputs(params ILazyOutput<bool>[] inputs)
        {
            Contract.Requires(inputs.Length >= InputCount);

            Array.Copy(inputs, Inputs, Inputs.Length);
        }
    }
}
