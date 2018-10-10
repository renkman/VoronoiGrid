using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoronoiEngine.Elements;
using VoronoiEngine.Geomerty;

namespace VoronoiTests
{
    [TestFixture]
    public class CircleEventCalculationServiceTest
    {
        [Test]
        [Ignore("To be revised")]
        public void TestDetermineCircleEvent()
        {
            var parentDummy = new Node(null);

            var site1 = new Point { X = 130, Y = 160 };
            var site2 = new Point { X = 110, Y = 150 };
            var site3 = new Point { X = 170, Y = 140 };

            var node1 = new Leaf(new Point { X = 170, Y = 140 });
            var node2 = new Leaf(new Point { X = 130, Y = 160 }); 
            var node3 = new Leaf(new Point { X = 110, Y = 150 }); 
            node1.Parent = parentDummy;
            node2.Parent = parentDummy;
            node3.Parent = parentDummy;
            var arcs = new INode[] { node1, node2, node3 };

            var expectedCricleEvent = new Point { X = 136.25, Y = 84 };

            var service = new CircleEventCalculationService();
            var result = service.DetermineCircleEvent(node3, 140);

            Assert.AreEqual(expectedCricleEvent, result.Point);
       }
    }
}
