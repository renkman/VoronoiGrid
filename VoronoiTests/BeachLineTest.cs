using NUnit.Framework;
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

            var nodeLeft = root.Left as Node;
            Assert.IsNotNull(nodeLeft);

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
    }
}