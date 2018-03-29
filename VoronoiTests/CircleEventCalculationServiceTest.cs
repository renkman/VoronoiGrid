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
        public void TestDetermineCircleEvent()
        {
            var node1 = new Leaf(new Point { X = 130, Y = 160 });
            var node2 = new Leaf(new Point { X = 110, Y = 150 });
            var node3 = new Leaf(new Point { X = 170, Y = 140 });
            var arcs = new INode[] { node1, node2, node3 };

            var expectedVertex = new Point { X = 136, Y = 121 };
            var expectedCricleEvent = new Point { X = 136, Y = 83 };

            var service = new CircleEventCalculationService();
            var result = service.DetermineCircleEvent(arcs);

            Assert.AreEqual(expectedCricleEvent, result.Point);
            Assert.AreEqual(expectedVertex, result.Vertex);
        }
    }
}
