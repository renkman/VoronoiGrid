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
            var generator = new SiteGenerator();
            var result = generator.GenerateSites(20, 20, 6);

            Assert.AreEqual(6, result.Count);
            Assert.IsTrue(result.Select(s => s.Point.X).Min() >= 0);
            Assert.IsTrue(result.Select(s => s.Point.Y).Min() >= 0);
            Assert.IsTrue(result.Select(s => s.Point.X).Max() <= 20);
            Assert.IsTrue(result.Select(s => s.Point.Y).Max() <= 20);
        }
    }
}