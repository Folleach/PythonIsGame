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

        private AreaMapGenerator mapGenerator;

        private SnakeBot bot;
        private SceneManager sceneManager;

        public override void Create(SceneManager ownerManager, object data)
        {
            sceneManager = ownerManager;
            base.Create(ownerManager, data);
            map = new ChunkedMap(mapGenerator = new AreaMapGenerator(Left, Up, Right, Bottom));
            player = new Snake(2, 2, map, "player", true);
            InitializeBot();
            map.RegisterIntersectionWithMaterial(player.Head, typeof(AppleMaterial), m => IntersectWithFood(player, m));
            map.RegisterIntersectionWithMaterial(player.Head, typeof(WallMaterial), m => GameOver("Человек убился об стену, победили машины.", false));
            map.RegisterIntersectionWithEntity(player.Head, typeof(SnakeBody), e => GameOver("Алгоритм поиска пути смог поймать человека в ловушку?", false));
            KeyDownHandlers[Keys.A] = e => StepInDirection(Direction.Left);
            KeyDownHandlers[Keys.W] = e => StepInDirection(Direction.Up);
            KeyDownHandlers[Keys.D] = e => StepInDirection(Direction.Right);
            KeyDownHandlers[Keys.S] = e => StepInDirection(Direction.Down);
            map.Update();
            map.SetMaterial(new AppleMaterial(), mapGenerator.GetRandomPointInArea());
        }

        public override void Update(TimeSpan delta)
        {
            bot.Update(delta);
            camera.TargetPosition = new Point(player.X - (int)(Width / (2 * camera.Scale)), player.Y - (int)(Height / (2 * camera.Scale)));
            camera.Update();
        }

        private void GameOver(string message, bool isWin)
        {
            sceneManager.ReplaceScene(new GameOverScene(), new GameOverModel()
            {
                IsWin = isWin,
                Message = message,
                Score = player.Score,
                GameScene = this
            });
        }

        private void InitializeBot()
        {
            var pos = mapGenerator.GetRandomPointInArea();
            bot = new SnakeBot(pos.X, pos.Y, map, "bot");
            bot.Speed = 2000;
            map.RegisterIntersectionWithMaterial(bot.Head, typeof(AppleMaterial), m => IntersectWithFood(bot, m));
            map.RegisterIntersectionWithMaterial(bot.Head, typeof(WallMaterial), m => KillBot());
            map.RegisterIntersectionWithEntity(bot.Head, typeof(SnakeBody), e => KillBot());
        }

        private void KillBot()
        {
            map.UnregisterIntersectionWithMaterial(bot.Head, typeof(AppleMaterial));
            map.UnregisterIntersectionWithMaterial(bot.Head, typeof(WallMaterial));
            map.UnregisterIntersectionWithEntity(bot.Head, typeof(SnakeBody));
            bot.Kill();
            InitializeBot();
        }

        private void StepInDirection(Direction direction)
        {
            player.Direction = direction;
            player.StepTo(direction);
            map.Update();
        }

        private void IntersectWithFood(Snake snake, PositionMaterial obj)
        {
            var apple = obj.Material as AppleMaterial;
            snake.Score += apple.NutritionalValue;
            map.RemoveMaterial(obj.Position);
            map.SetMaterial(new AppleMaterial(), mapGenerator.GetRandomPointInArea());
            snake.AddTailSegment();
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            foreach (var obj in bot.GetEntities())
                graphics.FillRectangle(GetBrush(colorMapping[obj.GetType()]), new Rectangle(obj.Position, DefaultSize));
        }
    }
}
