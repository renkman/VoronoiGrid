using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
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
        }

        private BeachLine _beachLine;
        private EventQueue _eventQueue;
        private SiteGenerator _siteGenerator;

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
                    var halfEdges = HandleSiteEvent(siteEvent);
                    map.AddRange(halfEdges.Cast<IGeometry>());
                    continue;
                }

                var circleEvent = sweepEvent as CircleEvent;
                if (circleEvent == null)
                    throw new InvalidOperationException("SweepEvent is neither SiteEvent nor CircleEvent");

                var vertex = HandleCircleEvent(circleEvent);
                map.Add(vertex);
            }

            return map;
        }

        private ICollection<HalfEdge> HandleSiteEvent(SiteEvent siteEvent)
        {
            var circleEvent = _beachLine.FindCircleEventAbove(siteEvent.Point);
            if (circleEvent != null)
                _eventQueue.Remove(circleEvent);

            var halfEdges = _beachLine.InsertSite(siteEvent.Point);

            var circleEvents = _beachLine.GenerateCircleEvent(siteEvent.Point);
            _eventQueue.Insert(circleEvents);
            return halfEdges;
        }

        private Vertex HandleCircleEvent(CircleEvent circleEvent)
        {
            _beachLine.RemoveLeaf(circleEvent.CenterArc);
            var leftCircleEvent = _beachLine.GenerateSingleCircleEvent(circleEvent.LeftArc);
            var rightCircleEvent = _beachLine.GenerateSingleCircleEvent(circleEvent.RightArc);

            _eventQueue.Insert(leftCircleEvent);
            _eventQueue.Insert(rightCircleEvent);
            return new Vertex { Point = circleEvent.Vertex };
        }
    }
}