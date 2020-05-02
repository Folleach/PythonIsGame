using System.Drawing;

namespace PythonIsGame.Common
{
    public interface IEntity
    {
        Point Position { get; set; }
        Color Color { get; set; }

        bool Intersect(IEntity other);
    }
}
