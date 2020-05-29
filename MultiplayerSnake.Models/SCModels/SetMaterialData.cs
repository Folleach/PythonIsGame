using Folleach.StreamNet.Common;
using PythonIsGame.Common;
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
    public class SetMaterialData : IPacket
    {
        public Point Position { get; set; }
        public IMaterial Material { get; set; }

        public static SetMaterialData Unpack(byte[] data)
        {
            var binary = new BinaryFormatter();
            var stream = new MemoryStream(data);
            return (SetMaterialData)binary.Deserialize(stream);
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
