using Folleach.StreamNet.Client;
using MultiplayerSnake.Models.CSModels;
using MultiplayerSnake.Models.SCModels;
using PythonIsGame.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerSnake
{
    public class NetworkMapGenerator : IMapGenerator
    {
        public int ChunkSize { get; private set; }
        private StreamNetClient network;

        private Dictionary<Point, Chunk> cache = new Dictionary<Point, Chunk>();
        private object lockObject = new object();

        private Action<byte[]> a;

        public NetworkMapGenerator(StreamNetClient network, int chunkSize, Action<string> log)
        {
            a = (b) =>
            {
                var data = ChunkData.Unpack(b);
                cache[new Point(data.ChunkObject.ChunkPositionX, data.ChunkObject.ChunkPositionY)] = data.ChunkObject;
            };
            this.network = network;
            ChunkSize = chunkSize;
            network.RegisterSCID(2, a);
        }

        public Chunk Generate(Point chunkPosition)
        {
            if (!cache.ContainsKey(chunkPosition))
            {
                network.Send(2, new ChunkRequestData(chunkPosition.X, chunkPosition.Y));
                cache.Add(chunkPosition, null);
            }
            var result = cache[chunkPosition];
            cache.Remove(chunkPosition);
            return result;
        }
    }
}
