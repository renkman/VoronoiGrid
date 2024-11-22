using VoronoiEngine.Elements;
using Xunit;

namespace VoronoiTests
{
    /// <summary>
    /// Summary description for PointTest
    /// </summary>
    public class PointTest
    {
        [Fact]
        public void TestCompareLeft()
        {
            var a = new Point { X = 1, Y = 6 };
            var b = new Point { X = 11, Y = 16 };

            var result = a.CompareTo(b);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void TestCompEqual()
        {
            var a = new Point { X = 8, Y = 6 };
            var b = new Point { X = 8, Y = 16 };

            var result = a.CompareTo(b);
            Assert.Equal(0, result);
        }

        [Fact]
        public void TestCompareRight()
        {
            var a = new Point { X = 64, Y = 6 };
            var b = new Point { X = 32, Y = 16 };

            var result = a.CompareTo(b);
            Assert.Equal(1, result);
        }

        [Fact] public void TestGetHashCode()
        {
            var a = new Point { X = 4, Y = 8 };
            var b = new Point { X = 8, Y = 4 };

            Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}