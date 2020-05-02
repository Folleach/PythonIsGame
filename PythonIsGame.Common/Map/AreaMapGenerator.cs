﻿using PythonIsGame.Common.Materials;
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
        private bool Fill;

        public AreaMapGenerator(int left, int up, int right, int bottom, bool fill = false)
        {
            Left = left;
            Up = up;
            Right = right;
            Bottom = bottom;
            Fill = fill;
        }

        public Chunk Generate(Point chunkPosition)
        {
            var lineWidth = 3;
            var chunk = new Chunk(chunkPosition.X, chunkPosition.Y, ChunkSize);
            for (var x = 0; x < ChunkSize; x++)
            {
                for (var y = 0; y < ChunkSize; y++)
                {
                    var absoluteX = chunkPosition.X * ChunkSize + x;
                    var absoluteY = chunkPosition.Y * ChunkSize + y;
                    if (!Fill)
                    {
                        if ((absoluteX < Left && absoluteX >= Left - lineWidth)
                            || (absoluteX > Right && absoluteX <= Right + lineWidth)
                            || (absoluteY < Up && absoluteY >= Up - lineWidth)
                            || (absoluteY > Bottom && absoluteY <= Bottom + lineWidth))
                            chunk.SetMaterial(new Point(x, y), new WallMaterial());
                    }
                    else
                    {
                        if ((absoluteX < Left)
                            || (absoluteX > Right)
                            || (absoluteY < Up)
                            || (absoluteY > Bottom))
                            chunk.SetMaterial(new Point(x, y), new WallMaterial());
                    }
                }
            }
            return chunk;
        }
    }
}