using System;
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

            // Only add circle events, that will appear under the sweepline
            //if (leftCircleEvent?.Point.Y < sweepEvent.Point.Y)
                eventQueue.Insert(leftCircleEvent);
            //if (rightCircleEvent?.Point.Y < sweepEvent.Point.Y)
                eventQueue.Insert(rightCircleEvent);

            var vertex = new Vertex { Point = sweepEvent.Vertex };
            foreach (var edge in sweepEvent.Edges)
                ConnectHalfEdgeWithVertex(edge, vertex, (e, v) => e.End = v);

            // Add third half edge
            var breakpoint = parentNode.CalculateBreakpoint(sweepEvent.Point.Y);
            var halfEdge = new HalfEdge(breakpoint, sweepEvent.LeftArc.Site, sweepEvent.RightArc.Site);
            ConnectHalfEdgeWithVertex(halfEdge, vertex, (e, v) => e.Start = v);
            parentNode.HalfEdge = halfEdge;

            return new List<Vertex> { vertex };
        }

        private void ConnectHalfEdgeWithVertex(HalfEdge edge, Vertex vertex, Action<HalfEdge, Vertex> setPoint)
        {
            setPoint(edge, vertex);
            vertex.HalfEdges.Add(edge);
        }
    }
}