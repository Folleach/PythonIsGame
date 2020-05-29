using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Common
{
    public class Camera
    {
        public PointF Position { get; private set; }
        public PointF TargetPosition { get; set; }
        public float Velocity { get; set; } = 0.1f;
        public bool Smooth { get; set; }
        public float Scale { get; set; } = 16f;

        public Camera()
        {
        }

        public void Update()
        {
            if (Smooth)
            {
                var vector = new PointF(TargetPosition.X - Position.X, TargetPosition.Y - Position.Y);
                Position = new PointF(Position.X + vector.X * Velocity, Position.Y + vector.Y * Velocity);
            }
            else
            {
                Position = TargetPosition;
            }
        }

        public PointF GetTransformPosition()
        {
            return new PointF(-Position.X, -Position.Y);
        }
    }
}
