using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Geomerty;
using VoronoiEngine.Structures;

namespace VoronoiEngine.EventHandler
{
    public class CircleEventHandlerStrategy : IEventHandlerStrategy<CircleEvent, Vertex>
    {
        private IBreakpointCalculationService _breakpointCalculationService;

        public CircleEventHandlerStrategy()
        {
            _breakpointCalculationService = new BreakpointCalculationService();
        }

        public ICollection<Vertex> HandleEvent(CircleEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine)
        {
            beachLine.RemoveLeaf(sweepEvent.CenterArc);

            if (sweepEvent.LeftArc.CircleEvent != null)
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

            CreateHalfEdge(sweepEvent.LeftArc.Site, sweepEvent.CenterArc.Site, vertex, (h, p) => h.End = p);
            CreateHalfEdge(sweepEvent.CenterArc.Site, sweepEvent.RightArc.Site, vertex, (h, p) => h.End = p);
            CreateHalfEdge(sweepEvent.LeftArc.Site, sweepEvent.RightArc.Site, vertex, (h, p) => h.Start = p);

            return new List<Vertex> { vertex };
        }

        private void CreateHalfEdge(Point left, Point right, Vertex vertex, Action<HalfEdge, Vertex> setPoint)
        {
            var lowerY = Math.Min(left.Y, right.Y);
            var point = _breakpointCalculationService.CalculateBreakpoint(left, right, lowerY - 1);
            var halfEdge = new HalfEdge(point);
            setPoint(halfEdge, vertex);
            vertex.HalfEdges.Add(halfEdge);
        }
    }
}