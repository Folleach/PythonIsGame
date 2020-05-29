using PythonIsGame.Common.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Models
{
    [Serializable]
    public class NetworkBody : SnakeBody
    {
        public NetworkSnake Snake { get; private set; }

        public NetworkBody(NetworkSnake owner, int x, int y) : base(x, y)
        {
            Snake = owner;
        }
    }
}
