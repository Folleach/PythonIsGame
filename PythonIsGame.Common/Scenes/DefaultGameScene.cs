using PythonIsGame.Common.Entities;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PythonIsGame.Common.Scenes
{
    public class DefaultGameScene : Scene
    {
        protected Snake player;
        protected ChunkedMap map;

        protected Color background = GameColors.GameBackground;
        protected bool gradient = true;
        protected Camera camera = new Camera();
        protected Dictionary<Type, Color> colorMapping = new Dictionary<Type, Color>();

        private Image gradientImage = Image.FromFile("Images/gradient.png");
        private Rectangle gradiendRectangle;

        public override void Create(SceneManager ownerManager, object data)
        {
            base.Create(ownerManager, data);
            colorMapping.Add(typeof(AppleMaterial), GameColors.AppleMaterialColor);
            colorMapping.Add(typeof(TeleportMaterial), GameColors.TeleportMaterialColor);
            colorMapping.Add(typeof(WallMaterial), GameColors.WallMaterialColor);
            colorMapping.Add(typeof(SnakeHead), GameColors.SnakeHead);
            colorMapping.Add(typeof(SnakeBody), GameColors.SnakeBody);

            KeyDownHandlers[Keys.A] = e => player.Direction = Direction.Left;
            KeyDownHandlers[Keys.W] = e => player.Direction = Direction.Up;
            KeyDownHandlers[Keys.D] = e => player.Direction = Direction.Right;
            KeyDownHandlers[Keys.S] = e => player.Direction = Direction.Down;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.Clear(background);
            if (gradient)
                graphics.DrawImage(gradientImage, gradiendRectangle);
            var camPos = camera.GetTransformPosition();
            graphics.ScaleTransform(camera.Scale, camera.Scale);
            graphics.TranslateTransform(camPos.X, camPos.Y);
            
            foreach (var obj in map.GetMaterials())
                graphics.FillRectangle(GetBrush(colorMapping[obj.Item1.GetType()]), new Rectangle(obj.Item2, DefaultSize));
            foreach (var obj in player.GetEntities())
                graphics.FillRectangle(GetBrush(colorMapping[obj.GetType()]), new Rectangle(obj.Position, DefaultSize));
        }

        public override void Resize()
        {
            gradiendRectangle = new Rectangle(new Point(0, 0), new Size(Width, Height));
        }
    }
}
