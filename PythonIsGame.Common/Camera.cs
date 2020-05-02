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
        public Point Position { get; private set; }
        public Point TargetPosition { get; set; }
        public float Velocity { get; set; }
        public bool Smooth { get; set; }
        public float Scale { get; set; } = 16f;

        public Camera()
        {
        }

        public void Update()
        {
            if (Smooth)
            {
                // TODO: Сглаживание движения
            }
            else
            {
                Position = TargetPosition;
            }
        }

        public Point GetTransformPosition()
        {
            return new Point(-Position.X, -Position.Y);
        }
    }
}
