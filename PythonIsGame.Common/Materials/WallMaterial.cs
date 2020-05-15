namespace PythonIsGame.Common.Materials
{
    public class WallMaterial : IMaterial
    {
        public void IntersectedWithSnake(Snake snake)
        {
            snake.Kill();
        }
    }
}
