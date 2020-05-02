using PythonIsGame.Common;

namespace CustomGameModes.CompetitionWithBot
{
    public class CompetitionGameMode : IGameMode
    {
        public string GameModeName { get; } = "Соревнование с ботом";

        public Scene CreateGameScene()
        {
            return new CompetitionScene();
        }

        public void Initialize()
        {
        }
    }
}
