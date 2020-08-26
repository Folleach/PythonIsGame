using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralAI.AI
{
    public class NeuralNetwork<TResult>
    {
        public int LayersCount { get; private set; }

        private Layer<TResult> sensors;
        private Layer<TResult> responsers;

        private float trackMultipler;

        private Func<float[], TResult> converter;
        
        public NeuralNetwork(Func<float[], TResult> converter, int[] layers, float trackMultipler)
        {
            this.converter = converter;
            this.trackMultipler = trackMultipler;
            LayersCount = layers.Length;
            Layer<TResult> current = sensors = new Layer<TResult>(this, layers[0]);
            for (int i = 1; i < layers.Length; ++i)
            {
                Layer<TResult> temp = new Layer<TResult>(this, layers[i]);
                current.SetNext(temp);
                current = temp;
            }
            responsers = current;
        }

        public TResult Process(float[] data)
        {
            sensors.ClearTrack();
            if (data.Length != sensors.Size)
                throw new ArgumentException($"Invalid data: Count of data elements must be as sensors count {sensors.Size}");

            return converter(sensors.Process(data));
        }

        public void ProcessTrack(float deltaWeight)
        {
            Debug.WriteLine($"Process backtrack as {deltaWeight}");
            responsers.ProcessTack(deltaWeight * trackMultipler);
        }

        public override string ToString()
        {
            return $"Network: {LayersCount} layers";
        }
    }
}
