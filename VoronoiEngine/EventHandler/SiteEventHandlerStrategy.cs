using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.EventHandler
{
    public class SiteEventHandlerStrategy : IEventHandlerStrategy<SiteEvent>
    {
        public ICollection<IGeometry> HandleEvent(SiteEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine)
        {
            var circleEvent = beachLine.FindCircleEventAbove(sweepEvent.Point);
            if (circleEvent != null)
            {
                Logger.Instance.Log($"SiteEvent: {sweepEvent.Point.ToString()}: Remove CircleEvent {circleEvent.Point} from queue.");
                eventQueue.Remove(circleEvent);
            }

            Logger.Instance.Log($"SiteEvent: {sweepEvent.Point.ToString()}: Insert site into beach line");
            var result = beachLine.InsertSite(sweepEvent.Point);

            var circleEvents = beachLine.GenerateCircleEvent(result?.Leaves, sweepEvent.Point.Y);
            eventQueue.Insert(circleEvents);
            
            return result?.HalfEdges?.Cast<IGeometry>().ToList();
        }
    }
}