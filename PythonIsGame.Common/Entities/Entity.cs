using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Common.Entities
{
    public class Entity : IEntity
    {
        public Point Position { get; set; }
        public Color Color { get; set; }

        public bool Intersect(IEntity other)
        {
            return Position.X == other.Position.X && Position.Y == other.Position.Y;
        }
    }
}
