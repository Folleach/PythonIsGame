using Folleach.StreamNet.Common;
using PythonIsGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Models.CSModels
{
    public class ChangeDirection : IPacket
    {
        public ChangeDirection(Direction direction)
        {
            Direction = direction;
        }

        public Direction Direction { get; set; }

        public static ChangeDirection Unpack(byte[] data)
        {
            return new ChangeDirection((Direction)data[0]);
        }

        public byte[] Pack()
        {
            return BitConverter.GetBytes((byte)Direction);
        }
    }
}
