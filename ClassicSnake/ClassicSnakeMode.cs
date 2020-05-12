using PythonIsGame.Common;

namespace ClassicSnake
{
    public class ClassicSnakeMode : IGameMode
    {
        public string GameModeName => "Классическая змейка";
        public int Order { get; } = 1;

        public Scene CreateGameScene()
        {
            return new ClassicSnake();
        }

        public void Initialize()
        {
        }
    }
}
