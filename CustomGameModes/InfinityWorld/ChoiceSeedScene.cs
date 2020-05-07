using PythonIsGame.Common;
using PythonIsGame.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomGameModes.InfinityWorld
{
    public class ChoiceSeedScene : Scene
    {
        TextBox textBoxSeed = new TextBox();
        Button buttonRandomSeed = new Button();
        DrawLabel labelTitle = new DrawLabel(18, false);

        public override void Create(SceneManager ownerManager, object data)
        {
            base.Create(ownerManager, data);
            KeyDownHandlers[Keys.Enter] = e => ownerManager.ReplaceScene(new InfinityWorldScene(), int.Parse(textBoxSeed.Text));
            buttonRandomSeed.Click += (s, e) => ownerManager.ReplaceScene(new InfinityWorldScene(), new Random().Next());
            buttonRandomSeed.Text = "Случайный";
            labelTitle.Text = "Зерно генератора (seed)";
            labelTitle.Height = 30;
            AddControl(textBoxSeed);
            AddControl(buttonRandomSeed);
            AddControl(labelTitle);
        }

        public override void Resize()
        {
            var halfWidth = Width / 2;
            var brHeight = 10;
            labelTitle.Width = buttonRandomSeed.Width = textBoxSeed.Width = halfWidth;
            labelTitle.Left = buttonRandomSeed.Left = textBoxSeed.Left = halfWidth / 2;
            labelTitle.Top = 20;
            textBoxSeed.Top = labelTitle.Top + labelTitle.Height + brHeight;
            buttonRandomSeed.Top = textBoxSeed.Top + textBoxSeed.Height + brHeight;
        }
    }
}
