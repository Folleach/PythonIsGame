using Folleach.StreamNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Models.SCModels
{
    [Serializable]
    public class LeaderboardSetData : IPacket
    {
        public Dictionary<string, int> Leaderboard { get; set; }

        public static LeaderboardSetData Unpack(byte[] data)
        {
            var binary = new BinaryFormatter();
            var stream = new MemoryStream(data);
            return (LeaderboardSetData)binary.Deserialize(stream);
        }

        public byte[] Pack()
        {
            var binary = new BinaryFormatter();
            var stream = new MemoryStream();
            binary.Serialize(stream, this);
            return stream.GetBuffer();
        }
    }
}
