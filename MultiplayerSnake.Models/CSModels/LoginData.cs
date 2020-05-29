using Folleach.StreamNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MultiplayerSnake.Models.CSModels
{
    [Serializable]
    public class LoginData : IPacket
    {
        public string UserName { get; set; }

        public static LoginData Unpack(byte[] data)
        {
            var binary = new BinaryFormatter();
            var stream = new MemoryStream(data);
            return (LoginData)binary.Deserialize(stream);
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
