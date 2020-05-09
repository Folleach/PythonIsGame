using System.Drawing;

namespace PythonIsGame.Common.Entities
{
    public class SnakeHead : Entity
    {
        public SnakeHead(int x, int y)
        {
            Position = new Point(x, y);
        }
    }
}
