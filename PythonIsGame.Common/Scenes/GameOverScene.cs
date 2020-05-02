using PythonIsGame.Common;
using PythonIsGame.Common.SceneModels;
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
        Label messageLabel = new Label();
        Label scoreLabel = new Label();
        Button buttonToMainMenu = new Button();

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
            AddControl(buttonToMainMenu);
            AddControl(messageLabel);
            AddControl(scoreLabel);
        }

        public override void Draw(Graphics graphic)
        {
            base.Draw(graphic);
        }

        public override void Resize()
        {
            base.Resize();
            messageLabel.Width = Width;
            scoreLabel.Width = Width;
            buttonToMainMenu.Width = Width;
            messageLabel.Top = 100;
            scoreLabel.Top = 150;
        }
    }
}
