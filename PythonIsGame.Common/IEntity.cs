using System.Drawing;

namespace PythonIsGame.Common
{
    public interface IEntity
    {
        Point Position { get; set; }

        bool Intersect(IEntity other);
    }
}
