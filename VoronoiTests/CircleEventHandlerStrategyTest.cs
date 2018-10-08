using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.EventHandler;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;

namespace VoronoiTests
{
    [TestFixture]
    public class CircleEventHandlerStrategyTest
    {
        [Test]
        public void TestHandleEvent()
        {
            var beachLine = new BeachLine();
            beachLine.InsertSite(new Point(4, 6));
            var halfEdgesLeft = beachLine.InsertSite(new Point(6, 4)).HalfEdges.ToList();
            var halfEdgesRight = beachLine.InsertSite(new Point(2, 3)).HalfEdges.ToList();

            var centerLeaf = ((Node)((Node)((Node)beachLine.Root).Left).Left).Right;

            var sweepEvent = new CircleEvent
            {
                LeftArc = new Leaf(new Point(2, 3)),
                CenterArc = (Leaf)centerLeaf,
                RightArc = new Leaf(new Point(6, 4)),
                Edges = new List<HalfEdge> {
                    halfEdgesLeft[1],
                    halfEdgesRight[1]
                },
                Point = new Point(4, 2),
                Vertex = new Point(4, 4)
            };
            var eventQueue = new EventQueue();

            var strategy = new CircleEventHandlerStrategy();
            var result = strategy.HandleEvent(sweepEvent, eventQueue, beachLine).ToList();

            Assert.IsNotNull(result);
            var vertex = ((Vertex)result.Single(g => g is Vertex));
            Assert.AreEqual(sweepEvent.Vertex, vertex.Point);
            Assert.AreEqual(3, vertex.HalfEdges.Count);

            var start = vertex.HalfEdges.Single(h => h.Start != null);
            Assert.IsNull(start.End);
            Assert.AreEqual(new Point(2, 3), start.Left);
            Assert.AreEqual(new Point(6, 4), start.Right);

            var ends = vertex.HalfEdges.Where(h => h.End != null);
            Assert.AreEqual(2, ends.Count());
            Assert.IsTrue(!ends.Contains(start));
            Assert.IsTrue(ends.All(h => h.Start == null));
        }
    }
}