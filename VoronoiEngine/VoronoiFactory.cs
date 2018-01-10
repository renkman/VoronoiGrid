using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;

namespace VoronoiEngine
{
    public class VoronoiFactory
    {
        private BeachLine _beachLine;
        private EventQueue _eventQueue;

        public VoronoiMap CreateVoronoiMap(int x, int y, int pointQuantity)
        {
            _eventQueue = new EventQueue();
            _beachLine = new BeachLine();
            return new VoronoiMap();
        }

        public VoronoiMap CreateVoronoiMap(IEnumerable<Point> sites)
        {
            _eventQueue = new EventQueue();
            _beachLine = new BeachLine();
            var map = new VoronoiMap();

            _eventQueue.Initialize(sites);

            while(_eventQueue.HasEvents)
            {
                var sweepEvent = _eventQueue.GetNextEvent();

                var siteEvent = sweepEvent as SiteEvent;
                if (siteEvent != null)
                    HandleSiteEvent(siteEvent);

                var circleEvent = sweepEvent as CircleEvent;
                if (circleEvent == null)
                    throw new InvalidOperationException("sweepEvent is neither SiteEvent nor CircleEvent");

                var vertex = HandleCircleEvent(circleEvent);
                map.Add(vertex);
            }

            return new VoronoiMap();
        }

        private void HandleSiteEvent(SiteEvent siteEvent)
        {
            var circleEvent = _beachLine.FindCircleEventAbove(siteEvent.Point);
            if (circleEvent != null)
                _eventQueue.Remove(circleEvent);

            _beachLine.InsertSite(siteEvent.Point);

            _beachLine.GenerateCircleEvent(siteEvent.Point);
        }

        private Vertex HandleCircleEvent(CircleEvent circleEvent)
        {
            _beachLine.RemoveLeaf(circleEvent.Arc);
            return new Vertex { Point = circleEvent.Vertex };
        }
    }
}
