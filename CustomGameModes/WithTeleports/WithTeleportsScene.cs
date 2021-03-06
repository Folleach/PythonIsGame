﻿using PythonIsGame.Common;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using PythonIsGame.Common.Scenes;
using System;
using System.Drawing;

namespace CustomGameModes.WithTeleports
{
    public class WithTeleportsScene : DefaultGameScene
    {
        public override void Create(SceneManager ownerManager, object data)
        {
            base.Create(ownerManager, data);
            map = new ChunkedMap(new EmptyMapGenerator());
            player = new Snake(2, 2, map, "player", true);
            map.SetMaterial(new TeleportMaterial(new Point(10, 10)), new Point(2, 10));
            map.RegisterIntersectionWithMaterial(player.Head, typeof(TeleportMaterial), m => Teleport(player.Head, m));
            for (var i = 0; i < 222; i++)
                player.AddTailSegment();
        }

        public override void Update(TimeSpan delta)
        {
            player.Update(delta);
            camera.TargetPosition = new Point(player.Position.X - (int)(Width / (2 * camera.Scale)), player.Position.Y - (int)(Height / (2 * camera.Scale)));
            camera.Update();
            map.Update();
        }

        private void Teleport(IEntity entity, PositionMaterial obj)
        {
            var material = obj.Material as TeleportMaterial;
            material.Teleport(entity);
        }
    }
}
