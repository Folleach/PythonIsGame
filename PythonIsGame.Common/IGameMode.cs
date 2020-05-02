using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Common
{
    public interface IGameMode
    {
        string GameModeName { get; }
        Scene CreateGameScene();
        void Initialize();
    }
}
