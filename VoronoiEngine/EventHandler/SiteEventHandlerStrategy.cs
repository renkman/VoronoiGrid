using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;

namespace VoronoiEngine.EventHandler
{
    public class SiteEventHandlerStrategy : IEventHandlerStrategy<SiteEvent, HalfEdge>
    {
        public ICollection<HalfEdge> HandleEvent(SiteEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine)
        {
            var circleEvent = beachLine.FindCircleEventAbove(sweepEvent.Point);
            if (circleEvent != null)
                eventQueue.Remove(circleEvent);

            var halfEdges = beachLine.InsertSite(sweepEvent.Point);

            var circleEvents = beachLine.GenerateCircleEvent(sweepEvent.Point);
            eventQueue.Insert(circleEvents);
            return halfEdges;
        }
    }
}
