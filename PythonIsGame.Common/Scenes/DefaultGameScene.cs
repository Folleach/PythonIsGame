using PythonIsGame.Common.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Common.Scenes
{
    public class DefaultGameScene : Scene
    {
        protected Snake Player;
        protected ChunkedMap Map;

        protected private Color Background = Color.FromArgb(46, 50, 61);

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            graphics.Clear(Background);
            foreach (var obj in Map.GetMaterials())
                graphics.FillRectangle(GetBrush(obj.Item1.Color), new Rectangle(obj.Item2, DefaultSize));
            foreach (var obj in Player)
                graphics.FillRectangle(GetBrush(obj.Color), new Rectangle(obj.Position, DefaultSize));
        }
    }
}
