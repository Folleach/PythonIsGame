using PythonIsGame.Common;

namespace ClassicSnake
{
    public class ClassicSnakeMode : IGameMode
    {
        public string GameModeName => "Классическая змейка";

        public Scene CreateGameScene()
        {
            return new ClassicSnake();
        }

        public void Initialize()
        {
        }
    }
}
