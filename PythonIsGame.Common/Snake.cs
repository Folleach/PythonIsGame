using PythonIsGame.Common.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PythonIsGame.Common
{
    public class Snake
    {
        public int Score
        {
            get => score;
            set
            {
                score = value;
                ScoreChanged?.Invoke(this, value);
            }
        }
        private int score = 0;
        public readonly string Name;

        public Point Position
        {
            get => Head.Position;
            set
            {
                MoveTailTo(Head.Position);
                Head.Position = value;
            }
        }

        public event Action<Snake, Direction> Stepped;
        public event Action<Snake> Died;
        public event Action<Snake, int> ScoreChanged;
        public event Action<Snake, float> SpeedChanged;

        public bool Alive { get; private set; } = true;

        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                timeThresholdInMs = (int)(1000 / value);
                SpeedChanged?.Invoke(this, value);
            }
        }
        private float speed = 1;
        private int timeThresholdInMs = 1000;
        private int currentTime;

        public Direction Direction
        {
            get => currentDirection;
            set
            {
                if (CanTurn(value))
                    currentDirection = value;
            }
        }
        protected Direction currentDirection;
        private Direction previousStepDirection;

        public SnakeHead Head { get; private set; }
        protected LinkedList<SnakeBody> tail = new LinkedList<SnakeBody>();
        protected IMap map;

        public Snake(int x, int y, IMap map, string name, bool chunkFollow = false)
        {
            Name = name;
            Head = new SnakeHead(x, y);
            previousStepDirection = currentDirection = Direction.None;
            this.map = map;
            map.AddEntity(Head, chunkFollow);
            Speed = 16f;
        }

        public virtual void Update(TimeSpan delta)
        {
            if (!Alive)
                return;
            currentTime += delta.Milliseconds;
            if (currentTime > timeThresholdInMs)
            {
                currentTime = 0;
                Step();
            }
        }

        protected virtual void Step()
        {
            StepTo(currentDirection);
        }

        public void StepTo(Direction direction)
        {
            if (!CanTurn(direction))
                return;
            var delta = GetDeltaPointBy(direction);
            Position = new Point(Head.Position.X + delta.X, Head.Position.Y + delta.Y);
            previousStepDirection = direction;
            Stepped?.Invoke(this, direction);
        }

        public virtual void AddTailSegment()
        {
            var delta = GetDeltaPointBy(currentDirection);
            var body = new SnakeBody(Head.Position.X - delta.X, Head.Position.Y - delta.Y);
            tail.AddLast(body);
            map.AddEntity(body, false);
        }

        public virtual void RemoveTailSegment()
        {
            throw new NotImplementedException();
        }

        public void Kill()
        {
            foreach (var entity in GetEntities())
                map.RemoveEntity(entity);
            Alive = false;
            tail = null;
            Head = null;
            Died?.Invoke(this);
        }

        public IEnumerable<IEntity> GetEntities()
        {
            if (!Alive)
                yield break;
            yield return Head;
            foreach (var bodyItem in tail)
                yield return bodyItem;
        }

        protected bool CanTurn(Direction to)
        {
            return (to == Direction.Left && previousStepDirection != Direction.Right)
                || (to == Direction.Up && previousStepDirection != Direction.Down)
                || (to == Direction.Right && previousStepDirection != Direction.Left)
                || (to == Direction.Down && previousStepDirection != Direction.Up);
        }

        protected Point GetDeltaPointBy(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Point(-1, 0);
                case Direction.Right:
                    return new Point(1, 0);
                case Direction.Up:
                    return new Point(0, -1);
                case Direction.Down:
                    return new Point(0, 1);
                default:
                    return Point.Empty;
            }
        }

        private void MoveTailTo(Point position)
        {
            if (tail.Count > 0)
            {
                var t = tail.Last;
                tail.RemoveLast();
                tail.AddFirst(t);
                tail.First.Value.Position = position;
            }
        }
    }
}
