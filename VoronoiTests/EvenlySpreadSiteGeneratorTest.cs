using NUnit.Framework;
using System.Globalization;
using System.Linq;
using VoronoiEngine.Utilities;

namespace VoronoiTests
{
    [TestFixture]
    public class EvenlySpreadSiteGeneratorTest
    {
        [Test]
        public void TestGenerateSites_10_6_9()
        {
            const int quantity = 9;

            var siteGenerator = new EvenlySpreadSiteGenerator();
            var result = siteGenerator.GenerateSites(36, 27, quantity).ToList();

            Assert.AreEqual(quantity, result.Count);

            Assert.IsTrue(result[0].Point.X >= 0 && result[0].Point.X <= 12, result[0].Point.X.ToString(CultureInfo.InvariantCulture));
            Assert.IsTrue(result[0].Point.Y >= 0 && result[0].Point.Y <= 9, result[0].Point.Y.ToString(CultureInfo.InvariantCulture));

            Assert.IsTrue(result[1].Point.X >= 0 && result[1].Point.X <= 12, result[1].Point.X.ToString(CultureInfo.InvariantCulture));
            Assert.IsTrue(result[1].Point.Y >= 9 && result[1].Point.Y <= 18, result[1].Point.Y.ToString(CultureInfo.InvariantCulture));

            Assert.IsTrue(result[2].Point.X >= 0 && result[2].Point.X <= 12);
            Assert.IsTrue(result[2].Point.Y >= 18 && result[2].Point.Y <= 27);

            Assert.IsTrue(result[3].Point.X >= 12 && result[3].Point.X <= 24);
            Assert.IsTrue(result[3].Point.Y >= 0 && result[0].Point.Y <= 9, result[0].Point.Y.ToString(CultureInfo.InvariantCulture));

            Assert.IsTrue(result[4].Point.X >= 12 && result[4].Point.X <= 24);
            Assert.IsTrue(result[4].Point.Y >= 9 && result[4].Point.Y <= 18);

            Assert.IsTrue(result[5].Point.X >= 12 && result[5].Point.X <= 24);
            Assert.IsTrue(result[5].Point.Y >= 18 && result[5].Point.Y <= 27);

            Assert.IsTrue(result[6].Point.X >= 24 && result[6].Point.X <= 36);
            Assert.IsTrue(result[6].Point.Y >= 0 && result[6].Point.Y <= 9, result[0].Point.Y.ToString(CultureInfo.InvariantCulture));

            Assert.IsTrue(result[7].Point.X >= 24 && result[7].Point.X <= 36);
            Assert.IsTrue(result[7].Point.Y >= 9 && result[7].Point.Y <= 18);

            Assert.IsTrue(result[8].Point.X >= 24 && result[8].Point.X <= 36);
            Assert.IsTrue(result[8].Point.Y >= 18 && result[8].Point.Y <= 27);
        }

        [Test]
        public void TestGenerateSites_8_6_10()
        {
            const int quantity = 10;

            var siteGenerator = new EvenlySpreadSiteGenerator();
            var result = siteGenerator.GenerateSites(100, 100, quantity).ToList();

            Assert.AreEqual(quantity, result.Count);
        }
    }
}