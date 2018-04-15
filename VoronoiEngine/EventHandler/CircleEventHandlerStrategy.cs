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

            if(sweepEvent.LeftArc.CircleEvent != null)
            {
                eventQueue.Remove(sweepEvent.LeftArc.CircleEvent);
            }
            if (sweepEvent.RightArc.CircleEvent != null)
            {
                eventQueue.Remove(sweepEvent.RightArc.CircleEvent);
            }

            var leftCircleEvent = beachLine.GenerateSingleCircleEvent(sweepEvent.LeftArc);
            var rightCircleEvent = beachLine.GenerateSingleCircleEvent(sweepEvent.RightArc);

            eventQueue.Insert(leftCircleEvent);
            eventQueue.Insert(rightCircleEvent);

            var vertex = new Vertex { Point = sweepEvent.Vertex };
            var parent = sweepEvent.Parent;
            vertex.Edges.Add(parent.Edges.Left);
            vertex.Edges.Add(parent.Edges.Right);

            return new List<Vertex> { vertex };
        }
    }
}