using PythonIsGame.Common.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PythonIsGame.Common
{
    public abstract class Scene
    {
        public static readonly Size DefaultSize = new Size(1, 1);

        public readonly DrawPanel Root = new DrawPanel();

        public int Width => Root.Width;
        public int Height => Root.Height;

        public Dictionary<Keys, Action<KeyEventArgs>> KeyDownHandlers
            = new Dictionary<Keys, Action<KeyEventArgs>>();

        protected Camera camera = new Camera();

        protected Dictionary<Color, SolidBrush> brushes = new Dictionary<Color, SolidBrush>();

        public void AddControl(Control element)
        {
            Root.Controls.Add(element);
        }

        public void RemoveControl(Control element)
        {
            Root.Controls.Remove(element);
        }

        public virtual void Create(SceneManager ownerManager, object data)
        {
            KeyDownHandlers[Keys.Escape] = e => ownerManager.RemoveScene();
        }

        public virtual void Destroy()
        {
        }

        public virtual void Update(TimeSpan delta)
        {
        }

        public virtual void Draw(Graphics graphic)
        {
            var camPos = camera.GetTransformPosition();
            graphic.ScaleTransform(camera.Scale, camera.Scale);
            graphic.TranslateTransform(camPos.X, camPos.Y);
        }

        public virtual void Resize()
        {
        }

        protected Brush GetBrush(Color color)
        {
            if (!brushes.ContainsKey(color))
                brushes[color] = new SolidBrush(color);
            return brushes[color];
        }
    }
}
