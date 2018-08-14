using NUnit.Framework;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Structures;

namespace VoronoiTests
{
    /// <summary>
    /// Summary description for BeachLineTest
    /// </summary>
    [TestFixture]
    public class BeachLineTest
    {
        [Test]
        public void TestAddSingleLeaf()
        {
            var beachLine = new BeachLine();
            var site = new Point { X = 464, Y = 500 };
            beachLine.InsertSite(site);

            var leaf = beachLine.Root as Leaf;
            Assert.IsNotNull(leaf);
            Assert.AreEqual(site, leaf.Site);
        }

        [Test]
        public void TestAddTwoLeaves()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 20, Y = 1000 };
            var site2 = new Point { X = 464, Y = 500 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);

            var root = beachLine.Root as Node;
            Assert.IsNotNull(root);
            Assert.IsNotNull(root.HalfEdge);
            Assert.AreEqual(site1, root.HalfEdge.Left);
            Assert.AreEqual(site2, root.HalfEdge.Right);

            var nodeLeft = root.Left as Node;
            Assert.IsNotNull(nodeLeft);
            Assert.IsNotNull(nodeLeft.HalfEdge);
            Assert.AreEqual(site1, nodeLeft.HalfEdge.Left);
            Assert.AreEqual(site2, nodeLeft.HalfEdge.Right);

            var leafRight = root.Right as Leaf;
            var leafLeftLeft = nodeLeft.Left as Leaf;
            var leafLeftRight = nodeLeft.Right as Leaf;

            Assert.IsNotNull(leafRight);
            Assert.IsNotNull(leafLeftLeft);
            Assert.IsNotNull(leafLeftRight);

            Assert.AreEqual(site2, leafLeftRight.Site);
            Assert.AreEqual(site1, leafLeftLeft.Site);
            Assert.AreEqual(site1, leafRight.Site);

            Assert.AreEqual(root.Breakpoint.Left, site2);
            Assert.AreEqual(root.Breakpoint.Right, site1);
            Assert.AreEqual(nodeLeft.Breakpoint.Left, site1);
            Assert.AreEqual(nodeLeft.Breakpoint.Right, site2);
        }

        [Test]
        public void TestAddThreeLeaves()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 20, Y = 1000 };
            var site2 = new Point { X = 4000, Y = 500 };
            var site3 = new Point { X = 464, Y = 64 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            beachLine.InsertSite(site3);

            var root = beachLine.Root as Node;

            Assert.IsNotNull(root);

            var nodeLeft = root.Left as Node;
            var nodeLeftLeft = nodeLeft.Left as Node;
            var nodeLeftLeftLeft = nodeLeftLeft.Left as Node;
            Assert.IsNotNull(nodeLeft);
            Assert.IsNotNull(nodeLeftLeft);
            Assert.IsNotNull(nodeLeftLeftLeft);

            var leafRight = root.Right as Leaf;
            var leafLeftRight = nodeLeft.Right as Leaf;
            var leafLeftLeftRight = nodeLeftLeft.Right as Leaf;
            var leafLeftLeftLeftLeft = nodeLeftLeftLeft.Left as Leaf;
            var leafLeftLeftLeftRight = nodeLeftLeftLeft.Right as Leaf;

            Assert.IsNotNull(leafRight);
            Assert.IsNotNull(leafLeftRight);
            Assert.IsNotNull(leafLeftLeftRight);
            Assert.IsNotNull(leafLeftLeftLeftLeft);
            Assert.IsNotNull(leafLeftLeftLeftRight);

            Assert.AreEqual(site1, leafRight.Site);
            Assert.AreEqual(site2, leafLeftRight.Site);
            Assert.AreEqual(site1, leafLeftLeftRight.Site);
            Assert.AreEqual(site1, leafLeftLeftLeftLeft.Site);
            Assert.AreEqual(site3, leafLeftLeftLeftRight.Site);
        }

        [Test]
        public void TestGenerateCircleEventZeroLengthArc()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 40, Y = 60 };
            var site2 = new Point { X = 20, Y = 40 };
            var site3 = new Point { X = 60, Y = 40 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            beachLine.InsertSite(site3);

            var expectedCricleEvent = new Point { X = 40, Y = 20 };
            var expectedVertex = new Point { X = 40, Y = 40 };

            var result = beachLine.GenerateCircleEvent(site3);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedCricleEvent, result.Single().Point);
            Assert.AreEqual(expectedVertex, result.Single().Vertex);
            Assert.AreEqual(site3, result.Single().LeftArc.Site);
            Assert.AreEqual(site1, result.Single().CenterArc.Site);
            Assert.AreEqual(site2, result.Single().RightArc.Site);
        }
        
        [Test]
        public void TestGenerateCircleEvent()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 130, Y = 160 };
            var site2 = new Point { X = 110, Y = 150 };
            var site3 = new Point { X = 170, Y = 140 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            beachLine.InsertSite(site3);

            var expectedCricleEvent = new Point { X = 136, Y = 83 };
            var expectedVertex = new Point { X = 136, Y = 121 };

            var result = beachLine.GenerateCircleEvent(site3);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedCricleEvent, result.Single().Point);
            Assert.AreEqual(expectedVertex, result.Single().Vertex);
            Assert.AreEqual(site3, result.Single().LeftArc.Site);
            Assert.AreEqual(site1, result.Single().CenterArc.Site);
            Assert.AreEqual(site2, result.Single().RightArc.Site);
        }

        [Test]
        [Ignore("False alarm test should be clarified!")]
        public void TestFindCircleEventAbove()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 130, Y = 160 };
            var site2 = new Point { X = 110, Y = 150 };
            var site3 = new Point { X = 170, Y = 140 };
            var site4 = new Point { X = 130, Y = 120 };

            beachLine.InsertSite(site1);
            var circleEvents1 = beachLine.GenerateCircleEvent(site1);
            beachLine.InsertSite(site2);
            var circleEvents2 = beachLine.GenerateCircleEvent(site2);
            beachLine.InsertSite(site3);
            var circleEvents3 = beachLine.GenerateCircleEvent(site3);

            var result = beachLine.FindCircleEventAbove(site4);

            Assert.IsNotNull(result);
            //Assert.AreEqual(circleEvents.First().Point, result.Point);
            //Assert.AreEqual(circleEvents.First().Vertex, result.Vertex);
        }

        [Test]
        public void TestRemoveLeaf()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 40, Y = 60 };
            var site2 = new Point { X = 20, Y = 40 };
            var site3 = new Point { X = 60, Y = 40 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            beachLine.InsertSite(site3);

            var circleEvents = beachLine.GenerateCircleEvent(site3);
            var circleEvent = circleEvents.Single();

            beachLine.RemoveLeaf(circleEvent.CenterArc);

            var rightSubtree = ((Node)beachLine.Root).Right as Node;
            var leafLeft = rightSubtree.Left as Leaf;
            var leafRight = rightSubtree.Right as Leaf;

            Assert.IsNotNull(leafLeft);
            Assert.IsNotNull(leafRight);

            Assert.AreEqual(rightSubtree.Breakpoint.Left, site3);
            Assert.AreEqual(rightSubtree.Breakpoint.Right, site1);
            Assert.AreEqual(leafLeft.Site, site3);
            Assert.AreEqual(leafRight.Site, site1);
        }
        
        //[Test]
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
        //    Assert.IsNotNull(left);
        //}
    }
}