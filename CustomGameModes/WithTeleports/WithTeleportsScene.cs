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
            Map = new ChunkedMap(new EmptyMapGenerator());
            Player = new Snake(2, 2, Map, "player", true);
            Map.SetMaterial(new TeleportMaterial(new Point(10, 10)), new Point(2, 10));
            Map.RegisterIntersectionWithMaterial(Player.Head, typeof(TeleportMaterial), m => Teleport(Player.Head, m));
            KeyDownHandlers[Keys.A] = e => Player.Direction = Direction.Left;
            KeyDownHandlers[Keys.W] = e => Player.Direction = Direction.Up;
            KeyDownHandlers[Keys.D] = e => Player.Direction = Direction.Right;
            KeyDownHandlers[Keys.S] = e => Player.Direction = Direction.Down;
            for (var i = 0; i < 16; i++)
                Player.AddTailSegment();
        }

        public override void Update(TimeSpan delta)
        {
            camera.TargetPosition = new Point(Player.X - (int)(Width / (2 * camera.Scale)), Player.Y - (int)(Height / (2 * camera.Scale)));
            camera.Update();
            Player.Update();
            Map.Update();
        }

        private void Teleport(IEntity entity, PositionMaterial obj)
        {
            var material = obj.Material as TeleportMaterial;
            material.Teleport(entity);
        }
    }
}
