using VoronoiEngine.Elements;
using VoronoiEngine.EventHandler;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;
using Xunit;

namespace VoronoiTests
{
    public class SiteEventHandlerStrategyTest
    {
        private EventQueue _eventQueue;
        private BeachLine _beachLine;
        private SiteEventHandlerStrategy _strategy;
       
        public SiteEventHandlerStrategyTest()
        {
            _eventQueue = new EventQueue();
            _beachLine = new BeachLine(10);
            _strategy = new SiteEventHandlerStrategy();
        }

        [Fact]
        public void TestHandleSingleEvent()
        {
            var siteEvent = new SiteEvent
            {
                Point = new Point(4, 6)
            };

            var edge = _strategy.HandleEvent(siteEvent, _eventQueue, _beachLine);

            Assert.Empty(edge);
        }

        [Fact]
        public void TestHandleTwoEvents()
        {
            var siteEvents = new SiteEvent[] {
                new SiteEvent
                {
                    Point = new Point(4, 6)
                },
                new SiteEvent
                {
                    Point = new Point(6, 4)
                }
            };

            _strategy.HandleEvent(siteEvents[0], _eventQueue, _beachLine);
            var edges = _strategy.HandleEvent(siteEvents[1], _eventQueue, _beachLine).Cast<HalfEdge>().ToList();

            Assert.NotNull(edges);
            Assert.Equal(siteEvents[0].Point, edges[0].Left);
            Assert.Equal(siteEvents[1].Point, edges[0].Right);
        }

        //[Fact]
        //[Ignore("No cirlce event found, due to clock counterwise check")]
        //public void TestHandleThreeEvents()
        //{
        //    var siteEvents = new SiteEvent[] {
        //        new SiteEvent
        //        {
        //            Point = new Point(4, 6)
        //        },
        //        new SiteEvent
        //        {
        //            Point = new Point(6, 4)
        //        },
        //        new SiteEvent
        //        {
        //            Point = new Point(2, 3)
        //        }
        //    };

        //    var edges = siteEvents.Select(e => _strategy.HandleEvent(e, _eventQueue, _beachLine)).Where(r => r != null).ToList();

        //    Assert.NotNull(edges);
        //    Assert.Equal(2, edges.Count());
        //    Assert.Equal(siteEvents[0].Point, edges[0].Left);
        //    Assert.Equal(siteEvents[1].Point, edges[0].Right);            
        //    Assert.Equal(siteEvents[2].Point, edges[1].Left);
        //    Assert.Equal(siteEvents[0].Point, edges[1].Right);

        //    Assert.Equal(1, _eventQueue.Count);
        //    var circleEvent = _eventQueue.GetNextEvent() as CircleEvent;
        //    Assert.NotNull(circleEvent);
        //    Assert.Equal(2, circleEvent.Edges.Count);
        //    Assert.Equal(new Point(4, 4), circleEvent.Vertex);
        //    Assert.Equal(new Point(4, 2), circleEvent.Point);
        //    Assert.Equal(new Point(2, 3), circleEvent.LeftArc.Site);
        //    Assert.Equal(new Point(6, 4), circleEvent.RightArc.Site);
        //    Assert.Equal(new Point(4, 6), circleEvent.CenterArc.Site);
        //}
    }
}
