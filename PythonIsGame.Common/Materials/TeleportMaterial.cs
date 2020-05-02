using System.Drawing;

namespace PythonIsGame.Common.Materials
{
    public class TeleportMaterial : IMaterial
    {
        public Color Color { get; set; } = Color.FromArgb(163, 82, 191);
        private Point teleportPosition;

        public TeleportMaterial(Point teleportPosition)
        {
            this.teleportPosition = teleportPosition;
        }

        public void Teleport(IEntity entity)
        {
            entity.Position = teleportPosition;
        }
    }
}
