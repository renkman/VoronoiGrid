using System;
using System.Collections.Generic;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.EventHandler
{
    public class CircleEventHandlerStrategy : IEventHandlerStrategy<CircleEvent>
    {
        public ICollection<IGeometry> HandleEvent(CircleEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine)
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

            var leftCircleEvent = beachLine.GenerateSingleCircleEvent(sweepEvent.LeftArc, sweepEvent.Point.Y);
            var rightCircleEvent = beachLine.GenerateSingleCircleEvent(sweepEvent.RightArc, sweepEvent.Point.Y);

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

            if (vertex.HalfEdges.Count != 3)
            {
                var message = $"Halfedge {halfEdge} is already connected to Vertex {vertex}";
                Logger.Instance.Log(message);
                Logger.Instance.ToFile();
                throw new InvalidOperationException(message);
            }
            return new List<IGeometry> { vertex, halfEdge };
        }

        private void ConnectHalfEdgeWithVertex(HalfEdge edge, Vertex vertex, Action<HalfEdge, Vertex> setPoint)
        {
            setPoint(edge, vertex);
            vertex.HalfEdges.Add(edge);            
        }
    }
}