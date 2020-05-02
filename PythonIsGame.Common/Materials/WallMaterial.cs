using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Common.Materials
{
    public class WallMaterial : IMaterial
    {
        public Color Color { get; set; } = Color.FromArgb(18, 13, 20);
    }
}
