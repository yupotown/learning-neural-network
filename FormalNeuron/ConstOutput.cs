using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class ConstOutput<T> : ILazyOutput<T>
    {
        public ConstOutput(T value)
        {
            val = value;
        }

        public void Set(T value)
        {
            val = value;
        }

        public T Output
        {
            get { return val; }
        }

        private T val;
    }

    public class ConstOutput
    {
        public static ConstOutput<T> Create<T>(T value)
        {
            return new ConstOutput<T>(value);
        }

        public static ConstOutput<T>[] CreateArray<T>(params T[] value)
        {
            return value.Select(v => ConstOutput.Create(v)).ToArray();
        }
    }
}
