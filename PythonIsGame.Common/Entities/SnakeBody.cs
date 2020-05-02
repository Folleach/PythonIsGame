using System.Drawing;

namespace PythonIsGame.Common.Entities
{
    public class SnakeBody : Entity
    {
        public SnakeBody(int x, int y)
        {
            Position = new Point(x, y);
            Color = Color.FromArgb(84, 187, 74);
        }
    }
}
