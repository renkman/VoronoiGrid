using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;
using VoronoiEngine.Elements;
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

        [Test]
        public void GenerateSites_With20x10Map10Points_SitesFitToGrid()
        {
            const int x = 20;
            const int y = 10;
            const int quantity = 8;

            var generator = new EvenlySpreadSiteGenerator();

            var result = generator.GenerateSites(x, y, quantity);

            Assert.NotNull(result);

            var sites = result.OrderBy(s => Math.Floor(s.Point.Y) * 100 + Math.Floor(s.Point.X)).ToList();

            AssertIsBetween(2, 4, sites[0].Point.X);
            AssertIsBetween(2, 4, sites[0].Point.Y);
            AssertIsBetween(7, 9, sites[1].Point.X);
            AssertIsBetween(2, 4, sites[1].Point.Y);
            AssertIsBetween(12, 14, sites[2].Point.X);
            AssertIsBetween(2, 4, sites[2].Point.Y);
            AssertIsBetween(17, 19, sites[3].Point.X);
            AssertIsBetween(2, 4, sites[3].Point.Y);
            AssertIsBetween(2, 4, sites[4].Point.X);
            AssertIsBetween(7, 9, sites[4].Point.Y);
            AssertIsBetween(7, 9, sites[5].Point.X);
            AssertIsBetween(7, 9, sites[5].Point.Y);
            AssertIsBetween(12, 14, sites[6].Point.X);
            AssertIsBetween(7, 9, sites[6].Point.Y);
            AssertIsBetween(17, 19, sites[7].Point.X);
            AssertIsBetween(7, 9, sites[7].Point.Y);
        }

        private void AssertIsBetween(int min, int max, double value)
        {
            Assert.GreaterOrEqual(value, min);
            Assert.Less(value, max);
        }
    }
}