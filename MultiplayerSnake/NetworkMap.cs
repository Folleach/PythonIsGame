using PythonIsGame.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake
{
    public class NetworkMap : IMap
    {
        public IMapGenerator Generator { get; }

        private Dictionary<Point, Chunk> chunks;
        private List<IEntity> entities;
        private List<IEntity> chunksFollowFrom;
        private int drawingRange;
        private LinkedList<PositionMaterial> materialCache = new LinkedList<PositionMaterial>();

        private object chunksLockObject = new object();

        public int MaxX { get; private set; } = int.MinValue;
        public int MaxY { get; private set; } = int.MinValue;

        public NetworkMap(IMapGenerator mapGenerator, int drawingRange = 1)
        {
            Generator = mapGenerator;
            this.drawingRange = drawingRange;
            chunks = new Dictionary<Point, Chunk>();
            entities = new List<IEntity>();
            chunksFollowFrom = new List<IEntity>();
        }

        public bool AddEntity(IEntity entity, bool chunkFollow)
        {
            entities.Add(entity);
            if (chunkFollow)
                chunksFollowFrom.Add(entity);
            return true;
        }

        public bool RemoveEntity(IEntity entity)
        {
            if (chunksFollowFrom.Contains(entity))
                chunksFollowFrom.Remove(entity);
            return entities.Remove(entity);
        }

        public bool InsideLoadedMap(Point position)
        {
            return chunks.ContainsKey(GetChunkPosition(position));
        }

        public void RegisterIntersectionWithMaterial(IEntity entity, Type material, Action<PositionMaterial> callback)
        {
            throw new Exception("Not allowed in NetworkMap");
        }

        public void UnregisterIntersectionWithMaterial(IEntity entity, Type material)
        {
            throw new Exception("Not allowed in NetworkMap");
        }

        public void RegisterIntersectionWithEntity(IEntity entity, Type otherEntity, Action<IEntity> callback)
        {
            throw new Exception("Not allowed in NetworkMap");
        }

        public void UnregisterIntersectionWithEntity(IEntity entity, Type otherEntity)
        {
            throw new Exception("Not allowed in NetworkMap");
        }

        public void Update()
        {
            foreach (var position in chunksFollowFrom.Select(entity => entity.Position))
                LoadChunksFrom(position);
        }

        public IEntity GetEntity(Point position)
        {
            foreach (var entity in entities)
            {
                if (entity.Position == position)
                    return entity;
            }
            return null;
        }

        public PositionMaterial GetMaterial(Point position)
        {
            var chunkPosition = GetChunkPosition(position);
            GenerateChunkIfNotExists(position);
            return chunks[chunkPosition]
                    .GetMaterial(GetPositionInChunk(position));
        }

        public bool SetMaterial(IMaterial material, Point position)
        {
            var chunkPosition = GetChunkPosition(position);
            GenerateChunkIfNotExists(position);
            var positionInChunk = GetPositionInChunk(position);
            if (!chunks.ContainsKey(chunkPosition))
            {
                materialCache.AddLast(new PositionMaterial(material, position));
                return false;
            }
            return chunks[chunkPosition].SetMaterial(positionInChunk, material);
        }

        public bool RemoveMaterial(Point position)
        {
            var chunkPosition = GetChunkPosition(position);
            if (!InsideLoadedMap(chunkPosition))
                return false;
            return chunks[chunkPosition].RemoveMaterial(GetPositionInChunk(position));
        }

        public IEnumerable<Tuple<IMaterial, Point>> GetMaterials()
        {
            lock (chunksLockObject)
            {
                foreach (var chunk in chunks)
                {
                    foreach (var material in chunk.Value.GetMaterials())
                        yield return material;
                }
            }
        }

        public IEnumerable<IEntity> GetEntities()
        {
            foreach (var entity in entities)
                yield return entity;
        }

        private void LoadChunksFrom(Point position)
        {
            var range = drawingRange * Generator.ChunkSize;
            for (var x = position.X - range; x <= position.X + range; x += Generator.ChunkSize)
            {
                for (var y = position.Y - range; y <= position.Y + range; y += Generator.ChunkSize)
                    GenerateChunkIfNotExists(new Point(x, y));
            }
        }

        private Point GetChunkPosition(Point absolutePosition)
        {
            var localPos = GetPositionInChunk(absolutePosition);
            if (absolutePosition.X < 0)
                absolutePosition.X -= Generator.ChunkSize + localPos.X - 1;
            if (absolutePosition.Y < 0)
                absolutePosition.Y -= Generator.ChunkSize + localPos.Y - 1;
            return new Point(absolutePosition.X / Generator.ChunkSize,
                absolutePosition.Y / Generator.ChunkSize);
        }

        private Point GetPositionInChunk(Point absolutePosition)
        {
            return new Point(ValueMapping(absolutePosition.X, Generator.ChunkSize),
                ValueMapping(absolutePosition.Y, Generator.ChunkSize));
        }

        private void GenerateChunkIfNotExists(Point position)
        {
            lock (chunksLockObject)
            {
                if (!InsideLoadedMap(position))
                {
                    var chunkPosition = GetChunkPosition(position);
                    var chunk = Generator.Generate(chunkPosition);
                    if (chunk == null)
                        return;
                    if (materialCache.Count > 0)
                        RefreshMaterialCache(chunk, chunkPosition);
                    chunks[chunkPosition] = chunk;
                }
            }
        }

        private void RefreshMaterialCache(Chunk chunk, Point chunkPosition)
        {
            var toremoveFromCache = new Queue<PositionMaterial>();
            foreach (var material in materialCache)
            {
                var cpos = GetChunkPosition(material.Position);
                if (cpos != chunkPosition)
                    continue;
                chunk.SetMaterial(GetPositionInChunk(material.Position), material.Material);
                toremoveFromCache.Enqueue(material);
            }
            foreach (var material in toremoveFromCache)
                materialCache.Remove(material);
        }

        private int ValueMapping(int val, int l)
        {
            return (val % l + l) % l;
        }
    }
}
