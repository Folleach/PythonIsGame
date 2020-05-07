using PythonIsGame.Common;
using PythonIsGame.Common.Entities;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using PythonIsGame.Common.SceneModels;
using PythonIsGame.Common.Scenes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClassicSnake
{
    public class ClassicSnake : DefaultGameScene
    {
        private SceneManager sceneManager;

        private Label scoreboard = new Label();

        private int Left = 0;
        private int Up = 0;
        private int Right = 60;
        private int Bottom = 30;

        public override void Create(SceneManager ownerManager, object data)
        {
            sceneManager = ownerManager;
            base.Create(ownerManager, data);
            map = new ChunkedMap(new AreaMapGenerator(Left, Up, Right, Bottom, false), 1);
            player = new Snake(2, 2, map, "player", true);
            map.AddEntity(player.Head, true);
            map.RegisterIntersectionWithMaterial(player.Head, typeof(WallMaterial), material => GameOver("Ударился об стенку и умер... Вот вопрос, зачем он полез на стенку?"));
            map.RegisterIntersectionWithMaterial(player.Head, typeof(AppleMaterial), IntersectWithFood);
            map.RegisterIntersectionWithEntity(player.Head, typeof(SnakeBody), entity => GameOver("Укусил себя и умер..."));
            KeyDownHandlers[Keys.A] = e => player.Direction = Direction.Left;
            KeyDownHandlers[Keys.W] = e => player.Direction = Direction.Up;
            KeyDownHandlers[Keys.D] = e => player.Direction = Direction.Right;
            KeyDownHandlers[Keys.S] = e => player.Direction = Direction.Down;
            scoreboard.BackColor = Color.Transparent;
            AddControl(scoreboard);
            map.SetMaterial(new AppleMaterial(), GetRandomPointInArea());
        }

        public override void Update(TimeSpan delta)
        {
            camera.TargetPosition = new Point(player.X - (int)(Width / (2 * camera.Scale)), player.Y - (int)(Height / (2 * camera.Scale)));
            camera.Update();
            player.Update();
            map.Update();
        }

        private void GameOver(string message)
        {
            sceneManager.ReplaceScene(new GameOverScene(), new GameOverModel()
            {
                Message = message,
                Score = player.Score,
                GameScene = this
            });
        }

        private void IntersectWithFood(PositionMaterial obj)
        {
            var apple = obj.Material as AppleMaterial;
            player.Score += apple.NutritionalValue;
            map.RemoveMaterial(obj.Position);
            map.SetMaterial(new AppleMaterial(), GetRandomPointInArea());
            scoreboard.Text = "Очки: " + player.Score;
            player.AddTailSegment();
        }

        private Point GetRandomPointInArea()
        {
            var random = new Random();
            return new Point(Left + random.Next(Right - Left), Up + random.Next(Bottom - Up));
        }
    }
}
