using NUnit.Framework;
using System.Linq;
using VoronoiEngine.Utilities;

namespace VoronoiTests
{
    [TestFixture]
    public class SiteGeneratorTest
    {
        [Test]
        public void TestGenerate()
        {
            var generator = new RandomSiteGenerator();
            var result = generator.GenerateSites(200, 200, 20);

            Assert.AreEqual(20, result.Count);
            Assert.IsTrue(result.Select(s => s.Point.X).Min() >= 0);
            Assert.IsTrue(result.Select(s => s.Point.Y).Min() >= 0);
            Assert.IsTrue(result.Select(s => s.Point.X).Max() <= 200);
            Assert.IsTrue(result.Select(s => s.Point.Y).Max() <= 200);
            Assert.AreEqual(20, result.GroupBy(s => s.Point).Count());
        }
    }
}