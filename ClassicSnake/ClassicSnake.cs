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
            Map = new ChunkedMap(new AreaMapGenerator(Left, Up, Right, Bottom, false), 1);
            Player = new Snake(2, 2, Map, "player", true);
            Map.AddEntity(Player.Head, true);
            Map.RegisterIntersectionWithMaterial(Player.Head, typeof(WallMaterial), material => GameOver("Ударился об стенку..."));
            Map.RegisterIntersectionWithMaterial(Player.Head, typeof(AppleMaterial), IntersectWithFood);
            Map.RegisterIntersectionWithEntity(Player.Head, typeof(SnakeBody), entity => GameOver("Укусил себя и умер..."));
            KeyDownHandlers[Keys.A] = e => Player.Direction = Direction.Left;
            KeyDownHandlers[Keys.W] = e => Player.Direction = Direction.Up;
            KeyDownHandlers[Keys.D] = e => Player.Direction = Direction.Right;
            KeyDownHandlers[Keys.S] = e => Player.Direction = Direction.Down;
            scoreboard.BackColor = Color.Transparent;
            AddControl(scoreboard);
            Map.SetMaterial(new AppleMaterial(), GetRandomPoint());
        }

        public override void Update(TimeSpan delta)
        {
            camera.TargetPosition = new Point(Player.X - (int)(Width / (2 * camera.Scale)), Player.Y - (int)(Height / (2 * camera.Scale)));
            camera.Update();
            Player.Update();
            Map.Update();
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

        private void IntersectWithFood(PositionMaterial obj)
        {
            var apple = obj.Material as AppleMaterial;
            Player.Score += apple.NutritionalValue;
            Map.RemoveMaterial(obj.Position);
            Map.SetMaterial(new AppleMaterial(), GetRandomPoint());
            scoreboard.Text = "Очки: " + Player.Score;
            Player.AddTailSegment();
        }

        private Point GetRandomPoint()
        {
            var random = new Random();
            return new Point(Left + random.Next(Right - Left), Up + random.Next(Bottom - Up));
        }
    }
}
