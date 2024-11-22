using Xunit;

namespace VoronoiTests
{
    public class BoundingBoxServiceTest
    {
        [Fact]
        public void TestArcSinus()
        {
            var sinA = 5 / 10d;
            var angle = Math.Asin(sinA);
            var degrees = (int)(angle * (180 / Math.PI));

            Assert.Equal(30, degrees);
        }
    }
}
