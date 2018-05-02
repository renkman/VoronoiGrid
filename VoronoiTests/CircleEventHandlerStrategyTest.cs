using NUnit.Framework;
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
            beachLine.InsertSite(new Point { X = 130, Y = 160 });
            beachLine.InsertSite(new Point { X = 110, Y = 150 });
            beachLine.InsertSite(new Point { X = 170, Y = 140 });

            var centerLeaf = beachLine.Root.Find(new Point { X = 130, Y = 160 });

            var sweepEvent = new CircleEvent
            {
                LeftArc = new Leaf(new Point { X = 170, Y = 140 }),
                CenterArc = (Leaf)centerLeaf,
                RightArc = new Leaf(new Point { X = 110, Y = 150 }),
                Parents = null,
                Point = new Point { X = 136, Y = 83 },
                Vertex = new Point { X = 136, Y = 121 }
            };
            var eventQueue = new EventQueue();

            var strategy = new CircleEventHandlerStrategy();
            var result = strategy.HandleEvent(sweepEvent, eventQueue, beachLine);

            Assert.IsNotNull(result);
            Assert.AreEqual(sweepEvent.Vertex, result.Point);
            Assert.AreEqual(3, result.HalfEdges.Count);

            var start = result.HalfEdges.Single(h => h.Start != null);
            Assert.IsNull(start.End);
            Assert.AreEqual(new Point { X = 170, Y = 140 }, start.Left);
            Assert.AreEqual(new Point { X = 110, Y = 150 }, start.Right);
            Assert.AreEqual(new Point { X = 61, Y = 139 }, start.Point);

            var ends = result.HalfEdges.Where(h => h.End != null);
            Assert.AreEqual(2, ends.Count());
            Assert.IsTrue(!ends.Contains(start));
            Assert.IsTrue(ends.All(h => h.Start == null));

            var left = ends.Single(h => h.Point.Equals(new Point { X = 74, Y = 139 }));
            Assert.AreEqual(new Point { X = 170, Y = 140 }, left.Left);
            Assert.AreEqual(new Point { X = 130, Y = 160 }, left.Right);
            Assert.AreEqual(new Point { X = 74, Y = 139 }, left.Point);

            var right = ends.Single(h => h.Point.Equals(new Point { X = 49, Y = 149 }));
            Assert.AreEqual(new Point { X = 130, Y = 160 }, right.Left);
            Assert.AreEqual(new Point { X = 110, Y = 150 }, right.Right);
            Assert.AreEqual(new Point { X = 49, Y = 149 }, right.Point);
        }
    }
}