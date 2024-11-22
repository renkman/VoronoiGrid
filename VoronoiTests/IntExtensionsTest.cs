using VoronoiEngine.Utilities;
using Xunit;

namespace VoronoiTests
{
    public class IntExtensionsTest
    {
        [Theory]
        [InlineData(10, 5, 2)]
        [InlineData(16, 4, 4)]
        [InlineData(9, 3, 3)]
        [InlineData(36, 6, 6)]
        [InlineData(32, 8, 4)]
        [InlineData(7, 7, 1)]
        [InlineData(0, 1, 0)]
        [InlineData(8, 4, 2)]
        public void TestGetGreatestDivisors(int number, int divisorA, int divisorB)
        {
            var result = number.GetGreatestDivisors();

            Assert.Equal(divisorA, result.Item1);
            Assert.Equal(divisorB, result.Item2);
        }
    }
}
