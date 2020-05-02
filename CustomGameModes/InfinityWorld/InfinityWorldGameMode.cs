using PythonIsGame.Common;

namespace CustomGameModes.InfinityWorld
{
    public class InfinityWorldGameMode : IGameMode
    {
        public string GameModeName { get; } = "Бесконечный мир";

        public Scene CreateGameScene()
        {
            return new InfinityWorldScene();
        }

        public void Initialize()
        {
        }
    }
}
