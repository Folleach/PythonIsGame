using MultiplayerSnake.Scenes;
using PythonIsGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake
{
    public class MultiplayerGameMode : IGameMode
    {
        public string GameModeName { get; } = "Мультиплеер";

        public int Order => 4;

        public Scene CreateGameScene()
        {
            return new ChoiceServerScene(); 
        }

        public void Initialize()
        {
        }
    }
}
