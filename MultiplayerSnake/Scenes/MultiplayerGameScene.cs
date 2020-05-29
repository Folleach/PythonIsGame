using Folleach.StreamNet.Client;
using MultiplayerSnake.Models;
using MultiplayerSnake.Models.CSModels;
using MultiplayerSnake.Models.SCModels;
using PythonIsGame.Common;
using PythonIsGame.Common.Entities;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using PythonIsGame.Common.SceneModels;
using PythonIsGame.Common.Scenes;
using PythonIsGame.Common.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiplayerSnake.Scenes
{
    public class MultiplayerGameScene : DefaultGameScene
    {
        private static readonly float emSize = 0.7f;
        private static readonly Font NametagFont = new Font(FontFamily.GenericMonospace, emSize);
        private static readonly SolidBrush FontBrush = new SolidBrush(Color.White);

        private SceneManager sceneManager;
        private StreamNetClient network;

        private DrawLabel guidLabel = new DrawLabel(12, true);
        private DrawLabel loggerLabel = new DrawLabel(12, true);
        private int LeaderboardOffset;
        private DrawLabel[] leaderboardLabels;

        private NetworkMapGenerator mapGenerator;
        private Rectangle MapArea;

        private Dictionary<Guid, NetworkSnake> players = new Dictionary<Guid, NetworkSnake>();

        public override void Create(SceneManager ownerManager, object data)
        {
            sceneManager = ownerManager;
            base.Create(ownerManager, data);
            colorMapping.Add(typeof(NetworkBody), GameColors.SnakeBody);
            var dataModel = data as GameInitializeModel;
            network = new StreamNetClient();
            network.RegisterSCID(1, (b) =>
            {
                var serverInitModel = ServerInitializePropertiesData.Unpack(b);
                mapGenerator = new NetworkMapGenerator(network, serverInitModel.ChunkSize, Log);
                map = new NetworkMap(mapGenerator, 1);
                player = new NetworkSnake(serverInitModel.InitX, serverInitModel.InitY, map, dataModel.UserName, serverInitModel.GUID, true);
                MapArea = serverInitModel.MapArea;
                leaderboardLabels = new DrawLabel[serverInitModel.LeaderboardCount];
                Root.Invoke((MethodInvoker)(() =>
                {
                    for (var i = 0; i < leaderboardLabels.Length; i++)
                    {
                        leaderboardLabels[i] = new DrawLabel(12, true);
                        leaderboardLabels[i].Width = 250;
                        AddControl(leaderboardLabels[i]);
                    }
                    leaderboardLabels[0].Top = LeaderboardOffset;
                    for (var i = 1; i < leaderboardLabels.Length; i++)
                        leaderboardLabels[i].Top = leaderboardLabels[i - 1].Top + leaderboardLabels[i - 1].Height;
                    Log($"Ваш ник: {player.Name}");
                }));
            });
            network.RegisterSCID(3, UpdateGameState);
            network.RegisterSCID(4, SpawnNetworkSnake);
            network.RegisterSCID(5, DespawnNetworkSnake);
            network.RegisterSCID(6, ChangeTail);
            network.RegisterSCID(7, SetMaterial);
            network.RegisterSCID(8, GameOver);
            network.RegisterSCID(9, UpdateLeaderboard);
            KeyDownHandlers[Keys.A] = e => ChangeDirection(Direction.Left);
            KeyDownHandlers[Keys.W] = e => ChangeDirection(Direction.Up);
            KeyDownHandlers[Keys.D] = e => ChangeDirection(Direction.Right);
            KeyDownHandlers[Keys.S] = e => ChangeDirection(Direction.Down);
            network.Connect(dataModel.IP, dataModel.Port);

            network.Send(1, new LoginData()
            {
                UserName = dataModel.UserName
            });
            guidLabel.Width = loggerLabel.Width = 400;
            loggerLabel.Top = guidLabel.Height;
            LeaderboardOffset = loggerLabel.Top + 2 * loggerLabel.Height;
            AddControl(guidLabel);
            AddControl(loggerLabel);
            camera.Smooth = true;
        }

        private void UpdateLeaderboard(byte[] data)
        {
            var model = LeaderboardSetData.Unpack(data);
            var index = 0;
            Root.Invoke((MethodInvoker)(() =>
            {
                foreach (var item in model.Leaderboard)
                {
                    leaderboardLabels[index].Text = $"{item.Key}: {item.Value}";
                    index++;
                }
                for (; index < leaderboardLabels.Length; index++)
                    leaderboardLabels[index].Text = "Пусто";
            }));
        }

        public override void Destroy()
        {
            network.Disconnect();
            base.Destroy();
        }

        private void SetMaterial(byte[] data)
        {
            var model = SetMaterialData.Unpack(data);
            map.SetMaterial(model.Material, model.Position);
        }

        private void ChangeTail(byte[] data)
        {
            var model = ChangeTailData.Unpack(data);
            NetworkSnake target = null;
            if ((player as NetworkSnake).GUID == model.GUID)
            {
                target = (NetworkSnake)player;
                PlaySound("Sounds/hit.wav");
            }
            else
                target = players.Where(x => x.Value.GUID == model.GUID).FirstOrDefault().Value;
            if (target == null)
                return;
            var sign = Math.Sign(model.DeltaTailSegment) * -1;
            var current = model.DeltaTailSegment;
            while (current != 0)
            {
                current += sign;
                if (sign == -1)
                    target.AddTailSegment();
                else
                    target.RemoveTailSegment();
            }
        }

        private void SpawnNetworkSnake(byte[] data)
        {
            var model = SpawnSnakeData.Unpack(data);
            var snake = new NetworkSnake(model.X, model.Y, map, model.Name, model.GUID);
            players.Add(model.GUID, snake);
            if (model.Tail != null)
            {
                foreach (var position in model.Tail)
                    snake.AddTailSegment(position);
            }
        }

        private void DespawnNetworkSnake(byte[] data)
        {
            var model = DespawnSnakeData.Unpack(data);
            if (players.ContainsKey(model.GUID))
            {
                var player = players[model.GUID];
                players.Remove(model.GUID);
                player.Kill();
            }
        }

        private void UpdateGameState(byte[] data)
        {
            var gameState = UpdateGameStateData.Unpack(data);
            var netPlayer = player as NetworkSnake;
            foreach (var item in gameState.Positions)
            {
                if (netPlayer.GUID == item.GUID)
                {
                    netPlayer.Position = item.Position;
                    continue;
                }
                if (!players.ContainsKey(item.GUID))
                    continue;
                players[item.GUID].Position = item.Position;
            }
            map.Update();
        }

        private void ChangeDirection(Direction direction)
        {
            network.Send(3, new ChangeDirection(direction));
        }

        public override void Update(TimeSpan delta)
        {
            if (player == null)
                return;
            if (string.IsNullOrEmpty(guidLabel.Text))
                guidLabel.Text = (player as NetworkSnake).GUID.ToString();
            
            var halfWidth = (int)(Width / (2 * camera.Scale));
            var halfHeight = (int)(Height / (2 * camera.Scale));
            camera.TargetPosition = new PointF(
                GetInsideValue(player.Position.X - halfWidth, MapArea.X, MapArea.Width - halfWidth * 2 + 1),
                GetInsideValue(player.Position.Y - halfHeight, MapArea.Y, MapArea.Height - halfHeight * 2 + 2));
            camera.Update();
        }

        private float GetInsideValue(float value, int x, int width)
        {
            return Math.Max(x, Math.Min(value, width));
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            foreach (var player in players)
            {
                var width = player.Value.Name.Length * emSize;
                graphics.DrawString(player.Value.Name, NametagFont, FontBrush,
                    new RectangleF(player.Value.Position.X - width / 2, player.Value.Position.Y - 1, width, camera.Scale));
            }
        }

        private void Log(string message)
        {
            loggerLabel.Text = message;
        }

        private void GameOver(byte[] data)
        {
            var model = KickData.Unpack(data);
            Root.Invoke((MethodInvoker)(() =>
            {
                sceneManager.ReplaceScene(new GameOverScene(), new GameOverModel()
                {
                    Message = model.Message,
                    Score = model.Score,
                    GameScene = this
                });
            }));
            network.Disconnect();
        }
    }
}
