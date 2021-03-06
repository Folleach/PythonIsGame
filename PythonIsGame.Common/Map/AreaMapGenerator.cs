﻿using PythonIsGame.Common.Materials;
using System;
using System.Drawing;

namespace PythonIsGame.Common.Map
{
    public class AreaMapGenerator : IMapGenerator
    {
        public int ChunkSize { get; private set; }

        private int Left;
        private int Up;
        private int Right;
        private int Bottom;
        private bool WallIsTeleport;

        private Random random = new Random();

        public AreaMapGenerator(int left, int up, int right, int bottom, bool wallIsTeleport = false, int chunkSize = 256)
        {
            Left = left;
            Up = up;
            Right = right;
            Bottom = bottom;
            WallIsTeleport = wallIsTeleport;
            ChunkSize = chunkSize;
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
                        chunk.SetMaterial(new Point(x, y), WallIsTeleport ? (IMaterial)new TeleportMaterial(new Point(absoluteX == Left ? Right - 1 : 1, y)) : new WallMaterial());
                    if ((absoluteY == Up || absoluteY == Bottom) && absoluteX > Left && absoluteX < Right)
                        chunk.SetMaterial(new Point(x, y), WallIsTeleport ? (IMaterial)new TeleportMaterial(new Point(x, absoluteY == Up ? Bottom - 1 : 1)) : new WallMaterial());
                }
            }
            return chunk;
        }

        public Point GetRandomPointInArea()
        {
            return new Point(1 + (random.Next() % (Right - 2)), 1 + (random.Next() % (Bottom - 2)));
        }
    }
}
