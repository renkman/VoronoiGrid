using NUnit.Framework;
using System.Collections.Generic;
using VoronoiEngine.Elements;
using VoronoiEngine.Structures;

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
        public void TestGetDescendantsClockwise()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 40, Y = 60 };
            var site2 = new Point { X = 20, Y = 40 };
            var site3 = new Point { X = 60, Y = 40 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            beachLine.InsertSite(site3);

            var result = new List<INode>();
            beachLine.Root.GetDescendants(site3, TraverseDirection.Clockwise, result, 3);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(site3, ((Leaf)result[0]).Site);
            Assert.AreEqual(site1, ((Leaf)result[1]).Site);
            Assert.AreEqual(site2, ((Leaf)result[2]).Site);
        }


        [Test]
        public void TestGetDescendantsCounterClockwise()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 40, Y = 60 };
            var site2 = new Point { X = 20, Y = 40 };
            var site3 = new Point { X = 60, Y = 40 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            beachLine.InsertSite(site3);

            var result = new List<INode>();
            beachLine.Root.GetDescendants(site3, TraverseDirection.CounterClockwise, result, 3);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(site3, ((Leaf)result[0]).Site);
            Assert.AreEqual(site1, ((Leaf)result[1]).Site);        
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