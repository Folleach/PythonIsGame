using NUnit.Framework;
using NUnit.Framework.Internal;
using PythonIsGame.Common;
using PythonIsGame.Common.Materials;
using System.Drawing;
using System.Linq;

namespace PythonIsGame.Tests
{
    public class MaterialTests
    {
        [Test]
        public void AppleCollectionTest_ScoreMustBeIncrease_TailMustBeIncrease()
        {
            var player = new Snake(0, 0, TestHelpers.EmptyMap, "");
            var appleMaterial = new AppleMaterial();

            appleMaterial.IntersectedWithSnake(player);

            Assert.AreEqual(2, player.GetEntities().Count());
            Assert.AreEqual(1, player.Score);
        }

        [Test]
        public void TeleportTest_ScoreMustBeIncrease_TailMustBeIncrease()
        {
            var player = new Snake(0, 0, TestHelpers.EmptyMap, "");
            var teleportMaterial = new TeleportMaterial(new Point(33, -9));

            teleportMaterial.Teleport(player.Head);

            Assert.AreEqual(33, player.X);
            Assert.AreEqual(-9, player.Y);
        }

        [Test]
        public void WallTest_SnakeMustBeDie()
        {
            var player = new Snake(33, -9, TestHelpers.EmptyMap, "");
            var wallMaterial = new WallMaterial();

            player.Died += s => Assert.Pass("Player was killed.");

            wallMaterial.IntersectedWithSnake(player);

            Assert.Fail("Player was not killed.");
        }
    }
}
