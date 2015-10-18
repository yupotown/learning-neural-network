using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class FunctionNeuron<In, Out> : ILazyOutput<Out>
    {
        public FunctionNeuron(Func<In, Out> function)
        {
            f = function;
        }

        /// <summary>
        /// 入力
        /// </summary>
        public ILazyOutput<In> Input { get; set; }

        /// <summary>
        /// 出力
        /// </summary>
        public Out Output
        {
            get { return f(Input.Output); }
        }

        private Func<In, Out> f;
    }

    public class FunctionNeuron
    {
        public static FunctionNeuron<In, Out> Create<In, Out>(Func<In, Out> function)
        {
            return new FunctionNeuron<In, Out>(function);
        }
    }
}
