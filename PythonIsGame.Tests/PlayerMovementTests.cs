using NUnit.Framework;
using PythonIsGame.Common;
using PythonIsGame.Common.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Tests
{
    [TestFixture]
    public class PlayerMovementTests
    {
        private ChunkedMap EmptyMap = new ChunkedMap(new EmptyMapGenerator());

        [Test]
        public void SimpleMove_ShouldBeMoveToRight()
        {
            HeadMove(0, 0, 1, 0, Direction.Right);
        }

        [Test]
        public void SimpleMove_ShouldBeMoveToDown()
        {
            HeadMove(0, 0, 0, 1, Direction.Down);
        }

        [Test]
        public void SimpleMove_ShouldBeMoveToLeft()
        {
            HeadMove(0, 0, -1, 0, Direction.Left);
        }

        [Test]
        public void SimpleMove_ShouldBeMoveToUp()
        {
            HeadMove(0, 0, 0, -1, Direction.Up);
        }

        [Test]
        public void SimpleMove_SouldNotMoveToBack()
        {
            HeadMove(0, 0, 3, 0, Direction.Right, Direction.Left, Direction.Left);
        }

        [Test]
        public void SimpleMove_ShouldNotMove()
        {
            HeadMove(10, 3, 10, 3, Direction.None, Direction.None, Direction.None);
        }

        [Test]
        public void DoubleChangeDirection_SnakeShouldNotTurnToBack()
        {
            var player = new Snake(0, 0, EmptyMap, "player");
            player.Direction = Direction.Right;
            player.Update();
            player.Direction = Direction.Up;
            player.Direction = Direction.Down;
            player.Direction = Direction.Left;
            player.Update();
            Assert.AreEqual(1, player.X);
            Assert.AreEqual(1, player.Y);
            Assert.AreEqual(Direction.Down, player.Direction);
        }

        public void HeadMove(int x, int y, int expectedX, int expectedY, params Direction[] directions)
        {
            var player = new Snake(x, y, EmptyMap, "player");
            foreach (var direction in directions)
            {
                player.Direction = direction;
                player.Update();
            }
            Assert.AreEqual(expectedX, player.X);
            Assert.AreEqual(expectedY, player.Y);
        }
    }
}
