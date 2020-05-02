using PythonIsGame.Common;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using PythonIsGame.Common.SceneModels;
using PythonIsGame.Common.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CustomGameModes.InfinityWorld
{
    public class InfinityWorldScene : Scene
    {
        private ChunkedMap map;
        private Snake player;

        private Color background = Color.FromArgb(46, 50, 61);

        private Label positionLabel = new Label();

        private Dictionary<Color, SolidBrush> brushes = new Dictionary<Color, SolidBrush>();

        public override void Create(SceneManager ownerManager, object data)
        {
            base.Create(ownerManager, data);
            map = new ChunkedMap(new InfinityWorldGenerator(87122356), 1);
            player = new Snake(2, 2, map, "player", true);
            map.RegisterIntersectionWithMaterial(player.Head, typeof(WallMaterial), material => GameOver(ownerManager));
            map.RegisterIntersectionWithMaterial(player.Head, typeof(AppleMaterial), PlayerEat);
            KeyDownHandlers[Keys.A] = e => player.Direction = Direction.Left;
            KeyDownHandlers[Keys.W] = e => player.Direction = Direction.Up;
            KeyDownHandlers[Keys.D] = e => player.Direction = Direction.Right;
            KeyDownHandlers[Keys.S] = e => player.Direction = Direction.Down;
            positionLabel.BackColor = Color.Transparent;
            positionLabel.ForeColor = Color.White;
            AddControl(positionLabel);
        }

        public override void Update(TimeSpan delta)
        {
            camera.TargetPosition = new Point(player.X - (int)(Width / (2 * camera.Scale)), player.Y - (int)(Height / (2 * camera.Scale)));
            camera.Update();
            player.Update();
            positionLabel.Text = $"Position: ({player.X}, {player.Y})";
            map.Update();
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            graphics.Clear(background);
            foreach (var obj in map.GetMaterials())
            {
                graphics.FillRectangle(GetBrush(obj.Item1.Color), new Rectangle(obj.Item2, DefaultSize));
            }
            foreach (var obj in player)
            {
                graphics.FillRectangle(GetBrush(obj.Color), new Rectangle(obj.Position, DefaultSize));
            }
        }

        public override void Resize()
        {
            positionLabel.Width = Width;
        }

        private Brush GetBrush(Color color)
        {
            if (!brushes.ContainsKey(color))
                brushes[color] = new SolidBrush(color);
            return brushes[color];
        }

        private void GameOver(SceneManager ownerManager)
        {
            ownerManager.ReplaceScene(new GameOverScene(), new GameOverModel()
            {
                Score = player.Score,
                Message = "Бесконечный путь закончился...",
                GameScene = this
            });
        }

        private void PlayerEat(PositionMaterial obj)
        {
            var apple = obj.Material as AppleMaterial;
            player.Score += apple.NutritionalValue;
            map.RemoveMaterial(obj.Position);
            player.AddTailSegment();
        }
    }
}
