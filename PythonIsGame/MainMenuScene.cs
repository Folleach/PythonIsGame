using PythonIsGame.Common;
using PythonIsGame.Common.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PythonIsGame
{
    public class MainMenuScene : Scene
    {
        private DrawLabel titleLabel = new DrawLabel(32, false);
        private DrawPictureBox pictureBox = new DrawPictureBox();
        private static Image snakeMainMenuImage = Image.FromFile("Images/snake_mainmenu.png");
        private List<Button> playButtons = new List<Button>();
        private ModificationLoader modificationLoader;

        public override void Create(SceneManager ownerManager, object data)
        {
            base.Create(ownerManager, data);
            modificationLoader = new ModificationLoader();
            titleLabel.Text = "Python is game";
            AddControl(titleLabel);
            foreach (var item in modificationLoader.OrderBy(x => x.Order))
            {
                var gameModeButton = new Button()
                {
                    Text = item.GameModeName,
                };
                gameModeButton.Click += (s, e) => ownerManager.PushScene(item.CreateGameScene(), null);
                playButtons.Add(gameModeButton);
                AddControl(gameModeButton);
            }
            pictureBox.Image = snakeMainMenuImage;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            AddControl(pictureBox);
        }

        public override void Resize()
        {
            titleLabel.Width = Width;
            titleLabel.Height = 50;
            var btnTop = 60;
            var btnWidth = Math.Max(Width / 3, 200);
            if (Width > 450)
            {
                pictureBox.Visible = true;
                pictureBox.Width = Width - btnWidth - 150;
                pictureBox.Height = (int)(pictureBox.Width * ((float)snakeMainMenuImage.Height / snakeMainMenuImage.Width));
                pictureBox.Left = btnWidth + 50;
                pictureBox.Top = btnTop;
            }
            else
                pictureBox.Visible = false;
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
