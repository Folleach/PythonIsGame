using System.Drawing;

namespace PythonIsGame.Common.Materials
{
    public class TeleportMaterial : IMaterial
    {
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
