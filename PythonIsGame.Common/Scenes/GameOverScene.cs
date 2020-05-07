using PythonIsGame.Common;
using PythonIsGame.Common.SceneModels;
using PythonIsGame.Common.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PythonIsGame.Common.Scenes
{
    public class GameOverScene : Scene
    {
        private DrawLabel messageLabel = new DrawLabel(14, false);
        private DrawLabel scoreLabel = new DrawLabel(12, false);
        private Button buttonToMainMenu = new Button();
        private DrawPictureBox pictureSnakeDead = new DrawPictureBox();
        private static Image snakeDeadImage = Image.FromFile("Images/snake_dead.png");
        private static Image snakeAliveImage = Image.FromFile("Images/snake_alive.png");

        public override void Create(SceneManager ownerManager, object data)
        {
            var model = data as GameOverModel;
            if (model != null)
            {
                messageLabel.Text = model.Message;
                scoreLabel.Text = $"Ваш результат: {model.Score}";
            }
            base.Create(ownerManager, data);
            buttonToMainMenu = new Button();
            buttonToMainMenu.Text = "Вернуться в главное меню";
            buttonToMainMenu.Click += (s, e) => ownerManager.RemoveScene();
            pictureSnakeDead.Image = model.IsWin ? snakeAliveImage : snakeDeadImage;
            pictureSnakeDead.SizeMode = PictureBoxSizeMode.StretchImage;
            AddControl(buttonToMainMenu);
            AddControl(messageLabel);
            AddControl(scoreLabel);
            AddControl(pictureSnakeDead);
        }

        public override void Draw(Graphics graphic)
        {
            base.Draw(graphic);
        }

        public override void Resize()
        {
            base.Resize();
            var halfWidth = Width / 2;
            var brHeight = 10;
            messageLabel.Width = scoreLabel.Width = buttonToMainMenu.Width = halfWidth;
            pictureSnakeDead.Width = halfWidth / 4;
            pictureSnakeDead.Left = halfWidth / 2;
            messageLabel.Left = scoreLabel.Left = buttonToMainMenu.Left = halfWidth / 2;
            pictureSnakeDead.Top = 20;
            pictureSnakeDead.Height = (int)(pictureSnakeDead.Width * ((float)snakeDeadImage.Height / snakeDeadImage.Width));
            messageLabel.Top = pictureSnakeDead.Top + pictureSnakeDead.Height + brHeight;
            scoreLabel.Top = messageLabel.Top + messageLabel.Height + brHeight;
            buttonToMainMenu.Top = scoreLabel.Top + scoreLabel.Height + brHeight;
        }
    }
}
