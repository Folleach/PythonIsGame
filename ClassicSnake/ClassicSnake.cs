using PythonIsGame.Common;
using PythonIsGame.Common.Entities;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using PythonIsGame.Common.SceneModels;
using PythonIsGame.Common.Scenes;
using PythonIsGame.Common.UI;
using System;

namespace ClassicSnake
{
    public class ClassicSnake : DefaultGameScene
    {
        private SceneManager sceneManager;

        private DrawLabel scoreboard = new DrawLabel(12, true);

        private int Left = 0;
        private int Up = 0;
        private int Right;
        private int Bottom;

        private AreaMapGenerator mapGenerator;

        public override void Create(SceneManager ownerManager, object data)
        {
            sceneManager = ownerManager;
            base.Create(ownerManager, data);
            Right = (int)Math.Round(Width / camera.Scale - 2);
            Bottom = (int)Math.Round(Height / camera.Scale - 3);
            map = new ChunkedMap(mapGenerator = new AreaMapGenerator(Left, Up, Right, Bottom, true), 1);
            player = new Snake(2, 2, map, "player", true);
            player.Speed = 8;
            player.Stepped += (s, d) => map.Update();
            player.Died += s => GameOver("Игра окончена");
            player.ScoreChanged += (sender, score) => scoreboard.Text = "Очки: " + sender.Score;
            map.AddEntity(player.Head, true);
            map.RegisterIntersectionWithMaterial(player.Head, typeof(TeleportMaterial), m => (m.Material as TeleportMaterial).Teleport(player.Head));
            map.RegisterIntersectionWithMaterial(player.Head, typeof(AppleMaterial), IntersectWithFood);
            map.RegisterIntersectionWithEntity(player.Head, typeof(SnakeBody), e => player.Kill());
            colorMapping[typeof(TeleportMaterial)] = GameColors.WallMaterialColor;
            scoreboard.Width = 180;
            AddControl(scoreboard);
            map.SetMaterial(new AppleMaterial(), mapGenerator.GetRandomPointInArea());
        }

        public override void Update(TimeSpan delta)
        {
            player.Update(delta);
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
            apple.IntersectedWithSnake(player);
            map.RemoveMaterial(obj.Position);
            map.SetMaterial(new AppleMaterial(), mapGenerator.GetRandomPointInArea());
        }
    }
}
