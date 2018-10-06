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

            var expectedVertex = new Point { X = 136.25, Y = 122.5 };
            var expectedCricleEvent = new Point { X = 136.25, Y = 84 };

            var service = new CircleEventCalculationService();
            var result = service.DetermineCircleEvent(arcs, 140);

            Assert.AreEqual(expectedCricleEvent, result.Point);
            Assert.AreEqual(expectedVertex, result.Vertex);
        }

        [Test]
        public void TestEdgeDetermineCircleEvent()
        {
            var parentLeft = new Node(null);
            var parentRight= new Node(null);

            var site1 = new Point { X = 130, Y = 160 };
            var site2 = new Point { X = 110, Y = 150 };
            var site3 = new Point { X = 170, Y = 140 };

            var node1 = new Leaf(new Point { X = 170, Y = 140 });
            var node2 = new Leaf(new Point { X = 130, Y = 160 });
            var node3 = new Leaf(new Point { X = 110, Y = 150 });
            node1.Parent = parentLeft;
            node2.Parent = parentLeft;
            node3.Parent = parentRight;
            var arcs = new INode[] { node1, node2, node3 };

            var expectedVertex = new Point { X = 136.25, Y = 122.5 };
            var expectedCricleEvent = new Point { X = 136.25, Y = 84 };

            var service = new CircleEventEdgeCalculationService();
            var result = service.DetermineCircleEvent(arcs, 140);

            Assert.AreEqual(expectedCricleEvent, result.Point);
            Assert.AreEqual(expectedVertex, result.Vertex);
        }

        //Found circle event for arcs: Leaf: Point: X: 11, Y: 19, CircleEvent: Point: X: -2147483648, Y: -766958430, Leaf: Point: X: 2, Y: 5, CircleEvent: , Leaf: Point: X: 11, Y: 19, CircleEvent:  at Point: Point: X: -2147483648, Y: -766958430 and Vertex: Point: X: -2147483648, Y: 1380525218
        [Test]
        public void TestDetermineCircleNonUnique()
        {
            var node1 = new Leaf(new Point { X = 11, Y = 19 });
            var node2 = new Leaf(new Point { X = 2, Y = 5 });
            var node3 = new Leaf(new Point { X = 11, Y = 19 });
            var arcs = new INode[] { node1, node2, node3 };

            //var expectedVertex = new Point { X = -2147483648, Y = 1380525218 };
            //var expectedCricleEvent = new Point { X = -2147483648, Y = -766958430 };

            var service = new CircleEventCalculationService();
            var result = service.DetermineCircleEvent(arcs, 5);

            Assert.IsNull(result);
        }
    }
}
