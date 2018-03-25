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
        private static VoronoiFactory _instance;

        public static VoronoiFactory Instance
        {
            get
            {
                return _instance ?? (_instance = new VoronoiFactory());
            }
        }

        private VoronoiFactory()
        {
            _siteGenerator = new SiteGenerator();
            _siteEventHandler = new SiteEventHandlerStrategy();
            _circleEventHandler = new CircleEventHandlerStrategy();
        }

        private BeachLine _beachLine;
        private EventQueue _eventQueue;
        private SiteGenerator _siteGenerator;

        private IEventHandlerStrategy<SiteEvent, HalfEdge> _siteEventHandler;
        private IEventHandlerStrategy<CircleEvent, Vertex> _circleEventHandler;

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

            _eventQueue.Initialize(sites.Select(s=>s.Point));
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