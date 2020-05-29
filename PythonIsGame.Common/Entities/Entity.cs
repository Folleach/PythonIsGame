using System;
using System.Drawing;

namespace PythonIsGame.Common.Entities
{
    [Serializable]
    public class Entity : IEntity
    {
        public Point Position { get; set; }

        public bool Intersect(IEntity other)
        {
            return Position.X == other.Position.X && Position.Y == other.Position.Y;
        }
    }
}
