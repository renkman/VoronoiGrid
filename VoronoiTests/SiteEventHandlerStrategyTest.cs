using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoronoiEngine.Elements;
using VoronoiEngine.EventHandler;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;

namespace VoronoiTests
{
    [TestFixture]
    public class SiteEventHandlerStrategyTest
    {
        private EventQueue _eventQueue;
        private BeachLine _beachLine;
        private SiteEventHandlerStrategy _strategy;
       
        [SetUp]
        public void Setup()
        {
            _eventQueue = new EventQueue();
            _beachLine = new BeachLine(10);
            _strategy = new SiteEventHandlerStrategy();
        }

        [Test]
        public void TestHandleSingleEvent()
        {
            var siteEvent = new SiteEvent
            {
                Point = new Point(4, 6)
            };

            var edge = _strategy.HandleEvent(siteEvent, _eventQueue, _beachLine);

            Assert.IsEmpty(edge);
        }

        [Test]
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

            Assert.IsNotNull(edges);
            Assert.AreEqual(siteEvents[0].Point, edges[0].Left);
            Assert.AreEqual(siteEvents[1].Point, edges[0].Right);
        }

        //[Test]
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

        //    Assert.IsNotNull(edges);
        //    Assert.AreEqual(2, edges.Count());
        //    Assert.AreEqual(siteEvents[0].Point, edges[0].Left);
        //    Assert.AreEqual(siteEvents[1].Point, edges[0].Right);            
        //    Assert.AreEqual(siteEvents[2].Point, edges[1].Left);
        //    Assert.AreEqual(siteEvents[0].Point, edges[1].Right);

        //    Assert.AreEqual(1, _eventQueue.Count);
        //    var circleEvent = _eventQueue.GetNextEvent() as CircleEvent;
        //    Assert.IsNotNull(circleEvent);
        //    Assert.AreEqual(2, circleEvent.Edges.Count);
        //    Assert.AreEqual(new Point(4, 4), circleEvent.Vertex);
        //    Assert.AreEqual(new Point(4, 2), circleEvent.Point);
        //    Assert.AreEqual(new Point(2, 3), circleEvent.LeftArc.Site);
        //    Assert.AreEqual(new Point(6, 4), circleEvent.RightArc.Site);
        //    Assert.AreEqual(new Point(4, 6), circleEvent.CenterArc.Site);
        //}
    }
}
