using PythonIsGame.Common;
using PythonIsGame.Common.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PythonIsGame
{
    public class MainMenuScene : Scene
    {
        private DrawLabel titleLabel = new DrawLabel(32, false);
        private List<Button> playButtons = new List<Button>();
        private ModificationLoader modificationLoader;

        public override void Create(SceneManager ownerManager, object data)
        {
            modificationLoader = new ModificationLoader();
            titleLabel.Text = "Python is game";
            AddControl(titleLabel);
            foreach (var item in modificationLoader)
            {
                var gameModeButton = new Button()
                {
                    Text = item.GameModeName,
                };
                gameModeButton.Click += (s, e) => ownerManager.PushScene(item.CreateGameScene(), null);
                playButtons.Add(gameModeButton);
                AddControl(gameModeButton);
            }
            KeyDownHandlers.Add(Keys.Escape, e => ownerManager.RemoveScene());
        }

        public override void Resize()
        {
            titleLabel.Width = Width;
            titleLabel.Height = 50;
            if (Width > 450)
            {
                //TODO: _
            }
            var btnTop = 60;
            var btnWidth = Math.Max(Width / 3, 200);
            foreach (var playButton in playButtons)
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
