using PythonIsGame.Common;
using PythonIsGame.Common.Entities;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using PythonIsGame.Common.SceneModels;
using PythonIsGame.Common.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomGameModes.CompetitionWithBot
{
    public class CompetitionScene : DefaultGameScene
    {
        private int Left = 0;
        private int Up = 0;
        private int Right = 60;
        private int Bottom = 30;

        private SnakeBot bot;
        private SceneManager sceneManager;

        public override void Create(SceneManager ownerManager, object data)
        {
            sceneManager = ownerManager;
            base.Create(ownerManager, data);
            Map = new ChunkedMap(new AreaMapGenerator(Left, Up, Right, Bottom));
            Player = new Snake(2, 2, Map, "player", true);
            bot = new SnakeBot(10, 10, Map, "bot");
            bot.Speed = 6f;
            Map.RegisterIntersectionWithMaterial(Player.Head, typeof(AppleMaterial), m => IntersectWithFood(Player, m));
            Map.RegisterIntersectionWithMaterial(bot.Head, typeof(AppleMaterial), m => IntersectWithFood(bot, m));
            Map.RegisterIntersectionWithMaterial(Player.Head, typeof(WallMaterial), m => GameOver("Человек убился об стену, победили машины."));
            Map.RegisterIntersectionWithMaterial(bot.Head, typeof(WallMaterial), m => GameOver("Вы победили!"));
            Map.RegisterIntersectionWithEntity(Player.Head, typeof(SnakeBody), e => GameOver("Алгоритм поиска пути смог поймать человека в ловушку?"));
            Map.RegisterIntersectionWithEntity(bot.Head, typeof(SnakeBody), e => GameOver("Бот был слишком глуп..."));
            KeyDownHandlers[Keys.A] = e => StepInDirection(Direction.Left);
            KeyDownHandlers[Keys.W] = e => StepInDirection(Direction.Up);
            KeyDownHandlers[Keys.D] = e => StepInDirection(Direction.Right);
            KeyDownHandlers[Keys.S] = e => StepInDirection(Direction.Down);
            Map.Update();
            Map.SetMaterial(new AppleMaterial(), new Point(11, 11));
        }

        public override void Update(TimeSpan delta)
        {
            camera.TargetPosition = new Point(Player.X - (int)(Width / (2 * camera.Scale)), Player.Y - (int)(Height / (2 * camera.Scale)));
            camera.Update();
            bot.Update(delta);
        }

        private void GameOver(string message)
        {
            sceneManager.ReplaceScene(new GameOverScene(), new GameOverModel()
            {
                Message = message,
                Score = Player.Score,
                GameScene = this
            });
        }

        private void StepInDirection(Direction direction)
        {
            Player.Direction = direction;
            Player.Update();
            Map.Update();
        }

        private void IntersectWithFood(Snake snake, PositionMaterial obj)
        {
            var apple = obj.Material as AppleMaterial;
            snake.Score += apple.NutritionalValue;
            Map.RemoveMaterial(obj.Position);
            Map.SetMaterial(new AppleMaterial(), GetRandomPoint());
            snake.AddTailSegment();
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            foreach (var obj in bot)
                graphics.FillRectangle(GetBrush(obj.Color), new Rectangle(obj.Position, DefaultSize));
        }

        private Point GetRandomPoint()
        {
            var random = new Random();
            return new Point(Left + random.Next(Right - Left), Up + random.Next(Bottom - Up));
        }
    }
}
