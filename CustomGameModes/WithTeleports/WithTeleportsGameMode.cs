using PythonIsGame.Common;

namespace CustomGameModes.WithTeleports
{
    public class WithTeleportsGameMode : IGameMode
    {
        public string GameModeName { get; } = "Демка с телепортами";

        public int Order { get; } = 10;

        public Scene CreateGameScene()
        {
            return new WithTeleportsScene();
        }

        public void Initialize()
        {
        }
    }
}
