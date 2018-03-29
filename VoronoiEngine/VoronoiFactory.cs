using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.EventHandler;
using VoronoiEngine.Events;
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

        public VoronoiFactory()
        {
            _siteGenerator = new SiteGenerator();
            _siteEventHandler = new SiteEventHandlerStrategy();
            _circleEventHandler = new CircleEventHandlerStrategy();
        }

        public VoronoiFactory(
            IEventHandlerStrategy<SiteEvent, HalfEdge> siteEventHandler,
            IEventHandlerStrategy<CircleEvent, Vertex> circleEventHandler,
            ISiteGenerator siteGenerator)
        {
            _siteGenerator = siteGenerator;
            _siteEventHandler = siteEventHandler;
            _circleEventHandler = circleEventHandler;
        }
        
        public VoronoiMap CreateVoronoiMap(int x, int y, int pointQuantity)
        {
            _eventQueue = new EventQueue();
            _beachLine = new BeachLine();

            var sites = _siteGenerator.GenerateSites(x, y, pointQuantity);
            return CreateVoronoiMap(sites);
        }

        public VoronoiMap CreateVoronoiMap(IEnumerable<Site> sites)
        {
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
                    var halfEdges = _siteEventHandler.HandleEvent(siteEvent, _eventQueue, _beachLine);
                    map.AddRange(halfEdges);
                    continue;
                }

                var circleEvent = sweepEvent as CircleEvent;
                if (circleEvent == null)
                    throw new InvalidOperationException("SweepEvent is neither SiteEvent nor CircleEvent");

                var vertices = _circleEventHandler.HandleEvent(circleEvent, _eventQueue, _beachLine);
                map.AddRange(vertices);
            }

            return map;
        }
    }
}