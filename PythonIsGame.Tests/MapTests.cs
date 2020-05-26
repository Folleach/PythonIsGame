using NUnit.Framework;
using PythonIsGame.Common;
using PythonIsGame.Common.Entities;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Tests
{
    [TestFixture]
    public class MapTests
    {
        [Test]
        public void MaterialSet_ShouldBe4()
        {
            var count = 4;
            MaterialCount(new Tuple<IMaterial, Point>[]
            {
                Tuple.Create((IMaterial)new AppleMaterial(), new Point(1, 1)),
                Tuple.Create((IMaterial)new TeleportMaterial(Point.Empty), new Point(2, 2)),
                Tuple.Create((IMaterial)new WallMaterial(), new Point(3, 3)),
                Tuple.Create((IMaterial)new AppleMaterial(), new Point(4, 4)),
            }, count);
        }

        [Test]
        public void MaterialSet_ShouldBe2()
        {
            var count = 2;
            MaterialCount(new Tuple<IMaterial, Point>[]
            {
                Tuple.Create((IMaterial)new AppleMaterial(), new Point(1, 1)),
                Tuple.Create((IMaterial)new TeleportMaterial(Point.Empty), new Point(2, 2)),
                Tuple.Create((IMaterial)new WallMaterial(), new Point(1, 1)),
                Tuple.Create((IMaterial)new AppleMaterial(), new Point(2, 2)),
            }, count);
        }

        [Test]
        public void MaterialSet_ShouldNotChanged()
        {
            MaterialSet(new AppleMaterial(), new Point(128, 128));
        }

        [Test]
        public void EntityIntersection()
        {
            var map = TestHelpers.EmptyMap;
            var point = new Point(13, 7);
            var entityInitiator = new Entity();
            entityInitiator.Position = point;

            map.RegisterIntersectionWithEntity(entityInitiator, typeof(SnakeBody), e => Assert.AreEqual(point, e.Position));
            map.AddEntity(entityInitiator, false);
            map.AddEntity(new SnakeBody(13, 7), false);
            map.AddEntity(new SnakeBody(0, 0), false);
            map.Update();
        }

        [Test]
        public void RemoveEntityTest()
        {
            var map = TestHelpers.EmptyMap;
            var head = new SnakeHead(0, 0);
            var body = new SnakeBody(0, 0);
            map.AddEntity(head, false);
            map.AddEntity(body, false);
            map.RegisterIntersectionWithEntity(head, typeof(SnakeBody), e => Assert.Fail("Entity was not completely removed from the map"));
            map.RemoveEntity(body);
            map.Update();
            Assert.AreEqual(1, map.GetEntities().Count());
        }

        [Test]
        public void TestOnIntersectionAxis()
        {
            var mapGenerator = new AxisMapGenerator();

            int FromX = -500, FromY = -500;
            int ToX = 500, ToY = 500;

            var random = new Random(82881285);

            for (var x = FromX; x < ToX; x++)
            {
                for (var y = FromY; y < ToY; y++)
                {
                    if (random.Next(2) == 1)
                        mapGenerator.PointsOfWalls.Add(new Point(x, y));
                }
            }

            var map = new ChunkedMap(mapGenerator);
            var player = new Snake(0, 0, map, "player", true);
            var p = new Point(-500, -384);
            void Intersect(PositionMaterial material)
            {
                if (player.Head.Position != material.Position)
                    Assert.Fail($"Diffirent position ({player.Head.Position} and {material.Position})");
                if (!mapGenerator.PointsOfWalls.Contains(player.Head.Position))
                    Assert.Fail($"Phantom material on {player.Head.Position}");
            }

            map.RegisterIntersectionWithMaterial(player.Head, typeof(WallMaterial), Intersect);

            for (var x = FromX; x < ToX; x++)
            {
                for (var y = FromY; y < ToY; y++)
                {
                    player.Head.Position = new Point(x, y);
                    map.Update();
                }
            }
        }

        public void MaterialSet(IMaterial material, Point point)
        {
            var map = TestHelpers.EmptyMap;
            map.SetMaterial(material, point);
            Assert.AreEqual(material, map.GetMaterial(point).Material);
            Assert.AreEqual(point, map.GetMaterial(point).Position);
            Assert.AreEqual(1, map.GetMaterials().Count());
        }

        public void MaterialCount(Tuple<IMaterial, Point>[] materials, int expectedCount)
        {
            var map = TestHelpers.EmptyMap;
            foreach (var material in materials)
                map.SetMaterial(material.Item1, material.Item2);
            Assert.AreEqual(expectedCount, map.GetMaterials().Count());
        }

        private class AxisMapGenerator : IMapGenerator
        {
            public int ChunkSize => 64;

            public HashSet<Point> PointsOfWalls = new HashSet<Point>();

            public Chunk Generate(Point chunkPosition)
            {
                var chunk = new Chunk(chunkPosition.X, chunkPosition.Y, ChunkSize);
                for (var x = 0; x < ChunkSize; x++)
                {
                    for (var y = 0; y < ChunkSize; y++)
                    {
                        var absolute = chunk.GetAbsolutePoint(x, y);
                        if (PointsOfWalls.Contains(absolute))
                            chunk.SetMaterial(new Point(x, y), new WallMaterial());
                    }
                }
                return chunk;
            }
        }
    }
}
