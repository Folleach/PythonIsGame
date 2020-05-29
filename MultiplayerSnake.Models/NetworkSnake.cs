using PythonIsGame.Common;
using PythonIsGame.Common.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Models
{
    public class NetworkSnake : Snake
    {
        public Guid GUID { get; private set; }

        public NetworkSnake(int x, int y, IMap map, string name, Guid guid, bool chunkFollow = false) : base(x, y, map, name, chunkFollow)
        {
            GUID = guid;
            Direction = Direction.Right;
        }

        public void AddTailSegment(Point position)
        {
            Point pos = default;
            if (tail.Count > 0)
                pos = tail.Last.Value.Position;
            else
                pos = Position;
            var body = new NetworkBody(this, pos.X, pos.Y);
            tail.AddLast(body);
            map.AddEntity(body, false);
        }

        public override void AddTailSegment()
        {
            NetworkBody body = null;
            if (tail.Count == 0)
            {
                var delta = GetDeltaPointBy(currentDirection);
                body = new NetworkBody(this, Head.Position.X - delta.X, Head.Position.Y - delta.Y);
            }
            else
            {
                var pos = tail.Last.Value.Position;
                body = new NetworkBody(this, pos.X, pos.Y);
            }
            tail.AddLast(body);
            map.AddEntity(body, false);
        }
    }
}
