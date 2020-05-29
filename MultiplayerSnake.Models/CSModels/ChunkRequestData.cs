using Folleach.StreamNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MultiplayerSnake.Models.CSModels
{
    [Serializable]
    public class ChunkRequestData : IPacket
    {
        public int X { get; set; }
        public int Y { get; set; }

        public ChunkRequestData(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static ChunkRequestData Unpack(byte[] data)
        {
            var binary = new BinaryFormatter();
            var stream = new MemoryStream(data);
            return (ChunkRequestData)binary.Deserialize(stream);
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
