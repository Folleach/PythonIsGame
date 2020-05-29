using Folleach.StreamNet.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Models.SCModels
{
    [Serializable]
    public class UpdateGameStateData : IPacket
    {
        public List<UserStateData> Positions { get; set; } = new List<UserStateData>();

        public static UpdateGameStateData Unpack(byte[] data)
        {
            var binary = new BinaryFormatter();
            var stream = new MemoryStream(data);
            return (UpdateGameStateData)binary.Deserialize(stream);
        }

        public byte[] Pack()
        {
            var binary = new BinaryFormatter();
            var stream = new MemoryStream();
            binary.Serialize(stream, this);
            return stream.GetBuffer();
        }
    }

    [Serializable]
    public class UserStateData
    {
        public Guid GUID { get; set; }
        public Point Position { get; set; }
    }
}
