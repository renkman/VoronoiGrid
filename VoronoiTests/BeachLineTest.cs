using VoronoiEngine.Elements;
using VoronoiEngine.Structures;
using Xunit;

namespace VoronoiTests
{
    /// <summary>
    /// Summary description for BeachLineTest
    /// </summary>
    public class BeachLineTest
    {
        [Fact]
        public void TestAddSingleLeaf()
        {
            var beachLine = new BeachLine(2000);
            var site = new Point { X = 464, Y = 500 };
            beachLine.InsertSite(site);

            var leaf = beachLine.Root as Leaf;
            Assert.NotNull(leaf);
            Assert.Equal(site, leaf.Site);
        }

        [Fact]
        public void TestAddTwoLeaves()
        {
            var beachLine = new BeachLine(2000);
            var site1 = new Point { X = 20, Y = 1000 };
            var site2 = new Point { X = 464, Y = 500 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);

            var root = beachLine.Root as Node;
            Assert.NotNull(root);
            Assert.NotNull(root.HalfEdge);
            Assert.Equal(site1, root.HalfEdge.Right);
            Assert.Equal(site2, root.HalfEdge.Left);

            var nodeLeft = root.Left as Node;
            Assert.NotNull(nodeLeft);
            Assert.NotNull(nodeLeft.HalfEdge);
            Assert.Equal(site1, nodeLeft.HalfEdge.Left);
            Assert.Equal(site2, nodeLeft.HalfEdge.Right);

            var leafRight = root.Right as Leaf;
            var leafLeftLeft = nodeLeft.Left as Leaf;
            var leafLeftRight = nodeLeft.Right as Leaf;

            Assert.NotNull(leafRight);
            Assert.NotNull(leafLeftLeft);
            Assert.NotNull(leafLeftRight);

            Assert.Equal(site2, leafLeftRight.Site);
            Assert.Equal(site1, leafLeftLeft.Site);
            Assert.Equal(site1, leafRight.Site);

            Assert.Equal(root.Breakpoint.Left, site2);
            Assert.Equal(root.Breakpoint.Right, site1);
            Assert.Equal(nodeLeft.Breakpoint.Left, site1);
            Assert.Equal(nodeLeft.Breakpoint.Right, site2);
        }

