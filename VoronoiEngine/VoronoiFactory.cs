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
        private readonly IEventHandlerStrategy<SiteEvent> _siteEventHandler;
        private readonly IEventHandlerStrategy<CircleEvent> _circleEventHandler;
        private readonly IBoundingBoxService _boundingBoxService;

        private readonly Logger _logger;

        public VoronoiFactory() : this(
            new SiteEventHandlerStrategy(),
            new CircleEventHandlerStrategy(),
            new RandomSiteGenerator(),
            new BoundingBoxService())
        {
        }

        public VoronoiFactory(ISiteGenerator siteGenerator) : this(
            new SiteEventHandlerStrategy(),
            new CircleEventHandlerStrategy(),
            siteGenerator,
            new BoundingBoxService())
        {
        }

        public VoronoiFactory(
            IEventHandlerStrategy<SiteEvent> siteEventHandler,
            IEventHandlerStrategy<CircleEvent> circleEventHandler,
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
            _beachLine = new BeachLine(height);

            try
            {
                var sites = _siteGenerator.GenerateSites(width, height, pointQuantity);
                return CreateVoronoiMap(height, width, sites);
            }
            catch(Exception e)
            {
                _logger.Log(e.Message);
                _logger.Log(e.StackTrace);
                _logger.ToFile();

                return null;
            }
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
            _beachLine = new BeachLine(height);
            var map = new VoronoiMap();

            _eventQueue.Initialize(sites.Select(s => s.Point));
            map.AddRange(sites.Cast<IGeometry>());

            while (_eventQueue.HasEvents)
            {
                var sweepEvent = _eventQueue.GetNextEvent();

                var siteEvent = sweepEvent as SiteEvent;
                if (siteEvent != null)
                {
                    _logger.Log($"Sweepline SiteEvent: {siteEvent.Point}");
                    var halfEdges = _siteEventHandler.HandleEvent(siteEvent, _eventQueue, _beachLine);
                    map.AddRange(halfEdges);

                    foreach (var geo in halfEdges)
                        _logger.Log($"Add {geo}");

                    continue;
                }

                var circleEvent = sweepEvent as CircleEvent;
                if (circleEvent == null)
                    throw new InvalidOperationException("SweepEvent is neither SiteEvent nor CircleEvent");

                //_logger.Log($"Sweepline CircleEvent: {circleEvent.Point} for arc {circleEvent.Arc.Site}");
                var circleEventResult = _circleEventHandler.HandleEvent(circleEvent, _eventQueue, _beachLine);

                foreach (var geo in circleEventResult)
                    _logger.Log($"Add {geo}");
                
                // Add vertex and new half edge
                map.AddRange(circleEventResult);
            }

            _beachLine.FinishEdge(_beachLine.Root, width);

            //map.FinishEdges(width);

            map.ConnectEdges();

            _logger.Log("Finished Voronoi map creation");

            foreach(var edge in map.Where(g=>g is HalfEdge).Cast<HalfEdge>())
            {
                _logger.Log(edge.ToString());
            }

            return map;
        }
    }
}