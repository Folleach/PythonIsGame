using PythonIsGame.Common;
using PythonIsGame.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiplayerSnake.Scenes
{
    public class ChoiceServerScene : Scene
    {
        private SceneManager sceneManager;

        private static string IPAddressCache = "207.148.31.28";
        private static string PortCache = "51777";
        private static string UserNameCache = "Игрок";

        TextBox textBoxIp = new TextBox();
        DrawLabel labelIp = new DrawLabel(12, false) { Text = "IP адрес" };
        TextBox textBoxPort = new TextBox();
        DrawLabel labelPort = new DrawLabel(12, false) { Text = "Порт" };
        TextBox textBoxUserName = new TextBox();
        DrawLabel labelUserName = new DrawLabel(12, false) { Text = "Имя игрока" };
        Button buttonConnect = new Button() { Text = "Подключиться" };
        DrawLabel labelTitle = new DrawLabel(18, false);

        public override void Create(SceneManager ownerManager, object data)
        {
            sceneManager = ownerManager;
            base.Create(ownerManager, data);
            KeyDownHandlers[Keys.Enter] = e => Connect();
            buttonConnect.Click += (s, e) => Connect();
            labelTitle.Text = "Подключение к серверу";
            labelTitle.Height = 30;
            AddControl(labelIp);
            AddControl(textBoxIp);
            AddControl(labelPort);
            AddControl(textBoxPort);
            AddControl(labelUserName);
            AddControl(textBoxUserName);
            AddControl(buttonConnect);
            AddControl(labelTitle);

            textBoxIp.Text = IPAddressCache;
            textBoxPort.Text = PortCache;
            textBoxUserName.Text = UserNameCache;
        }

        private void Connect()
        {
            IPAddressCache = textBoxIp.Text;
            PortCache = textBoxPort.Text;
            UserNameCache = textBoxUserName.Text;

            var model = new GameInitializeModel();

            if (IPAddress.TryParse(IPAddressCache, out IPAddress ip) &&
                int.TryParse(PortCache, out int port))
            {
                model.IP = ip;
                model.Port = port;
                model.UserName = UserNameCache;
                sceneManager.PushScene(new MultiplayerGameScene(), model);
                return;
            }
        }

        public override void Resize()
        {
            var halfWidth = Width / 2;
            var brHeight = 10;
            labelIp.Width = halfWidth;
            textBoxIp.Width = halfWidth;
            labelPort.Width = halfWidth;
            textBoxPort.Width = halfWidth;
            labelUserName.Width = halfWidth;
            textBoxUserName.Width = halfWidth;
            buttonConnect.Width = halfWidth;
            labelTitle.Width = halfWidth;

            labelIp.Left = halfWidth / 2;
            textBoxIp.Left = halfWidth / 2;
            labelPort.Left = halfWidth / 2;
            textBoxPort.Left = halfWidth / 2;
            labelUserName.Left = halfWidth / 2;
            textBoxUserName.Left = halfWidth / 2;
            buttonConnect.Left = halfWidth / 2;
            labelTitle.Left = halfWidth / 2;

            labelTitle.Top = 20;
            labelIp.Top = labelTitle.Top + labelTitle.Height + brHeight;
            textBoxIp.Top = labelIp.Top + labelIp.Height + brHeight;
            labelPort.Top = textBoxIp.Top + textBoxIp.Height + brHeight;
            textBoxPort.Top = labelPort.Top + labelPort.Height + brHeight;
            labelUserName.Top = textBoxPort.Top + textBoxPort.Height + brHeight;
            textBoxUserName.Top = labelUserName.Top + labelUserName.Height + brHeight;
            buttonConnect.Top = textBoxUserName.Top + textBoxUserName.Height + brHeight;
        }

    }
}
