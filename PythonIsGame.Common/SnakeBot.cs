using PythonIsGame.Common.Algorithms;
using PythonIsGame.Common.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Common
{
    public class SnakeBot : Snake
    {
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                timeThresholdInMs = (int)(1000 / value);
            }
        }
        private float speed = 1;
        private int timeThresholdInMs = 1000;
        private int currentTime;

        private HashSet<Type> CanNotStep = new HashSet<Type>();
        private LinkedList<Direction> directions = new LinkedList<Direction>();

        public SnakeBot(int x, int y, IMap map, string name) : base(x, y, map, name, false)
        {
            Direction = Direction.Left;
        }

        public void Update(TimeSpan delta)
        {
            if (!Alive)
                return;
            currentTime += delta.Milliseconds;
            if (currentTime > timeThresholdInMs)
            {
                if (directions.Count == 0)
                {
                    directions = BFS.FindPath(map, Head.Position, typeof(AppleMaterial), CanStepTo);
                    currentTime = -3;
                    return;
                }
                Direction = directions.First.Value;
                directions.RemoveFirst();
                StepTo(currentDirection);
                map.Update();
                currentTime = 0;
            }
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
