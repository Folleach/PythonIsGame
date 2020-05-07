using PythonIsGame.Common.Map;
using System.Drawing;

namespace PythonIsGame.Common.Scenes
{
    public class DefaultGameScene : Scene
    {
        protected Snake player;
        protected ChunkedMap map;

        protected Color background = GameColors.GameBackground;

        protected Camera camera = new Camera();

        public override void Draw(Graphics graphics)
        {
            var camPos = camera.GetTransformPosition();
            graphics.ScaleTransform(camera.Scale, camera.Scale);
            graphics.TranslateTransform(camPos.X, camPos.Y);
            graphics.Clear(background);
            foreach (var obj in map.GetMaterials())
                graphics.FillRectangle(GetBrush(obj.Item1.Color), new Rectangle(obj.Item2, DefaultSize));
            foreach (var obj in player)
                graphics.FillRectangle(GetBrush(obj.Color), new Rectangle(obj.Position, DefaultSize));
        }
    }
}