        [Fact]
        public void TestAddThreeLeaves()
        {
            var beachLine = new BeachLine(2000);
            var site1 = new Point { X = 20, Y = 1000 };
            var site2 = new Point { X = 4000, Y = 500 };
            var site3 = new Point { X = 464, Y = 64 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            beachLine.InsertSite(site3);

            var root = beachLine.Root as Node;

            Assert.NotNull(root);

            var nodeLeft = root.Left as Node;
            var nodeLeftLeft = nodeLeft.Left as Node;
            var nodeLeftLeftLeft = nodeLeftLeft.Left as Node;
            Assert.NotNull(nodeLeft);
            Assert.NotNull(nodeLeftLeft);
            Assert.NotNull(nodeLeftLeftLeft);

            var leafRight = root.Right as Leaf;
            var leafLeftRight = nodeLeft.Right as Leaf;
            var leafLeftLeftRight = nodeLeftLeft.Right as Leaf;
            var leafLeftLeftLeftLeft = nodeLeftLeftLeft.Left as Leaf;
            var leafLeftLeftLeftRight = nodeLeftLeftLeft.Right as Leaf;

            Assert.NotNull(leafRight);
            Assert.NotNull(leafLeftRight);
            Assert.NotNull(leafLeftLeftRight);
            Assert.NotNull(leafLeftLeftLeftLeft);
            Assert.NotNull(leafLeftLeftLeftRight);

            Assert.Equal(site1, leafRight.Site);
            Assert.Equal(site2, leafLeftRight.Site);
            Assert.Equal(site1, leafLeftLeftRight.Site);
            Assert.Equal(site1, leafLeftLeftLeftLeft.Site);
            Assert.Equal(site3, leafLeftLeftLeftRight.Site);
        }

        [Fact]
        public void TestGenerateCircleEventZeroLengthArc()
        {
            var beachLine = new BeachLine(2000);
            var site1 = new Point { X = 40, Y = 60 };
            var site2 = new Point { X = 20, Y = 40 };
            var site3 = new Point { X = 60, Y = 40 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            var insert = beachLine.InsertSite(site3);

            var expectedCricleEvent = new Point { X = 40, Y = 20 };

            var result = beachLine.GenerateCircleEvent(insert.Leaves, site3.Y);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedCricleEvent, result.Single().Point);
            //Assert.Equal(site2, result.Single().LeftArc.Site);
            Assert.Equal(site1, result.Single().Arc.Site);
            //Assert.Equal(site3, result.Single().RightArc.Site);
        }

        [Fact]
        public void TestGenerateCircleEvent()
        {
            var beachLine = new BeachLine(0);
            var site1 = new Point { X = 130, Y = 160 };
            var site2 = new Point { X = 110, Y = 150 };
            var site3 = new Point { X = 170, Y = 140 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            var insert = beachLine.InsertSite(site3);

            var expectedCricleEvent = new Point { X = 136, Y = 84 };

            var result = beachLine.GenerateCircleEvent(insert.Leaves, site3.Y);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedCricleEvent.XInt, result.Single().Point.XInt);
            Assert.Equal(expectedCricleEvent.YInt, result.Single().Point.YInt);
            //Assert.Equal(site2, result.Single().LeftArc.Site);
            Assert.Equal(site1, result.Single().Arc.Site);
            //Assert.Equal(site3, result.Single().RightArc.Site);
        }

        [Fact]
        public void TestFindCircleEventAbove()
        {
            var beachLine = new BeachLine(0);
            var site1 = new Point { X = 130, Y = 160 };
            var site2 = new Point { X = 110, Y = 150 };
            var site3 = new Point { X = 170, Y = 140 };
            var site4 = new Point { X = 140, Y = 120 };
            

            beachLine.InsertSite(site1);
            //beachLine.GenerateCircleEvent(site1);
            beachLine.InsertSite(site2);
            //beachLine.GenerateCircleEvent(site2);
            var insert = beachLine.InsertSite(site3);

            var circleEvents = beachLine.GenerateCircleEvent(insert.Leaves, site3.Y);
            var result = beachLine.FindCircleEventAbove(site4);

            Assert.NotNull(result);
            Assert.Equal(circleEvents.Single().Point, result.Point);
        }

        [Fact]
        public void TestRemoveLeaf()
        {
            var beachLine = new BeachLine(0);
            var site1 = new Point { X = 40, Y = 60 };
            var site2 = new Point { X = 20, Y = 40 };
            var site3 = new Point { X = 60, Y = 40 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            var insert = beachLine.InsertSite(site3);

            var circleEvents = beachLine.GenerateCircleEvent(insert.Leaves, site3.Y);
            var circleEvent = circleEvents.Single();

            beachLine.RemoveLeaf(circleEvent.Arc);

            var rightSubtree = ((Node)beachLine.Root).Right as Node;
            var leafLeft = rightSubtree.Left as Leaf;
            var leafRight = rightSubtree.Right as Leaf;

            Assert.NotNull(leafLeft);
            Assert.NotNull(leafRight);

            Assert.Equal(rightSubtree.Breakpoint.Left, site3);
            Assert.Equal(rightSubtree.Breakpoint.Right, site1);
            Assert.Equal(leafLeft.Site, site3);
            Assert.Equal(leafRight.Site, site1);
        }

        //[Fact]
        //public void TestRemoveLastLeaf()
        //{
        //    var beachLine = new BeachLine();
        //    var site1 = new Point { X = 40, Y = 60 };
        //    var site2 = new Point { X = 20, Y = 40 };

        //    beachLine.InsertSite(site1);
        //    beachLine.InsertSite(site2);
        //    var root = beachLine.Root as Node;
        //    var right = root.Left as Node;
        //    beachLine.RemoveLeaf((Leaf)right.Left);

        //    var rightLeaf = root.Right as Leaf;
        //    beachLine.RemoveLeaf(rightLeaf);

        //    Assert.IsNull(root.Right);
        //    var left = root.Left as Leaf;
        //    Assert.NotNull(left);
        //}
    }
}