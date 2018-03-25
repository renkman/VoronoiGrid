using System.Collections.Generic;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;

namespace VoronoiEngine.EventHandler
{
    public class CircleEventHandlerStrategy : IEventHandlerStrategy<CircleEvent, Vertex>
    {
        public ICollection<Vertex> HandleEvent(CircleEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine)
        {
            beachLine.RemoveLeaf(sweepEvent.CenterArc);
            var leftCircleEvent = beachLine.GenerateSingleCircleEvent(sweepEvent.LeftArc);
            var rightCircleEvent = beachLine.GenerateSingleCircleEvent(sweepEvent.RightArc);

            eventQueue.Insert(leftCircleEvent);
            eventQueue.Insert(rightCircleEvent);
            return new List<Vertex> { new Vertex { Point = sweepEvent.Vertex } };
        }
    }
}