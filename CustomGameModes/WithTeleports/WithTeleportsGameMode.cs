using PythonIsGame.Common;

namespace CustomGameModes.WithTeleports
{
    public class WithTeleportsGameMode : IGameMode
    {
        public string GameModeName { get; } = "Демка с телепортами";

        public Scene CreateGameScene()
        {
            return new WithTeleportsScene();
        }

        public void Initialize()
        {
        }
    }
}
