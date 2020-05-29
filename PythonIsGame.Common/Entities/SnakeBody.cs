using System;
using System.Drawing;

namespace PythonIsGame.Common.Entities
{
    [Serializable]
    public class SnakeBody : Entity
    {
        public SnakeBody(int x, int y)
        {
            Position = new Point(x, y);
        }
    }
}
