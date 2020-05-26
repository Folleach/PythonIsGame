using PythonIsGame.Common.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace PythonIsGame.Common
{
    public abstract class Scene
    {
        public static readonly Size DefaultSize = new Size(1, 1);

        public readonly DrawPanel Root = new DrawPanel();

        public bool Active
        {
            get => active;
            set
            {
                active = value;
            }
        }
        private bool active;

        public int Width => Root.Width;
        public int Height => Root.Height;

        public Dictionary<Keys, Action<KeyEventArgs>> KeyDownHandlers
            = new Dictionary<Keys, Action<KeyEventArgs>>();

        protected Dictionary<Color, SolidBrush> brushes = new Dictionary<Color, SolidBrush>();

        private Dictionary<string, SoundPlayer> soundPlayers = new Dictionary<string, SoundPlayer>();

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
            graphic.Clear(GameColors.Background);
        }

        public virtual void Resize()
        {
        }

        protected void PlaySound(string file)
        {
            if (!soundPlayers.ContainsKey(file))
                soundPlayers.Add(file, new SoundPlayer(file));
            soundPlayers[file].Play();
        }

        protected Brush GetBrush(Color color)
        {
            if (!brushes.ContainsKey(color))
                brushes[color] = new SolidBrush(color);
            return brushes[color];
        }
    }
}
