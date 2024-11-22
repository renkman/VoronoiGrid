using VoronoiEngine.Utilities;
using Xunit;

namespace VoronoiTests
{
    public class SiteGeneratorTest
    {
        [Fact]
        public void TestGenerate()
        {
            var generator = new RandomSiteGenerator();
            var result = generator.GenerateSites(200, 200, 20);

            Assert.Equal(20, result.Count);
            Assert.True(result.Select(s => s.Point.X).Min() >= 0);
            Assert.True(result.Select(s => s.Point.Y).Min() >= 0);
            Assert.True(result.Select(s => s.Point.X).Max() <= 200);
            Assert.True(result.Select(s => s.Point.Y).Max() <= 200);
            Assert.Equal(20, result.GroupBy(s => s.Point).Count());
        }
    }
}