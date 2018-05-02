using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.EventHandler;
using VoronoiEngine.Events;
using VoronoiEngine.Geomerty;
using VoronoiEngine.Structures;
using VoronoiEngine.Utilities;

namespace VoronoiEngine
{
    public class VoronoiFactory
    {
        private BeachLine _beachLine;
        private EventQueue _eventQueue;

        private readonly ISiteGenerator _siteGenerator;
        private readonly IEventHandlerStrategy<SiteEvent, HalfEdge> _siteEventHandler;
        private readonly IEventHandlerStrategy<CircleEvent, Vertex> _circleEventHandler;
        private readonly IBoundingBoxService _boundingBoxService;

        private readonly Logger _logger;

        public VoronoiFactory() : this(
            new SiteEventHandlerStrategy(), 
            new CircleEventHandlerStrategy(), 
            new SiteGenerator(), 
            new BoundingBoxService())
        {
        }

        public VoronoiFactory(
            IEventHandlerStrategy<SiteEvent, HalfEdge> siteEventHandler,
            IEventHandlerStrategy<CircleEvent, Vertex> circleEventHandler,
            ISiteGenerator siteGenerator,
            IBoundingBoxService boundingBoxService)
        {
            _siteGenerator = siteGenerator;
            _siteEventHandler = siteEventHandler;
            _circleEventHandler = circleEventHandler;
            _boundingBoxService = boundingBoxService;
            _logger = Logger.Instance;
        }

        public VoronoiMap CreateVoronoiMap(int height, int width, int pointQuantity)
        {
            _eventQueue = new EventQueue();
            _beachLine = new BeachLine();

            var sites = _siteGenerator.GenerateSites(width, height, pointQuantity);
            return CreateVoronoiMap(height, width, sites);
        }

        public VoronoiMap CreateVoronoiMap(int height, int width, IEnumerable<Site> sites)
        {
            if (sites == null)
                throw new ArgumentNullException("sites");

            if (sites.GroupBy(s => s.Point).Count() != sites.Count())
                throw new ArgumentException("Multiple sites with the same coordinates passed!");

            var siteList = string.Join(", ", sites.Select(s => s.Point.ToString()).ToArray());
            _logger.Log($"Create Voronoi map with sites: {siteList}");

            _eventQueue = new EventQueue();
            _beachLine = new BeachLine();
            var map = new VoronoiMap();

            _eventQueue.Initialize(sites.Select(s => s.Point));
            map.AddRange(sites.Cast<IGeometry>());

            while (_eventQueue.HasEvents)
            {
                var sweepEvent = _eventQueue.GetNextEvent();

                var siteEvent = sweepEvent as SiteEvent;
                if (siteEvent != null)
                {
                    _logger.Log($"Sweepline SiteEvent: {siteEvent.Point.ToString()}");
                    _siteEventHandler.HandleEvent(siteEvent, _eventQueue, _beachLine);
                    _logger.Log(_beachLine.ToString());
                    continue;
                }

                var circleEvent = sweepEvent as CircleEvent;
                if (circleEvent == null)
                    throw new InvalidOperationException("SweepEvent is neither SiteEvent nor CircleEvent");

                _logger.Log($"Sweepline CircleEvent: {circleEvent.Point.ToString()}");
                var vertex = _circleEventHandler.HandleEvent(circleEvent, _eventQueue, _beachLine);
                var halfEdges = String.Join(", ", vertex.HalfEdges.Select(h => h.Point.ToString()).ToArray());
                _logger.Log($"Add Vertex: {vertex.Point}, Half Edges: {halfEdges}");
                map.Add(vertex);
            }

            //var openEnds = map.Where(g => g is HalfEdge).Cast<HalfEdge>().ToList();
            //foreach(var halfEdge in openEnds)
            //{
            //    _boundingBoxService.AttachHalfEdge(halfEdge, height, width);
            //}

            _logger.Log("Finished Voronoi map creation");

            return map;
        }
    }
}