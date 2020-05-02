using System.Drawing;

namespace PythonIsGame.Common
{
    public class PositionMaterial
    {
        public IMaterial Material;
        public Point Position;

        public PositionMaterial(IMaterial material, Point position)
        {
            Material = material;
            Position = position;
        }
    }
}
