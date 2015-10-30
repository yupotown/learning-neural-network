using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class ConstOutput : IOutput
    {
        public ConstOutput(double value)
        {
            val = value;
        }

        public void Set(double value)
        {
            val = value;
        }

        public double Output { get { return val; } }

        private double val;
    }
}
