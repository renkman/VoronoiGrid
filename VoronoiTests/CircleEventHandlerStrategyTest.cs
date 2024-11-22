using VoronoiEngine.Elements;
using VoronoiEngine.Structures;
using Xunit;

namespace VoronoiTests
{
    public class CircleEventHandlerStrategyTest
    {
        [Fact(Skip = "To be revised")]
        public void TestHandleEvent()
        {
            var beachLine = new BeachLine(10);
            var center = beachLine.InsertSite(new Point(4, 6));
            var right = beachLine.InsertSite(new Point(6, 4));
            var left = beachLine.InsertSite(new Point(2, 3));

            //var centerLeaf = ((Node)((Node)((Node)beachLine.Root).Left).Left).Right;

            //var sweepEvent = new CircleEvent
            //{
            //    LeftArc = new Leaf(new Point(2, 3)),
            //    CenterArc = left.Leaves[0],
            //    RightArc = new Leaf(new Point(6, 4)),
            //    Edges = new List<HalfEdge> {
            //        halfEdgesLeft[1],
            //        halfEdgesRight[1]
            //    },
            //    Point = new Point(4, 2),
            //    Vertex = new Point(4, 4)
            //};
            //var eventQueue = new EventQueue();

            //var strategy = new CircleEventHandlerStrategy();
            //var result = strategy.HandleEvent(sweepEvent, eventQueue, beachLine).ToList();

            //Assert.NotNull(result);
            //var vertex = ((Vertex)result.Single(g => g is Vertex));
            //Assert.Equal(sweepEvent.Vertex, vertex.Point);
            //Assert.Equal(3, vertex.HalfEdges.Count);

            //var start = vertex.HalfEdges.Single(h => h.Start != null);
            //Assert.IsNull(start.End);
            //Assert.Equal(new Point(2, 3), start.Left);
            //Assert.Equal(new Point(6, 4), start.Right);

            //var ends = vertex.HalfEdges.Where(h => h.End != null);
            //Assert.Equal(2, ends.Count());
            //Assert.True(!ends.Contains(start));
            //Assert.True(ends.All(h => h.Start == null));
        }
    }
}