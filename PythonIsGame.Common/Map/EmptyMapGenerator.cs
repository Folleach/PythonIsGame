using System.Drawing;

namespace PythonIsGame.Common.Map
{
    public class EmptyMapGenerator : IMapGenerator
    {
        public int ChunkSize { get; } = 128;

        public EmptyMapGenerator(int chunkSize = 128)
        {
            ChunkSize = chunkSize;
        }

        public Chunk Generate(Point chunkPosition)
        {
            return new Chunk(chunkPosition.X, chunkPosition.Y, ChunkSize);
        }
    }
}
