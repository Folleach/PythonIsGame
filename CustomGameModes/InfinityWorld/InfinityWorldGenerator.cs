using PythonIsGame.Common;
using PythonIsGame.Common.Materials;
using System;
using System.Drawing;

namespace CustomGameModes.InfinityWorld
{
    public class InfinityWorldGenerator : IMapGenerator
    {
        public int ChunkSize { get; } = 32;
        public int Seed { get; }
        private Random random;
        private static readonly int wallThreshold = int.MaxValue / 100;
        private static readonly int appleThreshold = int.MaxValue / 5000;

        public InfinityWorldGenerator(int seed)
        {
            Seed = seed;
        }

        public Chunk Generate(Point chunkPosition)
        {
            random = new Random(unchecked(Seed * chunkPosition.GetHashCode()));
            var chunk = new Chunk(chunkPosition.X, chunkPosition.Y, ChunkSize);
            for (var x = 0; x < ChunkSize; x++)
            {
                for (var y = 0; y < ChunkSize; y++)
                {
                    if (random.Next() < wallThreshold)
                        chunk.SetMaterial(new Point(x, y), new WallMaterial());
                    if (random.Next() < appleThreshold)
                        chunk.SetMaterial(new Point(x, y), new AppleMaterial());
                }
            }
            return chunk;
        }
    }
}
