using Folleach.StreamNet.Common;
using PythonIsGame.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MultiplayerSnake.Models.SCModels
{
    [Serializable]
    public class ChunkData : IPacket
    {
        public Chunk ChunkObject { get; set; }

        public static ChunkData Unpack(byte[] data)
        {
            var binary = new BinaryFormatter();
            var stream = new MemoryStream(data);
            return (ChunkData)binary.Deserialize(stream);
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
