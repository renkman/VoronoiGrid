using NUnit.Framework;
using VoronoiEngine.Elements;

namespace VoronoiTests
{
    [TestFixture]
    public class NodeTest
    {
        [Test]
        public void TestCalculateBreakpoint()
        {
            var node = new Node
            {
                Breakpoint = new Tuple
                {
                    Left = new Point { X = 2, Y = 10 },
                    Right = new Point { X = 20, Y = 12 }
                }
            };

            var result = node.CalculateBreakpoint(40);
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.X);
            Assert.AreEqual(40, result.Y);
        }

        [Test]
        public void TestCalculateBreakpointSameY()
        {
            var node = new Node
            {
                Breakpoint = new Tuple
                {
                    Left = new Point { X = 15, Y = 120 },
                    Right = new Point { X = 20, Y = 120 }
                }
            };

            var result = node.CalculateBreakpoint(40);
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.X);
            Assert.AreEqual(40, result.Y);
        }

        [Test]
        public void TestCalculateBreakpointLeftOnSweepline()
        {
            var node = new Node
            {
                Breakpoint = new Tuple
                {
                    Left = new Point { X = 15, Y = 120 },
                    Right = new Point { X = 20, Y = 12 }
                }
            };

            var result = node.CalculateBreakpoint(120);
            Assert.IsNotNull(result);
            Assert.AreEqual(15, result.X);
            Assert.AreEqual(120, result.Y);
        }

        [Test]
        public void TestCalculateBreakpointRightOnSweepline()
        {
            var node = new Node
            {
                Breakpoint = new Tuple
                {
                    Left = new Point { X = 15, Y = 120 },
                    Right = new Point { X = 20, Y = 40 }
                }
            };

            var result = node.CalculateBreakpoint(40);
            Assert.IsNotNull(result);
            Assert.AreEqual(20, result.X);
            Assert.AreEqual(40, result.Y);
        }

        [Test]
        public void TestCalculateBreakpointTemp()
        {
            var node = new Node
            {
                Breakpoint = new Tuple
                {
                    Left = new Point { X = 20, Y = 1000 },
                    Right = new Point { X = 4000, Y = 500 }
                }
            };

            var resultA = node.CalculateBreakpoint(64);
            Assert.IsNotNull(resultA);

            node.Breakpoint.Left = new Point { X = 4000, Y = 500 }; 
            node.Breakpoint.Right = new Point { X = 20, Y = 1000 };

            var resultB = node.CalculateBreakpoint(64);
            Assert.IsNotNull(resultB);
        }
    }
}