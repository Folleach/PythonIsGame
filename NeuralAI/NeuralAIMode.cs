using PythonIsGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralAI
{
    public class NeuralAIMode : IGameMode
    {
        public string GameModeName => "Нейронная сеть";

        public int Order => 5;

        public Scene CreateGameScene()
        {
            return new NeuralAIMain();
        }

        public void Initialize()
        {
        }
    }
}
