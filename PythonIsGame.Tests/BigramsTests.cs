using NUnit.Framework;
using PythonIsGame.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Tests
{
    [TestFixture]
    public class BigramsTests
    {
        [Test]
        public void BigramsOfSingleItemList()
        {
            var actual = new[] { 42.0 }.GetBigrams();
            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void BigramsOfTwoItemList()
        {
            var actual = new[] { 1, 2 }.GetBigrams();
            Assert.That(actual, Is.EqualTo(new[] { Tuple.Create(1, 2) }));
        }

        [Test]
        public void BigramsOfLongList()
        {
            var count = 1000;
            var actual = Enumerable.Range(100, count).GetBigrams().ToList();
            Assert.That(actual, Has.Count.EqualTo(count - 1));
            for (int index = 0; index < count - 1; index++)
            {
                var tuple = actual[index];
                Assert.That(tuple.Item1, Is.EqualTo(100 + index));
                Assert.That(tuple.Item2, Is.EqualTo(101 + index));
            }
        }

        [Test]
        public void BigramsOfReferenceTypeSequence()
        {
            var actual = new[] { "1", null, "2" }.GetBigrams();
            Assert.That(actual,
                Is.EqualTo(
                    new[]
                    {
                        Tuple.Create("1", (string)null),
                        Tuple.Create((string)null, "2"),
                    }));

        }
    }
}
