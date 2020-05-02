using PythonIsGame.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PythonIsGame
{
    public class MainMenuScene : Scene
    {
        private Label title = new Label();
        private List<Button> PlayButtons = new List<Button>();
        private ModificationLoader modificationLoader;
        public override void Create(SceneManager ownerManager, object data)
        {
            modificationLoader = new ModificationLoader();
            title.Text = "Python is game";
            title.ForeColor = Color.White;
            title.BackColor = Color.Transparent;
            title.Font = new Font(FontFamily.GenericMonospace, 32);
            AddControl(title);
            foreach (var item in modificationLoader)
            {
                var btn = new Button()
                {
                    Text = item.GameModeName,
                };
                btn.Click += (s, e) => ownerManager.PushScene(item.CreateGameScene(), null);
                PlayButtons.Add(btn);
                AddControl(btn);
            }
            KeyDownHandlers.Add(Keys.Escape, e => ownerManager.RemoveScene());
        }

        public override void Draw(Graphics graphic)
        {
            graphic.Clear(Color.FromArgb(27, 32, 35));
        }

        public override void Resize()
        {
            title.Width = Width;
            title.Height = 50;
            if (Width > 450)
            {
                //TODO: _
            }
            var btnTop = 60;
            var btnWidth = Math.Max(Width / 3, 200);
            foreach (var playButton in PlayButtons)
            {
                playButton.Top = btnTop;
                playButton.Left = 10;
                playButton.Width = btnWidth;
                playButton.Height = 30;
                btnTop += 40;
            }
        }
    }
}
