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
        public void TestGetNeighborClockwise()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 40, Y = 60 };
            var site2 = new Point { X = 20, Y = 40 };
            var site3 = new Point { X = 60, Y = 40 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            beachLine.InsertSite(site3);

            var rootNode = beachLine.Root as Node;
            var leaf = ((Node)((Node)rootNode.Right).Left).Left as Leaf;
            var expected = ((Node)rootNode.Left).Right as Leaf;

            var result = beachLine.Root.GetNeighbor(leaf, TraverseDirection.Clockwise);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestGetNeighborCounterClockwise()
        {
            var beachLine = new BeachLine();
            var site1 = new Point { X = 40, Y = 60 };
            var site2 = new Point { X = 20, Y = 40 };
            var site3 = new Point { X = 60, Y = 40 };

            beachLine.InsertSite(site1);
            beachLine.InsertSite(site2);
            beachLine.InsertSite(site3);

            var rootNode = beachLine.Root as Node;
            var leaf = ((Node)((Node)rootNode.Right).Left).Left as Leaf;
            var expected = ((Node)((Node)rootNode.Right).Left).Right as Leaf;

            var result = beachLine.Root.GetNeighbor(leaf, TraverseDirection.CounterClockwise);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }
    }
}