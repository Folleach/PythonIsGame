using NUnit.Framework;
using PythonIsGame.Common;
using System;
using System.Linq;

namespace PythonIsGame.Tests
{
    [TestFixture]
    public class PlayerEventsTests
    {
        [Test]
        public void PlayerScoreChangedTest()
        {
            var player = new Snake(0, 0, TestHelpers.EmptyMap, "");
            player.ScoreChanged += (sender, score) =>
            {
                Assert.AreEqual(1, score);
                Assert.Pass("Event was triggered.");
            };

            player.Score = 1;

            Assert.Fail("Event was not triggered.");
        }

        [Test]
        public void PlayerSpeedChangedTest()
        {
            var speedToSet = 983;

            var player = new Snake(0, 0, TestHelpers.EmptyMap, "");
            player.SpeedChanged += (sender, speed) =>
            {
                Assert.AreEqual(speed, speedToSet);
                Assert.Pass("Event was triggered.");
            };

            player.Speed = speedToSet;

            Assert.Fail("Event was not triggered.");
        }

        [Test]
        public void PlayerSteppedTest_StepTo()
        {
            var directionToStep = Direction.Down;
            var player = new Snake(0, 0, TestHelpers.EmptyMap, "");
            player.Stepped += (sender, direction) =>
            {
                Assert.AreEqual(directionToStep, direction);
                Assert.Pass("Event was triggered.");
            };

            player.StepTo(directionToStep);

            Assert.Fail("Event was not triggered.");
        }

        [Test]
        public void PlayerSteppedTest_Update()
        {
            var directionToStep = Direction.Down;
            var player = new Snake(0, 0, TestHelpers.EmptyMap, "");
            player.Stepped += (sender, direction) =>
            {
                Assert.AreEqual(directionToStep, direction);
                Assert.Pass("Event was triggered.");
            };

            player.Direction = directionToStep;
            player.Update(TimeSpan.MaxValue);

            Assert.Fail("Event was not triggered.");
        }

        [Test]
        public void PlayerKillTest()
        {
            var map = TestHelpers.EmptyMap;
            var player = new Snake(0, 0, TestHelpers.EmptyMap, "");
            player.Died += sender =>
            {
                Assert.AreEqual(0, map.GetEntities().Count());
                Assert.Pass("Event was triggered.");
            };

            player.Kill();

            Assert.Fail("Event was not triggered.");
        }
    }
}
