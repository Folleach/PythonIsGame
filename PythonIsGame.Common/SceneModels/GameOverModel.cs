namespace PythonIsGame.Common.SceneModels
{
    public class GameOverModel
    {
        public bool IsWin { get; set; }
        public int Score { get; set; }
        public string Message { get; set; }
        public Scene GameScene { get; set; }
    }
}
