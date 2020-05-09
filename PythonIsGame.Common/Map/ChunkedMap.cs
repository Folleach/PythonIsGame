using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PythonIsGame.Common.Map
{
    public class ChunkedMap : IMap
    {
        public IMapGenerator Generator { get; }

        //TODO: Выгрузка ненужных частей
        private Dictionary<Point, Chunk> chunks;
        private Dictionary<IEntity, Dictionary<Type, Action<PositionMaterial>>> intersectingWithMaterials;
        private Dictionary<IEntity, Dictionary<Type, Action<IEntity>>> intersectingWithEntity;
        private List<IEntity> entities;
        private Dictionary<Type, LinkedList<IEntity>> entitiesByType;
        private List<IEntity> chunksFollowFrom;
        private Queue<Action> intersectingEvents;
        private int drawingRange;

        public ChunkedMap(IMapGenerator mapGenerator, int drawingRange = 1)
        {
            Generator = mapGenerator;
            this.drawingRange = drawingRange;
            chunks = new Dictionary<Point, Chunk>();
            intersectingWithMaterials = new Dictionary<IEntity, Dictionary<Type, Action<PositionMaterial>>>();
            intersectingWithEntity = new Dictionary<IEntity, Dictionary<Type, Action<IEntity>>>();
            entities = new List<IEntity>();
            entitiesByType = new Dictionary<Type, LinkedList<IEntity>>();
            chunksFollowFrom = new List<IEntity>();
            intersectingEvents = new Queue<Action>();
        }

        public bool AddEntity(IEntity entity, bool chunkFollow)
        {
            entities.Add(entity);
            if (chunkFollow)
                chunksFollowFrom.Add(entity);
            var type = entity.GetType();
            if (!entitiesByType.ContainsKey(type))
                entitiesByType.Add(type, new LinkedList<IEntity>());
            entitiesByType[type].AddFirst(entity);
            return true;
        }

        public bool RemoveEntity(IEntity entity)
        {
            var type = entity.GetType();
            entitiesByType[type].Remove(entity);
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
            if (!intersectingWithMaterials.ContainsKey(entity))
                intersectingWithMaterials[entity] = new Dictionary<Type, Action<PositionMaterial>>();
            intersectingWithMaterials[entity][material] = callback;
        }

        public void UnregisterIntersectionWithMaterial(IEntity entity, Type material)
        {
            if (!intersectingWithMaterials.ContainsKey(entity))
                return;
            intersectingWithMaterials[entity].Remove(material);
        }

        public void RegisterIntersectionWithEntity(IEntity entity, Type otherEntity, Action<IEntity> callback)
        {
            if (!intersectingWithEntity.ContainsKey(entity))
                intersectingWithEntity[entity] = new Dictionary<Type, Action<IEntity>>();
            intersectingWithEntity[entity][otherEntity] = callback;
        }

        public void UnregisterIntersectionWithEntity(IEntity entity, Type otherEntity)
        {
            if (!intersectingWithEntity.ContainsKey(entity))
                return;
            intersectingWithEntity[entity].Remove(otherEntity);
        }

        public void Update()
        {
            foreach (var position in chunksFollowFrom.Select(entity => entity.Position))
                LoadChunksFrom(position);
            intersectingEvents.Clear();
            foreach (var entity in intersectingWithMaterials)
            {
                var behindMaterial = GetMaterial(entity.Key.Position);
                if (behindMaterial.Material == null)
                    continue;
                var type = behindMaterial.Material.GetType();
                if (entity.Value.ContainsKey(type))
                {
                    var x = entity.Value[type];
                    intersectingEvents.Enqueue(() => x.Invoke(behindMaterial));
                }
            }
            foreach (var entity in intersectingWithEntity)
            {
                foreach (var type in entity.Value.Keys)
                {
                    if (!entitiesByType.ContainsKey(type))
                        continue;
                    foreach (var item in entitiesByType[type])
                    {
                        if (entity.Key.Intersect(item))
                        {
                            var x = intersectingWithEntity[entity.Key][type];
                            intersectingEvents.Enqueue(() => x.Invoke(item));
                        }
                    }
                }
            }
            while (intersectingEvents.Count != 0)
                intersectingEvents.Dequeue().Invoke();
        }

        public IEntity GetEntity(Point position)
        {
            //TODO: Можно сделать оптимизацию используя куски карты
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
            return chunks[chunkPosition].SetMaterial(GetPositionInChunk(position), material);
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
            foreach (var chunk in chunks)
            {
                foreach (var material in chunk.Value.GetMaterials())
                    yield return material;
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
            if (absolutePosition.X < 0)
                absolutePosition.X -= Generator.ChunkSize;
            if (absolutePosition.Y < 0)
                absolutePosition.Y -= Generator.ChunkSize;
            return new Point(absolutePosition.X / Generator.ChunkSize,
                absolutePosition.Y / Generator.ChunkSize);
        }

        private Point GetPositionInChunk(Point absoulePosition)
        {
            return new Point(ValueMapping(absoulePosition.X, Generator.ChunkSize),
                ValueMapping(absoulePosition.Y, Generator.ChunkSize));
        }

        private void GenerateChunkIfNotExists(Point position)
        {
            if (!InsideLoadedMap(position))
            {
                var chunkPosition = GetChunkPosition(position);
                chunks[chunkPosition] = Generator.Generate(chunkPosition);
            }
        }

        private int ValueMapping(int val, int l)
        {
            return (val % l + l) % l;
        }
    }
}
