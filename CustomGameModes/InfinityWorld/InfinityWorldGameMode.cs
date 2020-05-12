using PythonIsGame.Common;

namespace CustomGameModes.InfinityWorld
{
    public class InfinityWorldGameMode : IGameMode
    {
        public string GameModeName { get; } = "Бесконечный мир";

        public int Order { get; } = 2;

        public Scene CreateGameScene()
        {
            return new ChoiceSeedScene();
        }

        public void Initialize()
        {
        }
    }
}
