using System;
using System.Collections.Generic;
using System.Drawing;

namespace PythonIsGame.Common
{
    public interface IMap
    {
        IMapGenerator Generator { get; }
        bool AddEntity(IEntity entity, bool chunkFollow);
        bool RemoveEntity(IEntity entity);
        bool RemoveMaterial(Point position);
        bool InsideLoadedMap(Point position);
        void RegisterIntersectionWithMaterial(IEntity entity, Type material, Action<PositionMaterial> callback);
        void RegisterIntersectionWithEntity(IEntity entity, Type otherEntity, Action<IEntity> callback);
        void Update();
        PositionMaterial GetMaterial(Point position);
        bool SetMaterial(IMaterial material, Point position);
        IEnumerable<Tuple<IMaterial, Point>> GetMaterials();
        IEnumerable<IEntity> GetEntities();
    }
}
