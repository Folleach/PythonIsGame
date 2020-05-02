using System.Drawing;

namespace PythonIsGame.Common.Materials
{
    public class AppleMaterial : IMaterial
    {
        public Color Color { get; set; } = Color.FromArgb(206, 57, 57);
        public int NutritionalValue = 1;
    }
}
