using PythonIsGame.Common.Algorithms;
using PythonIsGame.Common.Entities;
using PythonIsGame.Common.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PythonIsGame.Common
{
    public class SnakeBot : Snake
    {
        private HashSet<Type> CanNotStep = new HashSet<Type>();
        private LinkedList<Direction> directions = new LinkedList<Direction>();

        public SnakeBot(int x, int y, IMap map, string name) : base(x, y, map, name, false)
        {
            Direction = Direction.Left;
            CanNotStep.Add(typeof(WallMaterial));
            CanNotStep.Add(typeof(SnakeBody));
            CanNotStep.Add(typeof(SnakeHead));
        }

        protected override void Step()
        {
            if (directions.Count == 0)
                directions = BFS.FindPath(map, Head.Position, typeof(AppleMaterial), CanStepTo);
            if (directions.Count == 0)
            {
                var rand = new Random();
                var dir = (Direction)(1 + rand.Next(4));
                var d = GetDeltaPointBy(dir);
                if (CanStepTo(new Point(Position.X + d.X, Position.Y + d.Y)))
                {
                    StepTo(dir);
                    map.Update();
                }
                StepTo(currentDirection);
                return;
            }
            Direction = directions.First.Value;
            directions.RemoveFirst();
            var delta = GetDeltaPointBy(currentDirection);
            if (!CanStepTo(new Point(Position.X + delta.X, Position.Y + delta.Y)))
            {
                directions.Clear();
                return;
            }
            StepTo(currentDirection);
            map.Update();
        }

        private bool CanStepTo(Point point)
        {
            var material = map.GetMaterial(point).Material;
            var entity = map.GetEntity(point);
            if (entity != null && CanNotStep.Contains(entity.GetType()))
                return false;
            if (material != null && CanNotStep.Contains(material.GetType()))
                return false;
            var delta = GetDeltaPointBy(currentDirection);
            return map.InsideLoadedMap(point);
        }
    }
}
