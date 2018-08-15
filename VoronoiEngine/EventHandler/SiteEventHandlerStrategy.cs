using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.EventHandler
{
    public class SiteEventHandlerStrategy : IEventHandlerStrategy<SiteEvent, HalfEdge>
    {
        public HalfEdge HandleEvent(SiteEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine)
        {
            var circleEvent = beachLine.FindCircleEventAbove(sweepEvent.Point);
            if (circleEvent != null)
            {
                Logger.Instance.Log($"SiteEvent: {sweepEvent.Point.ToString()}: Remove CircleEvent {circleEvent.Point.ToString()} from queue.");
                eventQueue.Remove(circleEvent);
            }

            Logger.Instance.Log($"SiteEvent: {sweepEvent.Point.ToString()}: Insert site into beach line");
            var halfEdge = beachLine.InsertSite(sweepEvent.Point);

            var circleEvents = beachLine.GenerateCircleEvent(sweepEvent.Point);
            eventQueue.Insert(circleEvents);
            return halfEdge;
        }
    }
}