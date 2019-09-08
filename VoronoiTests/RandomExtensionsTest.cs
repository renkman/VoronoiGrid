using NUnit.Framework;
using System;
using VoronoiEngine.Utilities;

namespace VoronoiTests
{
    [TestFixture]
    public class RandomExtensionsTest
    {
        [Test]
        public void TestRange([Range(0, 10)] int min, [Range(1, 10)] int difference)
        {
            var max = min + difference;
            var random = new Random();
            var result = random.Range(min, max);

            Assert.IsTrue(result >= min, $"{result} < {min}");
            Assert.IsTrue(result <= max, $"{result} > {max}");
        }

        [Test]
        public void TestRangeException()
        {
            var random = new Random();
            Assert.Throws<ArgumentException>(() => random.Range(3, 2));
            Assert.Throws<ArgumentException>(() => random.Range(3, 3));
        }
    }
}