using PythonIsGame.Common.Algorithms;
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
        }

        protected override void Step()
        {
            if (directions.Count == 0)
                directions = BFS.FindPath(map, Head.Position, typeof(AppleMaterial), CanStepTo);
            Direction = directions.First.Value;
            directions.RemoveFirst();
            StepTo(currentDirection);
            map.Update();
        }

        private bool CanStepTo(Point point)
        {
            var material = map.GetMaterial(point).Material;
            if (material == null)
                return true;
            if (CanNotStep.Contains(material.GetType()))
                return false;
            return map.InsideLoadedMap(point);
        }
    }
}
