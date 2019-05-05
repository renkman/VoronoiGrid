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
            if(sweepEvent.Arc.Parent == null)
                throw new InvalidOperationException($"CircleEvent: Arc {sweepEvent.Arc.Site}: Parent of node {0} is null");

            var leftParent = sweepEvent.Arc.GetParent(TraverseDirection.CounterClockwise);
            var rightParent = sweepEvent.Arc.GetParent(TraverseDirection.Clockwise);                     
            var left = leftParent?.GetLeaf(TraverseDirection.CounterClockwise);
            var right = rightParent?.GetLeaf(TraverseDirection.Clockwise);
            
            if(left == null || right == null)
                throw new InvalidOperationException($"Neighbor of node {sweepEvent.Arc.Site} is null.");
            
            if (left.CircleEvent != null)
            {
                eventQueue.Remove(left.CircleEvent);
                left.CircleEvent = null;
            }
            if (right.CircleEvent != null)
            {
                eventQueue.Remove(right.CircleEvent);
                right.CircleEvent = null;
            }

            var p = new Point(sweepEvent.Point.X, _calculationService.GetY(sweepEvent.Arc.Site, sweepEvent.Point));
            
            leftParent.HalfEdge.EndPoint = p;
            rightParent.HalfEdge.EndPoint = p;

            var vertex = new Vertex { Point = p };

            ConnectHalfEdgeWithVertex(leftParent.HalfEdge, vertex, (e, v) => e.End = v);
            ConnectHalfEdgeWithVertex(rightParent.HalfEdge, vertex, (e, v) => e.End = v);
            
            // Add third half edge
            var halfEdge = new HalfEdge(p, left.Site, right.Site);

            ConnectHalfEdgeWithVertex(halfEdge, vertex, (e, v) => e.Start = v);
            Logger.Instance.Log($"Left parent: {leftParent.HalfEdge}");
            Logger.Instance.Log($"Right parent: {rightParent.HalfEdge}");
            var higher = sweepEvent.Arc.GetFirstParent(leftParent, rightParent);
            Logger.Instance.Log($"Higher node: {higher}");
            Logger.Instance.Log($"Higher edge old: {higher.HalfEdge}");
            higher.HalfEdge = halfEdge;

            if (vertex.HalfEdges.Count != 3)
            {
                var message = $"Halfedge {halfEdge} is already connected to Vertex {vertex}";
                Logger.Instance.Log(message);
                Logger.Instance.ToFile();
                throw new InvalidOperationException(message);
            }

            Logger.Instance.Log($"CircleEvent: Remove Arc {sweepEvent.Arc.Site}");
            beachLine.RemoveLeaf(sweepEvent.Arc);
            
            var circleEvents = beachLine.GenerateCircleEvent(new[] { left, right }, sweepEvent.Point.Y);
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