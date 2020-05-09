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
            var map = CreateEmptyMap();
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
            var map = CreateEmptyMap();
            var head = new SnakeHead(0, 0);
            var body = new SnakeBody(0, 0);
            map.AddEntity(head, false);
            map.AddEntity(body, false);
            map.RegisterIntersectionWithEntity(head, typeof(SnakeBody), e => Assert.Fail("Entity was not completely removed from the map"));
            map.RemoveEntity(body);
            map.Update();
            Assert.AreEqual(1, map.GetEntities().Count());
        }

        public void MaterialSet(IMaterial material, Point point)
        {
            var map = CreateEmptyMap();
            map.SetMaterial(material, point);
            Assert.AreEqual(material, map.GetMaterial(point).Material);
            Assert.AreEqual(point, map.GetMaterial(point).Position);
            Assert.AreEqual(1, map.GetMaterials().Count());
        }

        public void MaterialCount(Tuple<IMaterial, Point>[] materials, int expectedCount)
        {
            var map = CreateEmptyMap();
            foreach (var material in materials)
                map.SetMaterial(material.Item1, material.Item2);
            Assert.AreEqual(expectedCount, map.GetMaterials().Count());
        }

        private IMap CreateEmptyMap()
        {
            return new ChunkedMap(new EmptyMapGenerator(16));
        }
    }
}
