using PythonIsGame.Common.Entities;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PythonIsGame.Common.Scenes
{
    public class DefaultGameScene : Scene
    {
        protected Snake player;
        protected ChunkedMap map;

        protected Color background = GameColors.GameBackground;
        protected Camera camera = new Camera();
        protected Dictionary<Type, Color> colorMapping = new Dictionary<Type, Color>();

        public override void Create(SceneManager ownerManager, object data)
        {
            base.Create(ownerManager, data);
            colorMapping.Add(typeof(AppleMaterial), GameColors.AppleMaterialColor);
            colorMapping.Add(typeof(TeleportMaterial), GameColors.TeleportMaterialColor);
            colorMapping.Add(typeof(WallMaterial), GameColors.WallMaterialColor);
            colorMapping.Add(typeof(SnakeHead), GameColors.SnakeHead);
            colorMapping.Add(typeof(SnakeBody), GameColors.SnakeBody);
        }

        public override void Draw(Graphics graphics)
        {
            var camPos = camera.GetTransformPosition();
            graphics.ScaleTransform(camera.Scale, camera.Scale);
            graphics.TranslateTransform(camPos.X, camPos.Y);
            graphics.Clear(background);
            foreach (var obj in map.GetMaterials())
                graphics.FillRectangle(GetBrush(colorMapping[obj.Item1.GetType()]), new Rectangle(obj.Item2, DefaultSize));
            foreach (var obj in player.GetEntities())
                graphics.FillRectangle(GetBrush(colorMapping[obj.GetType()]), new Rectangle(obj.Position, DefaultSize));
        }
    }
}
