using PythonIsGame.Common.Materials;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Common.Map
{
    public class FrameMapGenerator : IMapGenerator
    {
        public int ChunkSize { get; }

        public FrameMapGenerator(int chunkSize)
        {
            ChunkSize = chunkSize;
        }

        public Chunk Generate(Point chunkPosition)
        {
            Debug.WriteLine($"Generate chunk on '{chunkPosition}'");
            var chunk = new Chunk(chunkPosition.X, chunkPosition.Y, ChunkSize);
            chunk.SetMaterial(new Point(0, 0), new WallMaterial());
            chunk.SetMaterial(new Point(ChunkSize - 1, 0), new WallMaterial());
            chunk.SetMaterial(new Point(0, ChunkSize - 1), new WallMaterial());
            chunk.SetMaterial(new Point(ChunkSize - 1, ChunkSize - 1), new WallMaterial());
            return chunk;
        }
    }
}
