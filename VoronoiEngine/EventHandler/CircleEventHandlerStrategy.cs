using System;
using System.Collections.Generic;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Geomerty;
using VoronoiEngine.Structures;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.EventHandler
{
    public class CircleEventHandlerStrategy : IEventHandlerStrategy<CircleEvent>
    {
        private readonly IBreakpointCalculationService _calculationService;

        public CircleEventHandlerStrategy()
        {
            _calculationService = new BreakpointCalculationService();
        }
        
        public ICollection<IGeometry> HandleEvent(CircleEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine)
        {
            var leftParent = sweepEvent.CenterArc.GetParent(TraverseDirection.CounterClockwise);
            var rightParent = sweepEvent.CenterArc.GetParent(TraverseDirection.Clockwise);                     

            var p = new Point(sweepEvent.Point.X, _calculationService.GetY(sweepEvent.CenterArc.Site, sweepEvent.Point));

            //if (p.Equals(sweepEvent.Vertex))
            //    throw new InvalidOperationException($"Calculated Vertex {p} differs from vertex {sweepEvent.Vertex}");

            var vertex = new Vertex { Point = sweepEvent.Vertex };
            foreach (var edge in sweepEvent.Edges)
                ConnectHalfEdgeWithVertex(edge, vertex, (e, v) => e.End = v);

            // Add third half edge
            var halfEdge = new HalfEdge(p, sweepEvent.LeftArc.Site, sweepEvent.RightArc.Site);

            ConnectHalfEdgeWithVertex(halfEdge, vertex, (e, v) => e.Start = v);
            var higher = sweepEvent.CenterArc.GetFirstParent(leftParent, rightParent);
            higher.HalfEdge = halfEdge;

            if (vertex.HalfEdges.Count != 3)
            {
                var message = $"Halfedge {halfEdge} is already connected to Vertex {vertex}";
                Logger.Instance.Log(message);
                Logger.Instance.ToFile();
                throw new InvalidOperationException(message);
            }

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

            var circleEvents = beachLine.GenerateCircleEvent(new[] { sweepEvent.LeftArc, sweepEvent.RightArc }, sweepEvent.Point.Y);
            eventQueue.Insert(circleEvents);

            return new List<IGeometry> { vertex, halfEdge };
        }

        private void ConnectHalfEdgeWithVertex(HalfEdge edge, Vertex vertex, Action<HalfEdge, Vertex> setPoint)
        {
            setPoint(edge, vertex);
            vertex.HalfEdges.Add(edge);            
        }
    }
}