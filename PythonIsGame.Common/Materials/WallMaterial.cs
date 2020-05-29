using System;

namespace PythonIsGame.Common.Materials
{
    [Serializable]
    public class WallMaterial : IMaterial
    {
        public void IntersectedWithSnake(Snake snake)
        {
            snake.Kill();
        }
    }
}
