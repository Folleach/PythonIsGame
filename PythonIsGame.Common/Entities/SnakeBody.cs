using System.Drawing;

namespace PythonIsGame.Common.Entities
{
    public class SnakeBody : Entity
    {
        public SnakeBody(int x, int y)
        {
            Position = new Point(x, y);
        }
    }
}
