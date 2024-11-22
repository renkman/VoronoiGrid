using VoronoiEngine.Utilities;
using Xunit;

namespace VoronoiTests
{
    public class RandomExtensionsTest
    {
        [Fact]
        // public void TestRange([Range(0, 10)] int min, [Range(1, 10)] int difference)
        public void TestRange()
        {
            const int min = 1;
            const int difference = 2;
            var max = min + difference;
            var random = new Random();
            var result = random.Range(min, max);

            Assert.True(result >= min, $"{result} < {min}");
            Assert.True(result <= max, $"{result} > {max}");
        }

        [Fact]
        public void TestRangeException()
        {
            var random = new Random();
            Assert.Throws<ArgumentException>(() => random.Range(3, 2));
            Assert.Throws<ArgumentException>(() => random.Range(3, 3));
        }
    }
}