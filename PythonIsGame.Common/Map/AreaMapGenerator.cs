using PythonIsGame.Common.Materials;
using System.Drawing;

namespace PythonIsGame.Common.Map
{
    public class AreaMapGenerator : IMapGenerator
    {
        public int ChunkSize => 256;

        private int Left;
        private int Up;
        private int Right;
        private int Bottom;

        public AreaMapGenerator(int left, int up, int right, int bottom)
        {
            Left = left;
            Up = up;
            Right = right;
            Bottom = bottom;
        }

        public Chunk Generate(Point chunkPosition)
        {
            var chunk = new Chunk(chunkPosition.X, chunkPosition.Y, ChunkSize);
            for (var x = 0; x < ChunkSize; x++)
            {
                for (var y = 0; y < ChunkSize; y++)
                {
                    var absoluteX = chunkPosition.X * ChunkSize + x;
                    var absoluteY = chunkPosition.Y * ChunkSize + y;
                    if ((absoluteX == Left || absoluteX == Right) && absoluteY >= Up && absoluteY <= Bottom)
                        chunk.SetMaterial(new Point(x, y), new TeleportMaterial(new Point(absoluteX == Left ? Right - 1 : 1, y)));
                    if ((absoluteY == Up || absoluteY == Bottom) && absoluteX > Left && absoluteX < Right)
                        chunk.SetMaterial(new Point(x, y), new TeleportMaterial(new Point(x, absoluteY == Up ? Bottom - 1 : 1)));
                }
            }
            return chunk;
        }
    }
}
