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

        public Vertex HandleEvent(CircleEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine)
        {
            var parentNode = beachLine.RemoveLeaf(sweepEvent.CenterArc);

            if (sweepEvent.LeftArc.CircleEvent != null)
            {
                eventQueue.Remove(sweepEvent.LeftArc.CircleEvent);
                sweepEvent.LeftArc.CircleEvent = null;
            }
            if (sweepEvent.RightArc.CircleEvent != null)
            {
                eventQueue.Remove(sweepEvent.RightArc.CircleEvent);
                sweepEvent.RightArc.CircleEvent = null;
            }
                        
            var leftCircleEvent = beachLine.GenerateSingleCircleEvent(sweepEvent.LeftArc);
            var rightCircleEvent = beachLine.GenerateSingleCircleEvent(sweepEvent.RightArc);

            eventQueue.Insert(leftCircleEvent);
            eventQueue.Insert(rightCircleEvent);

            var vertex = new Vertex { Point = sweepEvent.Vertex };
            foreach(var edge in sweepEvent.Edges)
                ConnectHalfEdgeWithVertex(edge, vertex, (e, v) => e.End = v);

            // Add third half edge
            var breakpoint = parentNode.CalculateBreakpoint(sweepEvent.Vertex.Y - 1);
            var halfEdge = new HalfEdge(breakpoint, sweepEvent.LeftArc.Site, sweepEvent.RightArc.Site);
            ConnectHalfEdgeWithVertex(halfEdge, vertex, (e, v) => e.Start = v);
            parentNode.HalfEdge = halfEdge;
            
            return vertex;
        }

        private void ConnectHalfEdgeWithVertex(HalfEdge edge, Vertex vertex, Action<HalfEdge, Vertex> setPoint)
        {
            setPoint(edge, vertex);
            vertex.HalfEdges.Add(edge);
        }

        private void CreateHalfEdge(Point left, Point right, Vertex vertex, Action<HalfEdge, Vertex> setPoint)
        {
            var lowerY = Math.Min(left.Y, right.Y);
            var point = _breakpointCalculationService.CalculateBreakpoint(left, right, lowerY - 1);
            var halfEdge = new HalfEdge(point, left, right);
            setPoint(halfEdge, vertex);
            vertex.HalfEdges.Add(halfEdge);
        }
    }
}