namespace PythonIsGame.Common
{
    public interface IGameMode
    {
        string GameModeName { get; }
        int Order { get; }
        Scene CreateGameScene();
        void Initialize();
    }
}
