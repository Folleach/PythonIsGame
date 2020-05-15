using System;
using System.Collections.Generic;
using System.Drawing;

namespace PythonIsGame.Common
{
    public class Chunk
    {
        public int ChunkPositionX { get; private set; }
        public int ChunkPositionY { get; private set; }
        private IMaterial[,] chunkData;
        private int chunkSize;

        public Chunk(int posX, int posY, int size)
        {
            ChunkPositionX = posX;
            ChunkPositionY = posY;
            this.chunkSize = size;
            chunkData = new IMaterial[size, size];
        }

        public bool SetMaterial(Point relativePosition, IMaterial material)
        {
            chunkData[relativePosition.X, relativePosition.Y] = material;
            return true;
        }

        public bool RemoveMaterial(Point relativePosition)
        {
            chunkData[relativePosition.X, relativePosition.Y] = null;
            return true;
        }

        public PositionMaterial GetMaterial(Point relativePosition)
        {
            return new PositionMaterial(chunkData[relativePosition.X, relativePosition.Y],
                GetAbsolutePoint(relativePosition.X, relativePosition.Y));
        }

        public IEnumerable<Tuple<IMaterial, Point>> GetMaterials()
        {
            for (var x = 0; x < chunkSize; x++)
            {
                for (var y = 0; y < chunkSize; y++)
                {
                    if (chunkData[x, y] != null)
                        yield return Tuple.Create(chunkData[x, y],
                            GetAbsolutePoint(x, y));
                }
            }
        }

        public Point GetAbsolutePoint(int x, int y)
        {
            return new Point(ChunkPositionX * chunkSize + x, ChunkPositionY * chunkSize + y);
        }
    }
}
