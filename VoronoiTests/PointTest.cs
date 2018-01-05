using NUnit.Framework;
using VoronoiEngine.Elements;

namespace VoronoiTests
{
    /// <summary>
    /// Summary description for PointTest
    /// </summary>
    [TestFixture]
    public class PointTest
    {
        [Test]
        public void TestCompareLeft()
        {
            var a = new Point { X = 1, Y = 6 };
            var b = new Point { X = 11, Y = 16 };

            var result = a.CompareTo(b);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void TestCompareEqual()
        {
            var a = new Point { X = 8, Y = 6 };
            var b = new Point { X = 8, Y = 16 };

            var result = a.CompareTo(b);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void TestCompareRight()
        {
            var a = new Point { X = 64, Y = 6 };
            var b = new Point { X = 32, Y = 16 };

            var result = a.CompareTo(b);
            Assert.AreEqual(1, result);
        }

        [Test] public void TestGetHashCode()
        {
            var a = new Point { X = 4, Y = 8 };
            var b = new Point { X = 8, Y = 4 };

            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}