using NeuralAI.AI;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralAI
{
    public class NeuralAIMain : DefaultGameScene
    {
        private SceneManager sceneManager;

        private DrawLabel scoreboard = new DrawLabel(12, true);

        private int Left = 0;
        private int Up = 0;
        private int Right;
        private int Bottom;

        private AreaMapGenerator mapGenerator;

        private NeuralNetwork<Direction> network;

        private Point startPosition = new Point(8, 6);
        private int countToOver = 300;
        private int remember = 300;

        Point targetPoint;
        private double oldDistance = double.PositiveInfinity;

        public override void Create(SceneManager ownerManager, object data)
        {
            sceneManager = ownerManager;
            base.Create(ownerManager, data);
            Right = (int)Math.Round(Width / camera.Scale - 2);
            Bottom = (int)Math.Round(Height / camera.Scale - 3);
            map = new ChunkedMap(mapGenerator = new AreaMapGenerator(Left, Up, Right, Bottom, false), 1);
            player = new Snake(startPosition.X, startPosition.Y, map, "player", true);
            player.ScoreChanged += (sender, score) => scoreboard.Text = "Очки: " + sender.Score;
            map.AddEntity(player.Head, true);
            map.RegisterIntersectionWithMaterial(player.Head, typeof(WallMaterial), m => GameOver());
            map.RegisterIntersectionWithMaterial(player.Head, typeof(AppleMaterial), IntersectWithFood);
            map.RegisterIntersectionWithEntity(player.Head, typeof(SnakeBody), e => GameOver());
            colorMapping[typeof(TeleportMaterial)] = GameColors.WallMaterialColor;
            scoreboard.Width = 280;
            AddControl(scoreboard);
            map.SetMaterial(new AppleMaterial(), targetPoint = mapGenerator.GetRandomPointInArea());
            gradient = true;

            KeyDownHandlers[Keys.Space] = e => Step();
            KeyDownHandlers[Keys.Y] = e => network.ProcessTrack(1);
            KeyDownHandlers[Keys.N] = e => network.ProcessTrack(-1);
            InitializeAI();
        }

        private void Step()
        {
            Direction direction = network.Process(GetData());
            player.StepTo(direction);
            map.Update();
            --remember;
            scoreboard.Text = $"Remember: {remember}";
            if (remember < 0)
            {
                GameOver();
                return;
            }
            double distance = GetDistance(player.Position, targetPoint);
            if (oldDistance != double.PositiveInfinity && distance > oldDistance)
                network.ProcessTrack(-0.3f);
            else if (oldDistance != double.PositiveInfinity && distance < oldDistance)
                network.ProcessTrack(0.3f);
            oldDistance = distance;
        }

        private double GetDistance(Point a, Point b)
        {
            return Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
        }

        private void InitializeAI()
        {
            network = new NeuralNetwork<Direction>(
                AIConverter,
                new int[] { 8, 12, 12, 4 },
                0.3f);
        }

        private Direction AIConverter(float[] data)
        {
            Direction direction = Direction.None;
            float max = float.NegativeInfinity;
            for (int i = 0; i < data.Length; ++i)
            {
                if (data[i] > max)
                {
                    direction = (Direction)(i + 1);
                    max = data[i];
                }
            }
            if (direction == Direction.None)
                throw new Exception("Rip.");
            return direction;
        }

        private float[] GetData()
        {
            HashSet<Point> entities = new HashSet<Point>(map.GetEntities().Select(x => x.Position));
            //float[] result = new float[(Right + 1) * (Bottom + 1)];
            //int index = 0;
            //for (int x = 0; x < Right + 1; ++x)
            //{
            //    for (int y = 0; y < Bottom + 1; ++y)
            //    {
            //        Point point = new Point(x, y);
            //        IMaterial material = map.GetMaterial(point).Material;
            //        bool hasEntity = entities.Contains(point);
            //        if (!hasEntity && material == null)
            //            result[index] = 0.7f;
            //        else if (hasEntity || material is WallMaterial)
            //            result[index] = 0;
            //        else if (map.GetMaterial(point).Material is AppleMaterial)
            //            result[index] = 1f;
            //        else
            //            throw new Exception("Unknow material or entity.");
            //        ++index;
            //    }
            //}
            //return result;
            bool hasObstacle(int x, int y)
            {
                Point target = new Point(x, y);
                IMaterial material = map.GetMaterial(target).Material;
                return entities.Contains(target) || (material != null && (material is WallMaterial || material is SnakeBody));
            }
            int dx = targetPoint.X - player.Position.X;
            int dy = targetPoint.Y - player.Position.Y;
            float[] result = new float[8];
            result[0] = dx < 0 ? 1 : dx == 0 ? 0.5f : 0;
            result[1] = dy < 0 ? 1 : dy == 0 ? 0.5f : 0;
            result[2] = dx > 0 ? 1 : dx == 0 ? 0.5f : 0;
            result[3] = dy > 0 ? 1 : dy == 0 ? 0.5f : 0;
            result[4] = hasObstacle(player.Position.X - 1, player.Position.Y) ? 0 : 1;
            result[5] = hasObstacle(player.Position.X, player.Position.Y - 1) ? 0 : 1;
            result[6] = hasObstacle(player.Position.X + 1, player.Position.Y) ? 0 : 1;
            result[7] = hasObstacle(player.Position.X, player.Position.Y + 1) ? 0 : 1;
            return result;
        }

        public override void Update(TimeSpan delta)
        {
            Step();
        }

        private void GameOver()
        {
            player.Position = startPosition;
            player.RemoveAllTail();
            network.ProcessTrack(-4);
            oldDistance = double.PositiveInfinity;
            remember = countToOver;
        }

        private void IntersectWithFood(PositionMaterial obj)
        {
            var apple = obj.Material as AppleMaterial;
            apple.IntersectedWithSnake(player);
            map.RemoveMaterial(obj.Position);
            map.SetMaterial(new AppleMaterial(), targetPoint = mapGenerator.GetRandomPointInArea());
            network.ProcessTrack(4);
            remember = countToOver;
            oldDistance = double.PositiveInfinity;
            PlaySound("Sounds/hit.wav");
        }
    }
}
