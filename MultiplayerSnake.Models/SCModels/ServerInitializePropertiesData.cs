using Folleach.StreamNet.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MultiplayerSnake.Models.SCModels
{
    [Serializable]
    public class ServerInitializePropertiesData : IPacket
    {
        public int ChunkSize { get; set; }
        public int InitX { get; set; }
        public int InitY { get; set; }
        public Guid GUID { get; set; }
        public int LeaderboardCount { get; set; }
        public Rectangle MapArea { get; set; }

        public static ServerInitializePropertiesData Unpack(byte[] data)
        {
            var binary = new BinaryFormatter();
            var stream = new MemoryStream(data);
            return (ServerInitializePropertiesData)binary.Deserialize(stream);
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
