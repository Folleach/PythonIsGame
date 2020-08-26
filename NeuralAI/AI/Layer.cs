using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralAI.AI
{
    public class Layer<T> : IEnumerable<Neuron>
    {
        private static Random random = new Random();

        public int Size => neurons.Length;

        private NeuralNetwork<T> network;
        private Neuron[] neurons;

        private Layer<T> next;
        private Layer<T> previous;

        // ( left ) <- ( right )
        private float[][] weights;

        private Stack<int> activated = new Stack<int>();
        
        public Layer(NeuralNetwork<T> network, int size)
        {
            this.network = network;
            neurons = new Neuron[size];
            for (int i = 0; i < size; ++i)
                neurons[i] = new Neuron(2);
        }

        public void SetNext(Layer<T> layer)
        {
            next = layer;
            layer.previous = this;
            weights = new float[layer.Size][];
            for (int right = 0; right < layer.Size; ++right)
            {
                weights[right] = new float[Size];
                for (int left = 0; left < Size; ++left)
                {
                    weights[right][left] = (float)(random.NextDouble() * 22);
                }
            }
        }

        public float[] Process(float[] data)
        {
            if (next == null)
                return data;
            float[] result = new float[next.Size];
            for (int i = 0; i < next.Size; ++i)
            {
                Neuron partipant = next[i];
                float neuroResult = partipant.Process(data, weights[i]);
                result[i] = neuroResult;
                if (neuroResult > 0)
                    activated.Push(i);
            }
            return next.Process(result);
        }

        public void ProcessTack(float deltaWeight)
        {
            if (weights != null)
            {
                while (activated.Count != 0)
                {
                    int activ = activated.Pop();
                    for (int i = 0; i < weights[activ].Length; ++i)
                        weights[activ][i] += deltaWeight;
                }
            }
            if (previous != null)
                previous.ProcessTack(deltaWeight);
        }

        public void ClearTrack()
        {
            activated.Clear();
            if (next != null)
                next.ClearTrack();
        }

        public override string ToString()
        {
            return $"Layer: {Size} neurons";
        }

        public Neuron this[int index]
        {
            get => neurons[index];
        }

        public IEnumerator<Neuron> GetEnumerator()
        {
            foreach (var neuron in neurons)
                yield return neuron;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
