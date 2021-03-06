﻿using PythonIsGame.Common;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using PythonIsGame.Common.SceneModels;
using PythonIsGame.Common.Scenes;
using PythonIsGame.Common.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CustomGameModes.InfinityWorld
{
    public class InfinityWorldScene : DefaultGameScene
    {
        private DrawLabel positionLabel = new DrawLabel(12, true);
        private DrawLabel seedLabel = new DrawLabel(12, true);

        public override void Create(SceneManager ownerManager, object data)
        {
            base.Create(ownerManager, data);
            int seed;
            if (data is int)
                seed = (int)data;
            else
                seed = new Random().Next();
            map = new ChunkedMap(new InfinityWorldGenerator(seed), 1);
            player = new Snake(2, 2, map, "player", true);
            map.RegisterIntersectionWithMaterial(player.Head, typeof(WallMaterial), material => GameOver(ownerManager));
            map.RegisterIntersectionWithMaterial(player.Head, typeof(AppleMaterial), PlayerEat);
            seedLabel.Text = $"Seed: {seed}";
            seedLabel.Top = positionLabel.Top + positionLabel.Height;
            positionLabel.Width = seedLabel.Width = 300;
            AddControl(positionLabel);
            AddControl(seedLabel);
            camera.Smooth = true;
        }

        public override void Update(TimeSpan delta)
        {
            player.Update(delta);
            camera.TargetPosition = new Point(player.Position.X - (int)(Width / (2 * camera.Scale)), player.Position.Y - (int)(Height / (2 * camera.Scale)));
            camera.Update();
            positionLabel.Text = $"Position: ({player.Position.X}, {player.Position.Y})";
            map.Update();
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
            PlaySound("Sounds/hit.wav");
        }
    }
}
