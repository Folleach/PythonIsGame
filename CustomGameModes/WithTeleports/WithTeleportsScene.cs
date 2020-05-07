using PythonIsGame.Common;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using PythonIsGame.Common.Scenes;
using System;
using System.Drawing;
using System.Windows.Forms;

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
            KeyDownHandlers[Keys.A] = e => player.Direction = Direction.Left;
            KeyDownHandlers[Keys.W] = e => player.Direction = Direction.Up;
            KeyDownHandlers[Keys.D] = e => player.Direction = Direction.Right;
            KeyDownHandlers[Keys.S] = e => player.Direction = Direction.Down;
            for (var i = 0; i < 16; i++)
                player.AddTailSegment();
        }

        public override void Update(TimeSpan delta)
        {
            camera.TargetPosition = new Point(player.X - (int)(Width / (2 * camera.Scale)), player.Y - (int)(Height / (2 * camera.Scale)));
            camera.Update();
            player.Update();
            map.Update();
        }

        private void Teleport(IEntity entity, PositionMaterial obj)
        {
            var material = obj.Material as TeleportMaterial;
            material.Teleport(entity);
        }
    }
}
