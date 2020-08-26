using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralAI.AI
{
    public class Neuron
    {
        public int Bias { get; set; }

        public Neuron(int bias)
        {
            Bias = bias;
        }

        public float Process(float[] data, float[] weights)
        {
            float result = 0;
            for (int i = 0; i < weights.Length; ++i)
                result += data[i] * weights[i];
            return result >= Bias ? 1 : 0;
        }

        public override string ToString()
        {
            return $"Neuron: {Bias}";
        }
    }
}
