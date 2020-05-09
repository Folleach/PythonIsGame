using PythonIsGame.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace PythonIsGame.Common
{
    public class Snake : IEnumerable<IEntity>
    {
        public int Score = 0;
        public readonly string Name;

        public Direction Direction
        {
            get => direction;
            set
            {
                if (CanTurn(value))
                    direction = value;
            }
        }
        protected Direction direction;
        protected Direction previousStepDirection;

        public SnakeHead Head { get; private set; }
        protected LinkedList<SnakeBody> tail = new LinkedList<SnakeBody>();
        protected IMap map;

        public int X => Head.Position.X;
        public int Y => Head.Position.Y;

        public Snake(int x, int y, IMap map, string name, bool chunkFollow = false)
        {
            Name = name;
            Head = new SnakeHead(x, y);
            previousStepDirection = direction = Direction.None;
            this.map = map;
            map.AddEntity(Head, chunkFollow);
        }

        public virtual void Update()
        {
            if (tail.Count > 0)
            {
                var t = tail.Last;
                tail.RemoveLast();
                tail.AddFirst(t);
                tail.First.Value.Position = Head.Position;
            }
            var delta = GetDeltaPointBy(Head.Position, direction);
            Head.Position = new Point(Head.Position.X + delta.X, Head.Position.Y + delta.Y);
            previousStepDirection = direction;
        }

        public void AddTailSegment()
        {
            var delta = GetDeltaPointBy(Head.Position, direction);
            var body = new SnakeBody(Head.Position.X - delta.X, Head.Position.Y - delta.Y);
            tail.AddLast(body);
            map.AddEntity(body, false);
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            yield return Head;
            foreach (var bodyItem in tail)
                yield return bodyItem;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool CanTurn(Direction to)
        {
            return (to == Direction.Left && previousStepDirection != Direction.Right)
                || (to == Direction.Up && previousStepDirection != Direction.Down)
                || (to == Direction.Right && previousStepDirection != Direction.Left)
                || (to == Direction.Down && previousStepDirection != Direction.Up);
        }

        private Point GetDeltaPointBy(Point point, Direction direction)
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
    }
}
